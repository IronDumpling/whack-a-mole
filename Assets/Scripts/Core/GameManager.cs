using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour, IAgent
{
    // Returns the GameManager.
    public static GameManager Instance => s_Instance;
    private static GameManager s_Instance;

    // State Machine
    StateMachine m_StateMachine;
    State[] m_States = { new StartState(), new ProgressState(),
                        new PausedState(), new OverState() };

    // Manage all holes
    public StateMachine[] m_HoleStateMahines;
    public Gopher gopherPrefab;

    // Use this for initialization
    void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        s_Instance = this;
        DontDestroyOnLoad(this);

        // Initialize state mchine
        m_StateMachine = new StateMachine(this);
        m_StateMachine.AddStates(m_States);
        m_StateMachine.SetDefault((int)GameStateType.Start);
    }

    void Start()
    {
        GameObject holes = GameObject.Find("Holes");
        GameObject[] m_Holes = new GameObject[holes.transform.childCount];
        for (int i = 0; i < m_Holes.Length; i++)
        {
            m_Holes[i] = holes.transform.GetChild(i).gameObject;
        }

        m_HoleStateMahines = new StateMachine[holes.transform.childCount]; 
        for (int i = 0; i < m_Holes.Length; i++)
        {
            m_HoleStateMahines[i] = m_Holes[i].GetComponent<Hole>().m_StateMachine;
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_StateMachine.Update();
    }

    public void SpawnGophers(ref int currGopherNum)
    {
        int spawnNum = Random.Range(1, 3);
        currGopherNum += spawnNum;

        StateMachine[] emptyHoles = m_HoleStateMahines
            .Where(sm => sm.m_CurrState.GetType() == typeof(EmptyState)).ToArray();

        emptyHoles = emptyHoles.OrderBy(x => Random.value).Take(spawnNum).ToArray();

        foreach (StateMachine hole in emptyHoles)
        {
            ((Hole)hole.m_Agent).m_Gopher = Instantiate(gopherPrefab,
                    new Vector3(((Hole)hole.m_Agent).transform.position.x,
                                ((Hole)hole.m_Agent).transform.position.y,
                                ((Hole)hole.m_Agent).transform.position.z - 1),
                    Quaternion.identity);
        }
    }
}

public enum GameStateType
{
    Start,
    Progress,
    Paused,
    Over
}

public class StartState : State
{
    GameObject panel;
    GameObject startMenu;

    public StartState()
    {
        m_StateType = (int)GameStateType.Start;
    }

    public override void Enter()
    {
        base.Enter();
        panel = GameObject.Find("Panel");
        startMenu = panel.transform.Find("StartState").gameObject;
        startMenu.SetActive(true);
    }

    public override State Execute()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            return m_StateMachine.Transition((int)GameStateType.Progress);
        else
            return this;
    }

    public override void Exit()
    {
        base.Exit();
        panel = GameObject.Find("Panel");
        startMenu = panel.transform.Find("StartState").gameObject;
        startMenu.SetActive(false);
    }
}

public class ProgressState : State
{
    // Gopher Management
    const int TOTAL_GOPHER_NUM = 20;
    int currGopherNum = 0;
    public GameObject gopher;

    // Timer
    private float timer = 0f;
    private const float SPAWN_INTERVAL = 3f;

    public ProgressState()
    {
        m_StateType = (int)GameStateType.Progress;
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0f;
        currGopherNum = 0;
        ScoreManager.Instance.ResetPoints();
    }

    public override State Execute()
    {
        timer += Time.deltaTime;

        if (timer >= (SPAWN_INTERVAL - currGopherNum * 0.1))
        {
            GameManager.Instance.SpawnGophers(ref currGopherNum);
            timer = 0f;
        }
            
        if(currGopherNum < TOTAL_GOPHER_NUM)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                return m_StateMachine.Transition((int)GameStateType.Paused);
            else
                return this;
        }
        else
            return m_StateMachine.Transition((int)GameStateType.Over);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class PausedState : State
{
    public PausedState()
    {
        m_StateType = (int)GameStateType.Paused;
    }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0f;
    }

    public override State Execute()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            return m_StateMachine.Transition((int)GameStateType.Progress);
        else
            return this;
    }

    public override void Exit()
    {
        base.Exit();
        Time.timeScale = 1f;
    }
}

public class OverState : State
{
    GameObject panel;
    GameObject overMenu;

    public OverState()
    {
        m_StateType = (int)GameStateType.Over;
    }

    public override void Enter()
    {
        base.Enter();
        panel = GameObject.Find("Panel");
        overMenu = panel.transform.Find("OverState").gameObject;
        overMenu.SetActive(true);
        overMenu.transform.GetChild(1).gameObject.GetComponent<UpdateScore>().ShowScore();
    }

    public override State Execute()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            return m_StateMachine.Transition((int)GameStateType.Progress);
        else
            return this;
    }

    public override void Exit()
    {
        base.Exit();
        panel = GameObject.Find("Panel");
        overMenu = panel.transform.Find("OverState").gameObject;
        overMenu.SetActive(false);
    }
}



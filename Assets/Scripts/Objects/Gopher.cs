using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class Gopher : MonoBehaviour, IAgent
{
    // State Machine
    public StateMachine m_StateMachine;
    State[] states = { new RedState(), new GreenState(), new DeadState() };

    // Material
    Material redMaterial;
    Material greenMaterial;

    // Button click
    public bool isClicked = false;

    // Start is called before the first frame update
    void Awake()
    {
        // Material
        redMaterial = Resources.Load<Material>("Materials/Red");
        greenMaterial = Resources.Load<Material>("Materials/Green");

        // Initialize state mchine
        m_StateMachine = new StateMachine(this);
        m_StateMachine.AddStates(states);
        m_StateMachine.SetDefault((int)GStateType.Red);
    }

    // Update is called once per frame
    void Update()
    {
        m_StateMachine.Update();
    }

    public void SetMaterial(int color)
    {
        if(color == (int)GStateType.Red)
        {
            GetComponent<SpriteRenderer>().material = redMaterial;
        }
        else if (color == (int)GStateType.Green)
        {
            GetComponent<SpriteRenderer>().material = greenMaterial;
        }
    }

    public void DestroyGopher()
    {
        Destroy(gameObject);
    }
}

public enum GStateType
{
    Red,
    Green,
    Dead
}

public class RedState: State
{
    // Timer
    private float timer = 0f;
    private const float RED_LIFE_SPAN = 6f;

    public RedState()
    {
        m_StateType = (int)GStateType.Red;
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0f;
        ((Gopher)m_Agent).SetMaterial((int)GStateType.Red);
    }

    public override State Execute()
    {
        timer += Time.deltaTime;

        if (((Gopher)m_Agent).isClicked)
        {
            if (timer < 0.3f) ScoreManager.Instance.IncreasePoints(20);
            else if (timer < 0.7f) ScoreManager.Instance.IncreasePoints(10);
            else ScoreManager.Instance.IncreasePoints(5);

            return m_StateMachine.Transition((int)GStateType.Green);
        }
            
        else if (timer < RED_LIFE_SPAN)
            return this;
        else
            return m_StateMachine.Transition((int)GStateType.Dead);
    }
}

public class GreenState : State
{
    // Timer
    private float timer = 0f;
    private const float GREEN_LIFE_SPAN = 0.5f;

    public GreenState()
    {
        m_StateType = (int)GStateType.Green;
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0f;
        ((Gopher)m_Agent).SetMaterial((int)GStateType.Green);
    }

    public override State Execute()
    {
        timer += Time.deltaTime;
      
        if (timer < GREEN_LIFE_SPAN)
            return this;
        else
            return m_StateMachine.Transition((int)GStateType.Dead);
    }
}

public class DeadState : State
{
    public DeadState()
    {
        m_StateType = (int)GStateType.Dead;
    }

    public override void Enter()
    {
        base.Enter();
        ((Gopher)m_Agent).isClicked = false;
        ((Gopher)m_Agent).DestroyGopher();
    }
}
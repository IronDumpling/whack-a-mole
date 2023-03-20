using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Hole : MonoBehaviour, IAgent
{
    // State Machine
    public StateMachine m_StateMachine;
    State[] m_States = { new EmptyState(), new FullState() };

    // gopher
    public Gopher m_Gopher;

    // Start is called before the first frame update
    void Awake()
    {
        // Initialize state mchine
        m_StateMachine = new StateMachine(this);
        m_StateMachine.AddStates(m_States);
        m_StateMachine.SetDefault((int)HStateType.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        m_StateMachine.Update();
    }

    // Button clicked
    public void OnButtonClick()
    {
        if( m_StateMachine.m_CurrState.GetType() == typeof(FullState) &&
            m_Gopher.m_StateMachine.m_CurrState.GetType() == typeof(RedState))
        {
            m_Gopher.isClicked = true;
        }
    }
}

public enum HStateType
{
    Empty,
    Full
}

public class EmptyState : State
{
    private Hole m_Hole;

    public EmptyState()
    {
        m_StateType = (int)HStateType.Empty;
    }

    public override State Execute()
    {
        m_Hole = (Hole)m_Agent;
        if (m_Hole.m_Gopher != null)
            return m_StateMachine.Transition((int)HStateType.Full);
        else
            return this;
    }
}

public class FullState : State
{
    private Hole m_Hole;

    public FullState()
    {
        m_StateType = (int)HStateType.Full;
    }

    public override State Execute()
    {
        m_Hole = (Hole)m_Agent;
        if (m_Hole.m_Gopher == null)
            return m_StateMachine.Transition((int)HStateType.Empty);
        else
            return this;
    }
}

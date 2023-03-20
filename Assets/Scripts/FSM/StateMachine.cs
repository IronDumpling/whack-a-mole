using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine
{
    public IAgent m_Agent;
    public Dictionary<int, State> m_StateDict = new Dictionary<int, State>();
    public State m_CurrState;

    public StateMachine(IAgent agent)
    {
        m_Agent = agent;
    }

    public void SetDefault(int key)
    {
        // m_CurrState = m_StateDict[key];
        // m_CurrState.Enter();
        if (m_StateDict.TryGetValue(key, out m_CurrState)) // 保证安全
            m_CurrState.Enter();
    }

    public void AddStates(State[] states)
    {
        foreach (State state in states)
        {
            AddState(state);
        }
    }

    public void AddState(State state)
    {
        state.m_Agent = m_Agent;
        state.SetStateMachine(this);
        m_StateDict[state.m_StateType] = state;
    }

    public State Transition(int key)
    {
        State s;
        m_StateDict.TryGetValue(key, out s);
        return s;
    }

    public void Update()
    {
        if (m_CurrState == null) return;

        State nextState = m_CurrState.Execute();

        if(nextState != m_CurrState)
        {
            m_CurrState.Exit(); // 先退后进
            m_CurrState = nextState;
            if (m_CurrState != null)
                m_CurrState.Enter();
        }
    }
}


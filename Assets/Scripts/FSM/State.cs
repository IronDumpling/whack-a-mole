using UnityEngine;
using System.Collections;

public abstract class State
{
    public int m_StateType { get; protected set; }
    protected StateMachine m_StateMachine;
    public IAgent m_Agent;

    public void SetStateMachine(StateMachine machine)
    {
        m_StateMachine = machine;
    }

    public virtual void Enter() { }

    public virtual State Execute() { return null; }

    public virtual void Exit() { }
}


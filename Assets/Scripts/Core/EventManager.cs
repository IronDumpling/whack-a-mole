using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Returns the EventManager
    public static EventManager Instance => s_Instance;
    private static EventManager s_Instance;

    // Event
    public delegate void ClickGopher();
    public ClickGopher clickEvent;

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
    }

    void Click()
    {
        if (clickEvent != null)
        {
            clickEvent();
        }
    }
}


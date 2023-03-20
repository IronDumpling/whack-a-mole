using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    // Returns the PointsManager.
    public static ScoreManager Instance => s_Instance;
    static ScoreManager s_Instance;

    [SerializeField] int m_Score = 0;
    public UnityEvent<int> OnScoreChange;

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

    public void IncreasePoints(int score)
    {
        m_Score += score;
        OnScoreChange.Invoke(m_Score);
    }

    public void DecreasePoints(int score)
    {
        m_Score -= score;
        OnScoreChange.Invoke(m_Score);
    }

    public void ResetPoints()
    {
        m_Score = 0;
        OnScoreChange.Invoke(m_Score);
    }

    public int GetScore()
    {
        return m_Score;
    }
}

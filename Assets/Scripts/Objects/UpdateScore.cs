using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateScore : MonoBehaviour
{
    public void ScoreChange(int currScore)
    {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = $"Score: {currScore}";
    }

    public void ShowScore()
    {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = $"Score: {ScoreManager.Instance.GetScore()}";
    }
}

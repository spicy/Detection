using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public RowUI rowUI;
    public ScoreManager scoreManager;

    void Start()
    {
        scoreManager.AddScore(new Score(999));

        var Score = scoreManager.GetScore().ToArray();
        for (int i = 0; i < Score.length; i++)
        {
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
            row.score.text = Score[i].ToString();
        }
    }
}

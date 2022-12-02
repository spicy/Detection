using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd;
    void Awake()
    {
        var json string = PlayerPerfs.GetString("scores", "{}");
        sd = JsonUtility.FromJson<ScoreData>(json);
    }

    public IEnumerable<score> GetScores()
    {
        return sd.Score.OrderByDescending();
    }

    public void AddScore(score Scores)
    {
        sd.Score.Add(Scores);
    }

    private void OnDestroy()
    {
        SaveScore();
    }

    public void SaveScore()
    { 
      var json string = JsonUtility.ToJson(sd);
        PlayerPrefs.SetString("scores", json);
    }
}

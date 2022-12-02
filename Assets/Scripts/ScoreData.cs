using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ScoreData 
{
    public List<score> Score;

    public ScoreData()
    {
        Score = new List<score>();
    }
}

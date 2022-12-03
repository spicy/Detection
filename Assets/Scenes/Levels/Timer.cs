using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Text timertext;
    private float startTime;
    private bool finished = false;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (finished)
            return;

        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f3");

        timertext.text = minutes + ":" + seconds;
    }

    public void Finished()
    {
        finished = true;
        timertext.color = Color.yellow;
    }
}

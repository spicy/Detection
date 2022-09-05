using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainbow : MonoBehaviour
{
    [Header("Settings")]
    public Camera cam;
    public Color32[] colors;
    // Start is called before the first frame update
    void Start()
    {
        cam = transform.GetComponent<Camera>();
        colors = new Color32[12]
        {
            new Color32(0, 0, 0, 255), // black
            new Color32(28, 28, 43, 255), // dark blue
            new Color32(28, 28, 43, 255), // dark purple
            new Color32(0, 0, 0, 255), // black
            new Color32(9, 17, 23, 255), // dark cyan
            new Color32(0, 0, 0, 255), // black
            new Color32(31, 23, 41, 255), // dark purple
            new Color32(0, 0, 0, 255), // black
            new Color32(20, 6, 6, 255), // dark red
            new Color32(41, 16, 16, 255), // dark red
            new Color32(0, 0, 0, 255), // black
            new Color32(6, 15, 7, 255), // dark green
        };
        StartCoroutine(Cycle());
    }

    public IEnumerator Cycle()
    {
        int startColorIndex = Random.Range(0, colors.Length);
        int endColorIndex = Random.Range(0, colors.Length);

        while (true)
        {
            for (float interpolant = 0; interpolant < 1f; interpolant += 0.01f)
            {
                cam.backgroundColor = Color.Lerp(colors[startColorIndex], colors[endColorIndex], interpolant);
                yield return null;
            }

            startColorIndex = endColorIndex;
            endColorIndex = Random.Range(0, colors.Length);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        Appear();
    }

    public void Appear()
    {
        StartCoroutine(FadeIn());
    }


    public void Disappear()
    {
        StartCoroutine(FadeOut());
    }

    //-1 showing --- 1 invisible
    private IEnumerator FadeIn()
    {
        float amt = material.GetFloat("_dissolveAmount");
        for (; amt > -1; amt -= 0.03f)
        { 
            material.SetFloat("_dissolveAmount", amt);
            yield return null;
        }
        Disappear();
    }

    private IEnumerator FadeOut()
    {
        float amt = material.GetFloat("_dissolveAmount");
        for (; amt < 1; amt += 0.03f)
        {
            material.SetFloat("_dissolveAmount", amt);
            yield return null;
        }
        Appear();
    }
}

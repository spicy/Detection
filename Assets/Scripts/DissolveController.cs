using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    [SerializeField] private List<SkinnedMeshRenderer> skinnedMeshRenderers;
    [SerializeField] private List<MeshRenderer> meshRenderers;
    private List<Material> materials;
    private bool coroutineRunning = false;
    private enum LastUsed
    {
        None,
        Appear,
        Disappear
    }

    private LastUsed lastUsed = LastUsed.None;

    public void Start()
    {
        materials = new List<Material>();
        foreach (SkinnedMeshRenderer smr in skinnedMeshRenderers)
        {
            foreach (Material mat in smr.materials)
            {
                materials.Add(mat);
            }
        }

        foreach (MeshRenderer mr in meshRenderers)
        {
            foreach (Material mat in mr.materials)
            {
                materials.Add(mat);
            }
        }
    }

    public void Appear()
    {
        if (!coroutineRunning && lastUsed != LastUsed.Appear)
        {
            lastUsed = LastUsed.Appear;
            foreach (Material mat in materials)
            {
                StartCoroutine(FadeIn(mat));
            }
        }
    }

    public void Disappear()
    {
        new WaitForSeconds(1f);
        if (!coroutineRunning && lastUsed != LastUsed.Disappear)
        {
            lastUsed = LastUsed.Disappear;
            foreach (Material mat in materials)
            {
                StartCoroutine(FadeOut(mat));
            }
        }
    }

    //-1 showing --- 1 invisible
    private IEnumerator FadeIn(Material mat)
    {
        float amt = mat.GetFloat("_dissolveAmount");
        while (amt > -1)
        {
            coroutineRunning = true;
            amt -= 0.1f;
            mat.SetFloat("_dissolveAmount", amt);
            yield return new WaitForSeconds(0.01f);
        }
        coroutineRunning = false;
    }

    private IEnumerator FadeOut(Material mat)
    {
        float amt = mat.GetFloat("_dissolveAmount");
        while (amt < 1)
        {
            coroutineRunning = true;
            amt += 0.1f;
            mat.SetFloat("_dissolveAmount", amt);
            yield return new WaitForSeconds(0.01f);
        }
        coroutineRunning = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFrustumCulling : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.bounds = new Bounds(transform.position, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
    }
}

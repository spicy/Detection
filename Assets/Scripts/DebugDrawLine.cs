using UnityEngine;

public class DebugDrawLine : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 25f);
    }
}

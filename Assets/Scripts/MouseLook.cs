using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public InputActionReference horizontalLook;
    public InputActionReference verticalLook;
    public float lookSpeed = 1f;
    public Transform cameraTransform;
    float pitch;
    float yaw;
    // Start is called before the first frame update
    void Start()
    {
        
    }

}

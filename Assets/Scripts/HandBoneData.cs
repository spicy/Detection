using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBoneData : MonoBehaviour
{
    public enum HandModelType { Left , Right };

    public HandModelType handSide;
    public Transform root;
    public Animator animator;
    public Transform[] fingerBones;
}

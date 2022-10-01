using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBoneData : MonoBehaviour
{
    public enum HandModelSide { Left, Right };

    public HandModelSide handSide;
    public Transform root;
    public Animator animator;
    public Transform[] fingerBones;
}

using UnityEngine;

public class HandBoneData : MonoBehaviour
{
    public enum HandModelSide { Left, Right };
    public enum HandModelPose { Primary, Secondary };

    public HandModelSide handSide;
    public HandModelPose poseType;

    public Transform root;
    public Animator animator;
    public Transform[] fingerBones;
}

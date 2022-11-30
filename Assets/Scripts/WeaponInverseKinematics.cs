using UnityEngine;

public class WeaponInverseKinematics : MonoBehaviour
{
    [System.Serializable]
    public class Bone
    {
        public HumanBodyBones bone;
        public float weight = 1.0f;
    }

    [SerializeField] private Transform aimTransform;
    public Transform TargetTransform { get; private set; }
    private Transform[] boneTransforms;
    public int accuracyIterations = 10;
    [Range(0,1)] public float weight;
    public float angleLimit = 90f;
    public float distanceLimit = 1.5f;
    public Bone[] bones;
    
    private void Awake()
    {
        Animator animator = GetComponent<Animator>();
        boneTransforms = new Transform[bones.Length];
        for (int i = 0; i < boneTransforms.Length; ++i)
            boneTransforms[i] = animator.GetBoneTransform(bones[i].bone);
    }

    void LateUpdate()
    {
        if (TargetTransform == null) return;
        if (aimTransform == null) return;

        Vector3 targetPosition = GetTargetPostiion();

        for (int i = 0; i < accuracyIterations; ++i)
        {
            for(int j = 0; j < boneTransforms.Length; ++j)
            {
                float boneWeight = bones[j].weight * weight;
                AimAtTarget(boneTransforms[j], targetPosition, boneWeight);
            }
        }
    }

    private Vector3 GetTargetPostiion()
    {
        Vector3 targetDir = TargetTransform.position - aimTransform.position;
        Vector3 aimDir = aimTransform.forward;

        float meld = 0f;
        float targetAngle = Vector3.Angle(targetDir, aimDir);

        if(targetAngle > angleLimit)
            meld += (targetAngle - angleLimit) / 50f;

        float targetDist = targetDir.magnitude;

        if(targetDist < distanceLimit)
            meld += distanceLimit - targetDist;

        Vector3 dir = Vector3.Slerp(targetDir, aimDir, meld);

        return aimTransform.position + dir;
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDir = aimTransform.forward;
        Vector3 targetdir = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDir, targetdir);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    {
        TargetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }
}

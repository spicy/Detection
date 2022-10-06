using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public struct StartAndEndHandData
{
    public Vector3 startingPosition;
    public Vector3 endingPosition;
    public Quaternion startingRotation;
    public Quaternion endingRotation;
    public Quaternion[] startingFingersRotation;
    public Quaternion[] endingFingersRotation;
}

public class GrabPistolHandPose : MonoBehaviour
{
    [SerializeField] private HandBoneData primaryLeftHandPose;
    [SerializeField] private HandBoneData primaryRightHandPose;
    [SerializeField] private HandBoneData secondaryLeftHandPose;
    [SerializeField] private HandBoneData secondaryRightHandPose;

    [SerializeField] private float poseInterpDuration;

    private StartAndEndHandData leftStartAndEndHandData; 
    private StartAndEndHandData rightStartAndEndHandData;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPoseOnInteraction);
        grabInteractable.selectExited.AddListener(UnsetPoseOnInteraction);

        leftStartAndEndHandData = new StartAndEndHandData();
        rightStartAndEndHandData = new StartAndEndHandData();

        primaryLeftHandPose.gameObject.SetActive(false);
        secondaryLeftHandPose.gameObject.SetActive(false);
        primaryRightHandPose.gameObject.SetActive(false);
        secondaryRightHandPose.gameObject.SetActive(false);
    }

    public void SetupPoseOnInteraction(BaseInteractionEventArgs arg)
    {
        SetupPose(arg.interactableObject, arg.interactorObject);
    }

    public void UnsetPoseOnInteraction(BaseInteractionEventArgs arg)
    {
        UnsetPose(arg.interactableObject, arg.interactorObject);
    }

    public void SetupPose(IXRInteractable gun, IXRInteractor hand)
    {
        if (gun is XRGrabInteractable)
        {
            HandBoneData handBoneData = hand.transform.GetComponentInChildren<HandBoneData>();
            handBoneData.animator.enabled = false;

            if (handBoneData.handSide == HandBoneData.HandModelSide.Left)
            {
                if (handBoneData.poseType == HandBoneData.HandModelPose.Primary) 
                    SetHandBoneDataValues(handBoneData, primaryLeftHandPose, out leftStartAndEndHandData);
                else SetHandBoneDataValues(handBoneData, secondaryLeftHandPose, out leftStartAndEndHandData);
                StartCoroutine(SetHandBoneDataRoutine(handBoneData, leftStartAndEndHandData.startingPosition, leftStartAndEndHandData.startingRotation, leftStartAndEndHandData.startingFingersRotation, leftStartAndEndHandData.endingPosition, leftStartAndEndHandData.endingRotation, leftStartAndEndHandData.endingFingersRotation));
            }
            else
            {
                if (handBoneData.poseType == HandBoneData.HandModelPose.Primary)
                    SetHandBoneDataValues(handBoneData, primaryRightHandPose, out rightStartAndEndHandData);
                else SetHandBoneDataValues(handBoneData, secondaryRightHandPose, out rightStartAndEndHandData);
                StartCoroutine(SetHandBoneDataRoutine(handBoneData, rightStartAndEndHandData.startingPosition, rightStartAndEndHandData.startingRotation, rightStartAndEndHandData.startingFingersRotation, rightStartAndEndHandData.endingPosition, rightStartAndEndHandData.endingRotation, rightStartAndEndHandData.endingFingersRotation));
            }
        }
    }

    public void UnsetPose(IXRInteractable gun, IXRInteractor hand)
    {
        if (gun is XRGrabInteractable)
        {
            HandBoneData handBoneData = hand.transform.GetComponentInChildren<HandBoneData>();
            handBoneData.animator.enabled = true;

            if (handBoneData.handSide == HandBoneData.HandModelSide.Left)
            {
                StartCoroutine(SetHandBoneDataRoutine(handBoneData, leftStartAndEndHandData.endingPosition, leftStartAndEndHandData.endingRotation, leftStartAndEndHandData.endingFingersRotation, leftStartAndEndHandData.startingPosition, leftStartAndEndHandData.startingRotation, leftStartAndEndHandData.startingFingersRotation));
            }
            else
            {
                StartCoroutine(SetHandBoneDataRoutine(handBoneData, rightStartAndEndHandData.endingPosition, rightStartAndEndHandData.endingRotation, rightStartAndEndHandData.endingFingersRotation, rightStartAndEndHandData.startingPosition, rightStartAndEndHandData.startingRotation, rightStartAndEndHandData.startingFingersRotation));
            }
        }
    }

    public void SetHandBoneDataValues(HandBoneData h1, HandBoneData h2, out StartAndEndHandData currentHand)
    {
        currentHand.startingPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x,
            h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);
        currentHand.endingPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x,
            h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);

        currentHand.startingRotation = h1.root.localRotation;
        currentHand.endingRotation = h2.root.localRotation;

        currentHand.startingFingersRotation = new Quaternion[h1.fingerBones.Length];
        currentHand.endingFingersRotation = new Quaternion[h2.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++)
        {
            currentHand.startingFingersRotation[i] = h1.fingerBones[i].localRotation;
            currentHand.endingFingersRotation[i] = h2.fingerBones[i].localRotation;
        }
    }

    // Snap version
    public void SetHandBoneData(HandBoneData hand, Vector3 newPos, Quaternion newRot, Quaternion[] newFingersRot)
    {
        hand.root.localPosition = newPos;
        hand.root.localRotation = newRot;

        for (int i = 0; i < newFingersRot.Length; i++)
        {
            hand.fingerBones[i].localRotation = newFingersRot[i];
        }
    }


    // Smooth interpolation version
    public IEnumerator SetHandBoneDataRoutine(HandBoneData hand, Vector3 startingPos, Quaternion startingRot, Quaternion[] startingFingersRot, Vector3 newPos, Quaternion newRot, Quaternion[] newFingersRot)
    {
        float totalTimeElapsed = 0;

        while (totalTimeElapsed < poseInterpDuration)
        {
            Vector3 interpPos = Vector3.Lerp(startingPos, newPos, totalTimeElapsed / poseInterpDuration);
            Quaternion interpRot = Quaternion.Lerp(startingRot, newRot, totalTimeElapsed / poseInterpDuration);

            hand.root.localPosition = interpPos;
            hand.root.localRotation = interpRot;

            for (int i = 0; i < newFingersRot.Length; i++)
            {
                hand.fingerBones[i].localRotation = Quaternion.Lerp(startingFingersRot[i], newFingersRot[i], totalTimeElapsed / poseInterpDuration);
            }

            totalTimeElapsed += Time.deltaTime;
            yield return null;
        }
    }

#if UNITY_EDITOR

    [MenuItem("Tools/Mirror the selected left grab pose")]
    public static void MirrorLeftPose()
    {
        Debug.Log("mirrored left to right");
        GrabPistolHandPose handPose = Selection.activeGameObject.GetComponent<GrabPistolHandPose>();
        handPose.MirrorPose(handPose.primaryLeftHandPose, handPose.primaryRightHandPose);
        handPose.MirrorPose(handPose.secondaryLeftHandPose, handPose.secondaryRightHandPose);
    }

    public void MirrorPose(HandBoneData toMirror, HandBoneData mirroredOut)
    {
        Vector3 mirroredPos = toMirror.root.localPosition;
        mirroredPos.x *= -1;

        Quaternion mirroredRot = toMirror.root.localRotation;
        mirroredRot.y *= -1;
        mirroredRot.z *= -1;

        mirroredOut.root.localPosition = mirroredPos;
        mirroredOut.root.localRotation = mirroredRot;

        for (int i = 0; i < toMirror.fingerBones.Length; i++)
        {
            mirroredOut.fingerBones[i].localRotation = toMirror.fingerBones[i].localRotation;
        }
    }
#endif
}
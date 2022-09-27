using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabPistolHandPose : MonoBehaviour
{
    public HandBoneData handPose;

    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;
    private Quaternion[] startingFingersRotation;
    private Quaternion[] finalFingersRotation;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnsetPose);

        handPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactableObject is XRGrabInteractable)
        {
            HandBoneData handBoneData = arg.interactorObject.transform.GetComponentInChildren<HandBoneData>();
            handBoneData.animator.enabled = false;

            SetHandBoneDataValues(handBoneData, handPose);
            SetHandBoneData(handBoneData, finalHandPosition, finalHandRotation, finalFingersRotation);
        }
    }

    public void UnsetPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactableObject is XRGrabInteractable)
        {
            HandBoneData handBoneData = arg.interactorObject.transform.GetComponentInChildren<HandBoneData>();
            handBoneData.animator.enabled = true;

            SetHandBoneData(handBoneData, startingHandPosition, startingHandRotation, startingFingersRotation);
        }
    }

    public void SetHandBoneDataValues(HandBoneData h1, HandBoneData h2)
    {
        startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x, 
            h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);
        finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x,
            h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);

        startingHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startingFingersRotation = new Quaternion[h1.fingerBones.Length];
        finalFingersRotation = new Quaternion[h2.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++)
        {
            startingFingersRotation[i] = h1.fingerBones[i].localRotation;
            finalFingersRotation[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SetHandBoneData(HandBoneData hand, Vector3 newPos, Quaternion newRot, Quaternion[] newFingersRot)
    {
        hand.root.localPosition = newPos;
        hand.root.localRotation = newRot;

        for (int i = 0; i < newFingersRot.Length; i++)
        {
            hand.fingerBones[i].localRotation = newFingersRot[i];
        }
    }
}

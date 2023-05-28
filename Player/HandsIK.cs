using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsIK : MonoBehaviour
{
    public bool rightHand;
    [SerializeField]
    private Transform rightHandPos;
    [SerializeField]
    private float rightHandWeight;

    public bool leftHand;
    [SerializeField]
    private Transform leftHandPos;
    [SerializeField]
    private float leftHandWeight;

    [SerializeField]
    private Animator anim;

    private void OnAnimatorIK(int layerIndex)
    {
        if (rightHand)
        {
            anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos.position);
            anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandPos.rotation);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
        }

        if (leftHand)
        {
            anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos.position);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandPos.rotation);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
        }
    }
}

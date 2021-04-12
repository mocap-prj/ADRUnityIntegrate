using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIKHelper : MonoBehaviour
{
    /*
    * 基本骨骼定义
    */
    public enum SikBones
    {
        Hips,
        RightUpLeg,
        RightLeg,
        RightFoot,
        LeftUpLeg,
        LeftLeg,
        LeftFoot,
        Spine,
        Spine1,
        Spine2,
        Neck,
        Neck1,
        Head,
        RightShoulder,
        RightArm,
        RightForeArm,
        RightHand,
        RightHandThumb1,
        RightHandThumb2,
        RightHandThumb3,
        RightInHandIndex,
        RightHandIndex1,
        RightHandIndex2,
        RightHandIndex3,
        RightInHandMiddle,
        RightHandMiddle1,
        RightHandMiddle2,
        RightHandMiddle3,
        RightInHandRing,
        RightHandRing1,
        RightHandRing2,
        RightHandRing3,
        RightInHandPinky,
        RightHandPinky1,
        RightHandPinky2,
        RightHandPinky3,
        LeftShoulder,
        LeftArm,
        LeftForeArm,
        LeftHand,
        LeftHandThumb1,
        LeftHandThumb2,
        LeftHandThumb3,
        LeftInHandIndex,
        LeftHandIndex1,
        LeftHandIndex2,
        LeftHandIndex3,
        LeftInHandMiddle,
        LeftHandMiddle1,
        LeftHandMiddle2,
        LeftHandMiddle3,
        LeftInHandRing,
        LeftHandRing1,
        LeftHandRing2,
        LeftHandRing3,
        LeftInHandPinky,
        LeftHandPinky1,
        LeftHandPinky2,
        LeftHandPinky3,
        MaxBone,
    };

    public static string MapBoneName(string name)
    {
        switch (name)
        {
            case "LeftFoot_End":
            case "RightFoot_End":
            case "LeftArm_Twist":
            case "LeftForeArm_Twist":
            case "LeftHandThumb_End":
            case "LeftHandIndex_End":
            case "LeftHandMiddle_End":
            case "LeftHandRing_End":
            case "LeftHandPinky_End":
            case "Head_End":
            case "RightArm_Twist":
            case "RightForeArm_Twist":
            case "RightHandThumb_End":
            case "RightHandIndex_End":
            case "RightHandMiddle_End":
            case "RightHandRing_End":
            case "RightHandPinky_End":
                return "";
            case "LeftHandIndex":
                name = "LeftInHandIndex";
                break;
            case "LeftHandMiddle":
                name = "LeftInHandMiddle";
                break;
            case "LeftHandRing":
                name = "LeftInHandRing";
                break;
            case "LeftHandPinky":
                name = "LeftInHandPinky";
                break;
            case "RightHandIndex":
                name = "RightInHandIndex";
                break;
            case "RightHandMiddle":
                name = "RightInHandMiddle";
                break;
            case "RightHandRing":
                name = "RightInHandRing";
                break;
            case "RightHandPinky":
                name = "RightInHandPinky";
                break;
        }

        return name;
    }
}

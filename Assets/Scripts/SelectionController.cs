using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SelectionController : MonoBehaviour
{


    public  bool getStatusTriggerRight()
    {
        return (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand));
    }

    public bool getStatusTriggerLeft()
    {
        return (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand));

    }



}
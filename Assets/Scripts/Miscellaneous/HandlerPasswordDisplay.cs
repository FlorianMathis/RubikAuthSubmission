using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class HandlerPasswordDisplay : MonoBehaviour
{

    public SteamVR_Action_Boolean m_GripAction;
    private SteamVR_Behaviour_Pose m_Pose = null;

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Start()
    {
        GlobalHandler.representation = 0;

    }
    // Update is called once per frame
    void Update()
    {
        //if (m_GripAction.GetStateDown(m_Pose.inputSource))
        //{
        //   GameObject.Find("Planes").transform.Rotate(0, 180, 0);
        // }
        switch (Input.inputString)
        {
            // no switches
            case "1":
                GameObject.Find("Password").GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword0S1");
                GlobalHandler.password = 1;
                break;
            case "2":
                GameObject.Find("Password").GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword0S2");
                GlobalHandler.password = 2;
                break;
            case "3":
                GameObject.Find("Password").GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword0S3");
                GlobalHandler.password = 3;
                break;
            // 1 switch
            case "4":
                GameObject.Find("Password").GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword1S1");
                GlobalHandler.password = 4;
                break;
            case "5":
                GameObject.Find("Password").GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword1S2");
                GlobalHandler.password = 5;
                break;
            case "6":
                GameObject.Find("Password").GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword1S3");
                GlobalHandler.password = 6;
                break;
           // 2 switches
            case "7":
                GameObject.Find("Password").GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword2S1");
                GlobalHandler.password = 7;
                break;
            case "8":
                GameObject.Find("Password").GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword2S2");
                GlobalHandler.password = 8;
                break;
            case "9":
                GameObject.Find("Password").GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword2S3");
                GlobalHandler.password = 9;
                break;
            // 3 switches
            case "p":
                GameObject.Find("Password").GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword3S1");
                GlobalHandler.password = 10;
                break;
            case "[":
                GameObject.Find("Password").GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword3S2");
                GlobalHandler.password = 11;
                break;
            case "]":
                GameObject.Find("Password").GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword3S3");
                GlobalHandler.password = 12;
                break;
                      
                    }
    }
}

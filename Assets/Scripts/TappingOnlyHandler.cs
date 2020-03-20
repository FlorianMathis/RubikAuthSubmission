using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class TappingOnlyHandler : MonoBehaviour
{
    public FileWriter fw;
    public MetaDataWriter mdfw;
    private float inputtime;
    private bool started;
    private bool stopped;
    private int count;
    public SteamVR_Action_Boolean m_GrabAction;
    public SteamVR_Action_Boolean m_GripAction;

    // The Unity EyeTracker helper object.

    private SteamVR_Behaviour_Pose m_Pose = null;
    public static PasswordModel passwordmodel;

    public Material _pincorrect;
    public Material _pinwrong;

    // The Unity EyeTracker helper object.
    private HandTappingOnly hand;
    // Start is called before the first frame update
    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }
    // Start is called before the first frame update
    void Start()
    {
        started = false;
        stopped = false;
        passwordmodel = new PasswordModel(new List<String>() { "POS1", "POS2", "POS3", "POS4" }, 4);
        fw = GameObject.Find("Storage").GetComponent<FileWriter>();
        mdfw = GameObject.Find("Storage").GetComponent<MetaDataWriter>();
        hand = GameObject.FindWithTag("Right").GetComponent<HandTappingOnly>();


    }
    // Update is called once per frame
    void Update()
    {
        // track time
        inputtime += Time.deltaTime;
        if (Input.GetKeyDown("space") || m_GripAction.GetStateDown(m_Pose.inputSource))
        {
            SceneManager.LoadScene("3DAuthenticationSchemeTappingOnly");
            started = false;
            stopped = false;
        }

        if (m_GrabAction.GetStateDown(m_Pose.inputSource) && !started)
        {
            fw.StartRecording();
            //Debug.Log("recording started");
            started = true;
            mdfw.StartRecording();
            inputtime = 0;
            mdfw.WriteToFileCsv(new DataModel(inputtime, passwordmodel.getUserInput()));


        }
        else if(m_GrabAction.GetStateDown(m_Pose.inputSource) && started && !stopped)
        {
            Debug.Log("writing");
            mdfw.WriteToFileCsv(new DataModel(inputtime, passwordmodel.getUserInput()));
        }

        if (hand.Userinput.Count == 4 && !stopped)
        {
            mdfw.WriteToFileCsv(new DataModel(inputtime, passwordmodel.getUserInput()));

            stopped = true;
            List<String> userinputStrings = new List<String>();
            foreach (var userinput in hand.Userinput)
            {
                Debug.Log(userinput);
                userinputStrings.Add(userinput.name);
                passwordmodel.backupInput(userinput.name);
            }
            //passwordmodel.checkPassword(userinputStrings);
            passwordmodel.entryError(userinputStrings);
            Debug.Log("recording stopped");
            fw.StopRecording();
            mdfw.StopRecording();

            if (passwordmodel.getErrors() == 0)
            {
                foreach (GameObject pin in GameObject.FindGameObjectsWithTag("Interactable"))
                {
                    pin.GetComponent<MeshRenderer>().material = _pincorrect;

                }
            }
            else
            {
                foreach (GameObject pin in GameObject.FindGameObjectsWithTag("Interactable"))
                {
                    pin.GetComponent<MeshRenderer>().material = _pinwrong;

                }
            }
        }

    }

}

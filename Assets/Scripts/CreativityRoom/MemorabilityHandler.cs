
//-----------------------------------------------------------------------
// Copyright © 2017 Tobii AB. All rights reserved.
//-----------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System; //This allows the IComparable Interface
using System.Collections.Generic;
using Tobii.Research.Unity.Examples;
using UnityEngine.SceneManagement;
using Valve.VR;

public class MemorabilityHandler : MonoBehaviour
{

    // The material to use for active objects.

    public Material _pinhighlight;
    public Material _wrong;

    // The object that we hit.
    private ActiveObject _highlightInfo;

    public GameObject RaySource;

    // Whatever we need to run the calibration.
    private bool _calibratedSuccessfully;

    // Remember if we have saved data.
    private bool _hasSavedData;

    // Gaze trail script.
    private Tobii.Research.Unity.VRGazeTrail _gazeTrail;

    // The Unity EyeTracker helper object.
    private Tobii.Research.Unity.VREyeTracker _eyeTracker;
    //public List<Int32> PIN2;
    // scenes
    private Tobii.Research.Unity.IVRGazeData gazeData;
    public static PasswordModel passwordmodel;
    public CreativityWriter fw;
    public MetaDataWriter mdfw;
    public List<GameObject> Userinput = new List<GameObject>();

    public List<String> memorabilitypasswords_weak = new List<String>();
    public List<String> memorabilitypasswords_medium = new List<String>();
    public List<String> memorabilitypasswords_strong = new List<String>();

    // full tracking
    public FullCreativityWriter fullfw;
    public FullMetaDataWriter fullmdfw;
    public Material _pincorrect;
    public Material _pinwrong;
    private GameObject currentGameobject;
    // The Unity EyeTracker helper object.
    public SteamVR_Action_Boolean m_GrabAction;
    public SteamVR_Action_Boolean m_GripAction;
    // The Unity EyeTracker helper object.
    private List<GameObject> current = new List<GameObject>();
    private FixedJoint m_Joint = null;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private bool started;
    private bool stopped;
    private float inputtime;
    private float fullinputtime;
    private List<String> selectedPINS;
    public List<MaterialHolder> materialholderList;
    private List<FeedbackModel> feedbackElements;
    private float feedbacktime;
    public int trials;
    public string security;

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }

    void Start()
    {
        loadPasswords();
        trials = 1;
        security = "weak";

        Init();

    }

    // called by anyone
    public void Init()
    {
        if(GlobalHandler.modality == 0 || GlobalHandler.modality == 2)
        {
            if(GameObject.Find("HeadPosePoint") != null)
            GameObject.Find("HeadPosePoint").SetActive(false);
        }
        if (GlobalHandler.modality == 1 || GlobalHandler.modality == 2)
        {
            if (GameObject.Find("[VRGazeTrail]") != null)
                GameObject.Find("[VRGazeTrail]").SetActive(false);

        }
        foreach (GameObject pin in GameObject.FindGameObjectsWithTag("PIN"))
        {
            materialholderList.Add(new MaterialHolder(pin.GetComponent<MeshRenderer>().material, pin.gameObject));
        }
        currentGameobject = null;
        // Get EyeTracker unity object
        _eyeTracker = Tobii.Research.Unity.VREyeTracker.Instance;
        if (_eyeTracker == null)
        {
            Debug.Log("Failed to find eye tracker, has it been added to scene?");
        }

        _gazeTrail = Tobii.Research.Unity.VRGazeTrail.Instance;

        _highlightInfo = new ActiveObject();

        selectedPINS = new List<String>();

        inputtime = 0;
        fullinputtime = 0;
        started = false;
        stopped = false;
        // not relevant but we need it because of the current implementation
        // participant passwords
        if(security == "weak")
        {
            switch (GlobalHandler.participant_number)
            {
                case 1:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1W", "POSX2W", "POSX3W", "POSX4W" }, 4);
                    break;
                case 2:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                    break;
                case 3:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                    break;
                case 4:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX1G", "POSX3G", "POSX4G" }, 4);
                    break;
                case 5:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                    break;
                case 6:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                    break;
                case 7:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX4G", "POSX7G", "POSX8G" }, 4);
                    break;
                case 8:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX4G", "POSX7G", "POSX8G" }, 4);
                    break;
                case 9:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX3G", "POSX1G", "POSX4G", "POSX1G" }, 4);
                    break;
                case 10:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1W", "POSX2W", "POSX3W", "POSX4W" }, 4);
                    break;
                case 11:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                    break;
                case 12:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                    break;
                case 13:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1W", "POSX2W", "POSX3W", "POSX4W" }, 4);
                    break;
                case 14:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX4G", "POSX6G", "POSX8G" }, 4);
                    break;
                case 15:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                    break;
                case 16:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX4G", "POSX6G", "POSX8G" }, 4);
                    break;
                case 17:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX5G", "POSX5G", "POSX5G" }, 4);
                    break;
                case 18:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                    break;
                case 19:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                    break;
                case 20:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX8W", "POSX2O", "POSX4R" }, 4);
                    break;
                case 21:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1W", "POSX5W", "POSX9W", "POSX6W" }, 4);
                    break;
                case 22:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                    break;
                case 23:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                    break;
            }

        }
        else if (security == "medium")
        {
            switch (GlobalHandler.participant_number)
            {
                case 1:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2W", "POSX4W", "POSX6W", "POSX8W" }, 4);
                    break;
                case 2:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX4G", "POSX1W", "POSX1W" }, 4);
                    break;
                case 3:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3O", "POSX4O" }, 4);
                    break;
                case 4:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX5G", "POSX9G", "POSX1O" }, 4);
                    break;
                case 5:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2W", "POSX3O", "POSX4R" }, 4);
                    break;
                case 6:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3W", "POSX4W" }, 4);
                    break;
                case 7:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX3G", "POSX1W", "POSX9W" }, 4);
                    break;
                case 8:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX8G", "POSX2O", "POSX4O" }, 4);
                    break;
                case 9:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX2G", "POSX4B", "POSX4O" }, 4);
                    break;
                case 10:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2W", "POSX3W", "POSX4O" }, 4);
                    break;
                case 11:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1R", "POSX2W", "POSX3G", "POSX4R" }, 4);
                    break;
                case 12:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX5G", "POSX8G", "POSX3G" }, 4);
                    break;
                case 13:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                    break;
                case 14:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX6G", "POSX1G", "POSX7G", "POSX9G" }, 4);
                    break;
                case 15:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX6G", "POSX5G", "POSX7G", "POSX1G" }, 4);
                    break;
                case 16:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX6G", "POSX7G", "POSX3G" }, 4);
                    break;
                case 17:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX4G", "POSX8G", "POSX6G" }, 4);
                    break;
                case 18:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX9G", "POSX6G", "POSX3G" }, 4);
                    break;
                case 19:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX3G", "POSX5G", "POSX7G" }, 4);
                    break;
                case 20:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX3G", "POSX2W", "POSX3O", "POSX9R" }, 4);
                    break;
                case 21:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1W", "POSX4W", "POSX7W", "POSX5W" }, 4);
                    break;
                case 22:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX9G", "POSX1G", "POSX9G" }, 4);
                    break;
                case 23:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX5G", "POSX9G", "POSX6G" }, 4);
                    break;
            }
        }
        else if (security == "strong")
        {
            switch (GlobalHandler.participant_number)
            {
                case 1:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2W", "POSX2W", "POSX8W", "POSX4W" }, 4);
                    break;
                case 2:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX4G", "POSX4O", "POSX2W", "POSX2W" }, 4);
                    break;
                case 3:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX5W", "POSX8B", "POSX9B" }, 4);
                    break;
                case 4:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX9R", "POSX6Y", "POSX1B", "POSX3G" }, 4);
                    break;
                case 5:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX7G", "POSX5O", "POSX2W", "POSX3G" }, 4);
                    break;
                case 6:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2W", "POSX3O", "POSX4B" }, 4);
                    break;
                case 7:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX5W", "POSX5O", "POSX3B" }, 4);
                    break;
                case 8:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX8G", "POSX2W", "POSX4O" }, 4);
                    break;
                case 9:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX6G", "POSX8G", "POSX6B", "POSX1W" }, 4);
                    break;
                case 10:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX3W", "POSX9W", "POSX9G", "POSX1G" }, 4);
                    break;
                case 11:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX7G", "POSX6B", "POSX8W", "POSX5Y" }, 4);
                    break;
                case 12:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX9G", "POSX8G", "POSX1G", "POSX5G" }, 4);
                    break;
                case 13:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX4B", "POSX3G", "POSX9R", "POSX4G" }, 4);
                    break;
                case 14:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX4G", "POSX1O", "POSX3W", "POSX2R" }, 4);
                    break;
                case 15:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX8O", "POSX2O", "POSX3W", "POSX8W" }, 4);
                    break;
                case 16:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX4G", "POSX1G", "POSX9G" }, 4);
                    break;
                case 17:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX1O", "POSX9W", "POSX9W" }, 4);
                    break;
                case 18:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX9G", "POSX6W", "POSX6O" }, 4);
                    break;
                case 19:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX5O", "POSX9B", "POSX5R" }, 4);
                    break;
                case 20:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX6W", "POSX4W", "POSX9O", "POSX7O" }, 4);
                    break;
                case 21:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2W", "POSX4W", "POSX6W", "POSX8W" }, 4);
                    break;
                case 22:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX6G", "POSX2O", "POSX7W", "POSX5R" }, 4);
                    break;
                case 23:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX3R", "POSX2W", "POSX3O" }, 4);
                    break;
            }
        }
        //
        //fw = GameObject.Find("Storage").GetComponent<CreativityWriter>();
        mdfw = GameObject.Find("Storage").GetComponent<MetaDataWriter>();
        //fullfw = GameObject.Find("Storage").GetComponent<FullCreativityWriter>();
        fullmdfw = GameObject.Find("Storage").GetComponent<FullMetaDataWriter>();
        //fullfw.StartRecording();
        fullmdfw.StartRecording();
        //Debug.Log(GameObject.Find("Planes").transform.rotation);
        feedbackElements = new List<FeedbackModel>();
        feedbacktime = 0.4f;
    }

    private void Update()
    {
        if (m_GripAction.GetStateDown(m_Pose.inputSource))
        {
            trials++;

            foreach (MaterialHolder materialobj in materialholderList)
            {
                materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;

            }
            Init();
           
        }

        if (Input.inputString == "e" ||
            Input.inputString == "m" ||
            Input.inputString == "s" )
        {
            trials = 1;
            foreach (GameObject pin in GameObject.FindGameObjectsWithTag("PIN"))
            {
                pin.GetComponent<MeshRenderer>().enabled = true;

            }

            Canvas weakpw = GameObject.Find("weakpw").GetComponent<Canvas>();
            Canvas mediumpw = GameObject.Find("mediumpw").GetComponent<Canvas>();
            Canvas strongpw = GameObject.Find("strongpw").GetComponent<Canvas>();


            // toggle display instruction
            //fullfw.StopRecording();
            fullmdfw.StopRecording();
            foreach (MaterialHolder materialobj in materialholderList)
            { 
                materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
            }

            if (Input.inputString == "e")
            {
                weakpw.enabled = true;
                mediumpw.enabled = false;
                strongpw.enabled = false;
                security = "weak";


            }
            else if (Input.inputString == "m")
            {
                weakpw.enabled = false;
                mediumpw.enabled = true;
                strongpw.enabled = false;
                security = "medium";


            }
            else if (Input.inputString == "s")
            {
                weakpw.enabled = false;
                mediumpw.enabled = false;
                strongpw.enabled = true;
                security = "strong";


            }
            Init();

        }

        // track time
        inputtime += Time.deltaTime;
        fullinputtime += Time.deltaTime;
        Vector3 origin = GameObject.Find("Camera").transform.position;
       
        //Debug.Log(origin);
        //Debug.Log("direction "+direction);
        var latestHitObject = _gazeTrail.LatestHitObject;
        if (latestHitObject != null && latestHitObject.name.StartsWith("POS") && m_GrabAction.GetStateDown(m_Pose.inputSource) && !stopped && GlobalHandler.modality == 0)
        {
            if (!started)
            { // when to start with recording by using dwell time?

                //fw.StartRecording();
                mdfw.StartRecording();
                started = true;
                inputtime = 0;
            }
            mdfw.WriteToFileCsv(new DataModel(inputtime, latestHitObject.name));
            fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, latestHitObject.name));

            passwordmodel.backupInput(latestHitObject.name);

            selectedPINS.Add(latestHitObject.name);
            if (selectedPINS.Count == 4)
            {
                passwordmodel.entryError(selectedPINS);
                if (passwordmodel.getErrors() == 0)
                {
                    mdfw.WriteToFileCsv(new DataModel(inputtime, security + " correct"));
                    fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, security + " correct"));
                }
                else
                {
                    mdfw.WriteToFileCsv(new DataModel(inputtime, security + " incorrect"));
                    fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, security + " incorrect"));
                }
                mdfw.WriteToFileCsv(new DataModel(inputtime, "trial: " + trials));
                fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "trial: " + trials));

                mdfw.StopRecording();
                fullmdfw.StopRecording();
               // fw.StopRecording();
               //fullfw.StopRecording();

            }
            feedbackElements.Add(new FeedbackModel(latestHitObject.gameObject, latestHitObject.gameObject.GetComponent<MeshRenderer>().material, feedbacktime));
            MeshRenderer renderer = latestHitObject.gameObject.GetComponent<MeshRenderer>();
            renderer.material = _pinhighlight;


        }
        else if (m_GrabAction.GetStateDown(m_Pose.inputSource) && !started && !stopped && current != null && GetNearestGameObject() != null && GlobalHandler.modality == 2)
        {

            //fw.StartRecording();
            //Debug.Log("recording started");
            started = true;
            mdfw.StartRecording();
            inputtime = 0;
            passwordmodel.backupInput(GetNearestGameObject().name);
            selectedPINS.Add(GetNearestGameObject().name);
            mdfw.WriteToFileCsv(new DataModel(inputtime, GetNearestGameObject().name));
            fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, GetNearestGameObject().name));
            if (selectedPINS.Count == 4)
            {
                passwordmodel.entryError(selectedPINS);
                if (passwordmodel.getErrors() == 0)
                {
                    mdfw.WriteToFileCsv(new DataModel(inputtime, security + " correct"));
                    fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, security + " correct"));
                }
                else
                {
                    mdfw.WriteToFileCsv(new DataModel(inputtime, security + " incorrect"));
                    fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, security + " incorrect"));
                }
                mdfw.WriteToFileCsv(new DataModel(inputtime, "trial: " + trials));
                fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "trial: " + trials));

                mdfw.StopRecording();
                fullmdfw.StopRecording();
               // fw.StopRecording();
                //fullfw.StopRecording();

            }
            feedbackElements.Add(new FeedbackModel(GetNearestGameObject(), GetNearestGameObject().GetComponent<MeshRenderer>().material, feedbacktime));
            MeshRenderer renderer = GetNearestGameObject().GetComponent<MeshRenderer>();
            renderer.material = _pinhighlight;
            Userinput.Add(GetNearestGameObject());


            //call method Input();
            //Pickup();
        }


        //Up
        else if (m_GrabAction.GetStateDown(m_Pose.inputSource) && started && !stopped && current != null && GetNearestGameObject() != null && GlobalHandler.modality == 2)
        {

            passwordmodel.backupInput(GetNearestGameObject().name);
            feedbackElements.Add(new FeedbackModel(GetNearestGameObject(), GetNearestGameObject().GetComponent<MeshRenderer>().material, feedbacktime));
            MeshRenderer renderer = GetNearestGameObject().GetComponent<MeshRenderer>();
            renderer.material = _pinhighlight;
            Userinput.Add(GetNearestGameObject());
            selectedPINS.Add(GetNearestGameObject().name);
            mdfw.WriteToFileCsv(new DataModel(inputtime, GetNearestGameObject().name));
            fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, GetNearestGameObject().name));
            if (selectedPINS.Count == 4)
            {
                passwordmodel.entryError(selectedPINS);
                if (passwordmodel.getErrors() == 0)
                {
                    mdfw.WriteToFileCsv(new DataModel(inputtime, security + " correct"));
                    fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, security + " correct"));
                }
                else
                {
                    mdfw.WriteToFileCsv(new DataModel(inputtime, security + " incorrect"));
                    fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, security + " incorrect"));
                }
                mdfw.WriteToFileCsv(new DataModel(inputtime, "trial: " + trials));
                fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "trial: " + trials));
                mdfw.StopRecording();
                fullmdfw.StopRecording();
                //fw.StopRecording();
               // fullfw.StopRecording();
            }

        }
        else if (GameObject.Find("HeadPosePoint") != null)
        {
            Vector3 direction = GameObject.Find("HeadPosePoint").transform.forward;
            Debug.DrawRay(origin, direction * 10f, Color.red);
            Ray ray = new Ray(origin, direction);

            if (Physics.Raycast(ray, out RaycastHit raycastHit) && GlobalHandler.modality == 1)
            {

                if (m_GrabAction.GetStateDown(m_Pose.inputSource) && raycastHit.transform.tag == "PIN" && !stopped)
                {
                    Debug.Log("ok this is fine");

                    if (!started)
                    {

                       // fw.StartRecording();
                        mdfw.StartRecording();
                        started = true;
                        inputtime = 0;
                    }

                    passwordmodel.backupInput(raycastHit.transform.name);
                    selectedPINS.Add(raycastHit.transform.name);
                    mdfw.WriteToFileCsv(new DataModel(inputtime, raycastHit.transform.name));
                    fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, raycastHit.transform.name));
                    if (selectedPINS.Count == 4)
                    {
                        passwordmodel.entryError(selectedPINS);
                        if (passwordmodel.getErrors() == 0)
                        {
                            mdfw.WriteToFileCsv(new DataModel(inputtime, security + " correct"));
                            fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, security + " correct"));
                        }
                        else
                        {
                            mdfw.WriteToFileCsv(new DataModel(inputtime, security + " incorrect"));
                            fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, security + " incorrect"));
                        }
                        mdfw.WriteToFileCsv(new DataModel(inputtime, "trial: " + trials));
                        fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "trial: " + trials));

                        mdfw.StopRecording();
                        fullmdfw.StopRecording();
                       // fw.StopRecording();
                       // fullfw.StopRecording();
                    }
                    feedbackElements.Add(new FeedbackModel(raycastHit.collider.gameObject, raycastHit.collider.GetComponent<MeshRenderer>().material, feedbacktime));
                    MeshRenderer renderer = raycastHit.collider.gameObject.GetComponent<MeshRenderer>();
                    renderer.material = _pinhighlight;
                    Userinput.Add(raycastHit.transform.gameObject);

                }
                else if (m_GrabAction.GetStateDown(m_Pose.inputSource) && raycastHit.transform.tag == "PIN" && started && !stopped)
                {
                    //Debug.Log("HALLOOOOOOOOOOOOO");
                    selectedPINS.Add(raycastHit.transform.name);
              
                    passwordmodel.backupInput(raycastHit.transform.name);
                    feedbackElements.Add(new FeedbackModel(raycastHit.collider.gameObject, raycastHit.collider.GetComponent<MeshRenderer>().material, feedbacktime));
                    MeshRenderer renderer = raycastHit.collider.gameObject.GetComponent<MeshRenderer>();
                    renderer.material = _pinhighlight;
                    Userinput.Add(raycastHit.transform.gameObject);
                    mdfw.WriteToFileCsv(new DataModel(inputtime, raycastHit.transform.name));
                    fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, raycastHit.transform.name));
                    if (selectedPINS.Count == 4)
                    {
                        if(passwordmodel.getErrors() == 0)
                        {
                            mdfw.WriteToFileCsv(new DataModel(inputtime, security + " correct"));
                            fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, security + " correct"));
                        } else
                        {
                            mdfw.WriteToFileCsv(new DataModel(inputtime, security + " incorrect"));
                            fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, security + " incorrect"));
                        }
                        passwordmodel.entryError(selectedPINS);

                        mdfw.WriteToFileCsv(new DataModel(inputtime, "trial: " + trials));
                        fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "trial: " + trials));

                        mdfw.StopRecording();
                        fullmdfw.StopRecording();
                       // fw.StopRecording();
                       // fullfw.StopRecording();
                    }

                }
            }
        }
        Debug.Log(selectedPINS);
        Debug.Log(passwordmodel.getErrors());
        if (passwordmodel.getErrors() == 0 && selectedPINS.Count == 4)
        {
            foreach (GameObject pin in GameObject.FindGameObjectsWithTag("PIN"))
            {
                pin.GetComponent<MeshRenderer>().material = _pincorrect;

            }
        }
        else if (selectedPINS.Count == 4)
        {
            foreach (GameObject pin in GameObject.FindGameObjectsWithTag("PIN"))
            {
                pin.GetComponent<MeshRenderer>().material = _pinwrong;

            }
        }
        if (feedbackElements != null)
        {
            foreach (FeedbackModel element in feedbackElements.ToArray())
            {
                element.Time -= Time.deltaTime;
                //Debug.Log("element time" + element.Time);
                if (element.Time <= 0 && selectedPINS.Count != 4)
                {
                    element.Gameobject.GetComponent<MeshRenderer>().material = element.Material;
                    feedbackElements.Remove(element);
                }
            }
        }
        

        _gazeTrail.ParticleColor = Color.blue;

    }
        private void OnTriggerEnter(Collider other)
        {

        if (!other.gameObject.CompareTag("PIN"))
                return;
            current.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("PIN"))
                return;
            current.Remove(other.gameObject);
            // m_ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
        }
        private GameObject GetNearestGameObject()
        {
            GameObject nearest = null;
            float minDistance = float.MaxValue;
            float distance = 0.0f;

            foreach (GameObject gameobject in current)
            {

                distance = (gameobject.transform.position - transform.position).sqrMagnitude;

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = gameobject;
                }
            }
            return nearest;
        }

    public void loadPasswords()
    {
        //participants
        //p1
        /*
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1W", "POSX2W", "POSX3W", "POSX4W" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX2W", "POSX4W", "POSX6W", "POSX8W" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX2W", "POSX2W", "POSX8W", "POSX4W" }, 4));
        //p2
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX4G", "POSX4W", "POSX4W" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX4G", "POSX4O", "POSX2W", "POSX2W" }, 4));
        //p3
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3O", "POSX4O" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX5W", "POSX8B", "POSX9B" }, 4));
        //p4
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX1G", "POSX3G", "POSX4G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX5G", "POSX9G", "POSX1O" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX9R", "POSX6Y", "POSX1B", "POSX3G" }, 4));
        //p5
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2W", "POSX3O", "POSX4R" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX7G", "POSX5O", "POSX2W", "POSX3G" }, 4));
        //p6
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3W", "POSX4W" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2W", "POSX3O", "POSX4B" }, 4));
        //p7
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX4G", "POSX7G", "POSX8G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX3G", "POSX1W", "POSX9W" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX5W", "POSX5O", "POSX3B" }, 4));
        //p8
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX4G", "POSX7G", "POSX8G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX8G", "POSX2O", "POSX4O" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX8G", "POSX2W", "POSX4O" }, 4));
        //p9
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX3G", "POSX1G", "POSX4G", "POSX1G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX2G", "POSX4B", "POSX4O" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX6G", "POSX8G", "POSX6B", "POSX1W" }, 4));
        //p10
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1W", "POSX2W", "POSX3W", "POSX4W" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2W", "POSX3W", "POSX4O" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX3W", "POSX9W", "POSX9G", "POSX1G" }, 4));
        //p11
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1R", "POSX2W", "POSX3G", "POSX4R" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX7G", "POSX6B", "POSX8W", "POSX5Y" }, 4));
        //p12
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX5G", "POSX8G", "POSX3G" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX9G", "POSX8G", "POSX1G", "POSX5G" }, 4));
        //p13
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1W", "POSX2W", "POSX3W", "POSX4W" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX4B", "POSX3G", "POSX9R", "POSX4G" }, 4));
        //p14
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX4G", "POSX6G", "POSX8G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX6G", "POSX1G", "POSX7G", "POSX9G" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX4G", "POSX1O", "POSX3W", "POSX2R" }, 4));
        //p15
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX6G", "POSX5G", "POSX7G", "POSX1G" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX8O", "POSX2O", "POSX3W", "POSX8W" }, 4));
        //p16
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX4G", "POSX6G", "POSX8G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX6G", "POSX7G", "POSX3G" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX4G", "POSX1G", "POSX9G" }, 4));
        //p17
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX5G", "POSX5G", "POSX5G", "POSX5G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX4G", "POSX8G", "POSX6G" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX1O", "POSX9W", "POSX9W" }, 4));
        //p18
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX9G", "POSX6G", "POSX3G" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX9G", "POSX6W", "POSX6O" }, 4));
        //p19
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX3G", "POSX5G", "POSX7G" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX5O", "POSX9B", "POSX5R" }, 4));
        //p20
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX8W", "POSX2O", "POSX4R" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX3G", "POSX2W", "POSX3O", "POSX9R" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX6W", "POSX4W", "POSX9O", "POSX7O" }, 4));
        //p21
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1W", "POSX5W", "POSX9W", "POSX6W" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1W", "POSX4W", "POSX7W", "POSX5W" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX2W", "POSX4W", "POSX6W", "POSX8W" }, 4));
        //p22
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX9G", "POSX1G", "POSX9G" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX6G", "POSX2O", "POSX7W", "POSX5R" }, 4));
        //p23
        memorabilitypasswords_weak.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4));
        memorabilitypasswords_medium.Add(new PasswordModel(new List<System.String>() { "POSX1G", "POSX5G", "POSX9G", "POSX6G" }, 4));
        memorabilitypasswords_strong.Add(new PasswordModel(new List<System.String>() { "POSX2G", "POSX3R", "POSX2W", "POSX3O" }, 4));
        */
    }


}



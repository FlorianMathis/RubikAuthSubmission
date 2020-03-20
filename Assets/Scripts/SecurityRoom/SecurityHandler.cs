
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

public class SecurityHandler : MonoBehaviour
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
    public SecurityWriter fw;
    public MetaDataWriter mdfw;
    public List<GameObject> Userinput = new List<GameObject>();

    // full tracking
    public FullSecurityWriter fullfw;
    public FullMetaDataWriter fullmdfw;

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
    public string security;
    public int trials;
    public PasswordGenerator pwGenerator;

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }

    void Start()
    {
        trials = 0;

        Init();
    }

    // called by anyone
    public void Init()
    {
        if (GlobalHandler.modality == 0 || GlobalHandler.modality == 2)
        {
            if (GameObject.Find("HeadPosePoint") != null)
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
        if (GlobalHandler.attackerscenario == 99) // which meas the expert observation attack is running
        {
            foreach (MaterialHolder materialobj in materialholderList)
            {
                materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
            }
            passwordmodel = new PasswordModel(new List<String>() { "POSX1W", "POSX3W", "POSX5G", "POSX6O" }, 4);
        }
        // change this area for each participant to ensure no follow-up effects
        /*
         * 
         *                          START
         */
        // for each participant, create file

        if (GlobalHandler.attackerscenario == 1)
        {
            if (GlobalHandler.modality == 0)
            {
                switch (trials)
                {
                    case 0:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX4G", "POSX2R", "POSX5R", "POSX8R" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX4G", "POSX2R", "POSX5R", "POSX8R" });
                        break;
                    case 1:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX8W", "POSX6R", "POSX4R", "POSX2G" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX8W", "POSX6R", "POSX4R", "POSX2G" });
                        break;
                    case 2:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX1W", "POSX1R", "POSX1B", "POSX1O" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX1W", "POSX1R", "POSX1B", "POSX1O" });
                        break;
                    case 3:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX1G", "POSX3G", "POSX7G", "POSX9G" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX3G", "POSX7G", "POSX9G" });
                        break;

                }
            }
            if (GlobalHandler.modality == 1)
            {
                switch (trials)
                {
                    case 0:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX1G", "POSX4G", "POSX7G", "POSX8G" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX4G", "POSX7G", "POSX8G" });
                        break;
                    case 1:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX1G", "POSX1W", "POSX2O", "POSX2B" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX1W", "POSX2O", "POSX2B" });
                        break;
                    case 2:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX6G", "POSX2O", "POSX3O", "POSX4G" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX6G", "POSX2O", "POSX3O", "POSX4G" });
                        break;
                    case 3:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX5G", "POSX4G", "POSX1W", "POSX2W" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX4G", "POSX1W", "POSX2W" });
                        break;
                }
            }
            if (GlobalHandler.modality == 2)
            {
                switch (trials)
                {
                    case 0:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX5W", "POSX4O", "POSX6W", "POSX9W" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX5W", "POSX4O", "POSX6W", "POSX9W" });
                        break;
                    case 1:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX4O", "POSX6W", "POSX5G", "POSX8R" }, 4);

                        pwGenerator.generateCube(new List<string>() { "POSX4O", "POSX6W", "POSX5G", "POSX8R" });
                        break;
                    case 2:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX3G", "POSX6G", "POSX9G", "POSX3W" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX3G", "POSX6G", "POSX9G", "POSX3W" });
                        break;
                    case 3:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX9G", "POSX7G", "POSX5G", "POSX3G" }, 4);

                        pwGenerator.generateCube(new List<string>() { "POSX9G", "POSX7G", "POSX5G", "POSX3G" });
                        break;
                }
            }
        }

        if (GlobalHandler.attackerscenario == 2)
        {
            if (GlobalHandler.modality == 0)
            {
                switch (trials)
                {
                    case 0:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX4W", "POSX5W", "POSX6W", "POSX7W" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX4W", "POSX5W", "POSX6W", "POSX7W" });
                        break;
                    case 1:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX4W", "POSX5W", "POSX6W", "POSX6R" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX4W", "POSX5W", "POSX6W", "POSX6R" });
                        break;
                    case 2:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX1G", "POSX2G", "POSX3O", "POSX4B" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX2G", "POSX3O", "POSX4B" });
                        break;
                    case 3:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX5G", "POSX6W", "POSX7R", "POSX8G" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX6W", "POSX7R", "POSX8G" });
                        break;

                }
            }
            if (GlobalHandler.modality == 1)
            {
                switch (trials)
                {
                    case 0:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX3G", "POSX6G", "POSX2G", "POSX1G" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX3G", "POSX6G", "POSX2G", "POSX1G" });
                        break;
                    case 1:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX3G", "POSX3O", "POSX2O", "POSX1O" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX3G", "POSX3O", "POSX2O", "POSX1O" });
                        break;
                    case 2:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX4G", "POSX6G", "POSX5O", "POSX5B" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX4G", "POSX6G", "POSX5O", "POSX5B" });
                        break;
                    case 3:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX1O", "POSX5W", "POSX3R", "POSX7B" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX1O", "POSX5W", "POSX3R", "POSX7B" });
                        break;
                }
            }
            if (GlobalHandler.modality == 2)
            {
                switch (trials)
                {
                    case 0:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX2G", "POSX4R", "POSX6O", "POSX8B" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX2G", "POSX4R", "POSX6O", "POSX8B" });
                        break;
                    case 1:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX7W", "POSX9R", "POSX9B", "POSX8B" }, 4);

                        pwGenerator.generateCube(new List<string>() { "POSX7W", "POSX9R", "POSX9B", "POSX8B" });
                        break;
                    case 2:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX5G", "POSX7G", "POSX6G", "POSX8G" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX7G", "POSX6G", "POSX8G" });
                        break;
                    case 3:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX2G", "POSX2W", "POSX5W", "POSX8W" }, 4);

                        pwGenerator.generateCube(new List<string>() { "POSX2G", "POSX2W", "POSX5W", "POSX8W" });
                        break;
                }
            }
        }

        if (GlobalHandler.attackerscenario == 3)
        {
            if (GlobalHandler.modality == 0)
            {
                switch (trials)
                {
                    case 0:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX9G", "POSX5G", "POSX1G", "POSX2G" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX9G", "POSX5G", "POSX1G", "POSX2G" });
                        break;
                    case 1:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX3G", "POSX4W", "POSX3O", "POSX4B" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX3G", "POSX4W", "POSX3O", "POSX4B" });
                        break;
                    case 2:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX1G", "POSX1O", "POSX2O", "POSX3O" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX1O", "POSX2O", "POSX3O" });
                        break;
                    case 3:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX1W", "POSX3W", "POSX5G", "POSX6O" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX1W", "POSX3W", "POSX5G", "POSX6O" });
                        break;

                }
            }
            if (GlobalHandler.modality == 1)
            {
                switch (trials)
                {
                    case 0:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX5R", "POSX3W", "POSX7G", "POSX9B" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX5R", "POSX3W", "POSX7G", "POSX9B" });
                        break;
                    case 1:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX5G", "POSX6G", "POSX8G", "POSX9G" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX6G", "POSX8G", "POSX9G" });
                        break;
                    case 2:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX2W", "POSX8G", "POSX7G", "POSX3R" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX2W", "POSX8G", "POSX7G", "POSX3R" });
                        break;
                    case 3:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX5W", "POSX5B", "POSX6B", "POSX7B" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX5W", "POSX5B", "POSX6B", "POSX7B" });
                        break;
                }
            }
            if (GlobalHandler.modality == 2)
            {
                switch (trials)
                {
                    case 0:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX9W", "POSX3B", "POSX5B", "POSX7B" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX9W", "POSX3B", "POSX5B", "POSX7B" });
                        break;
                    case 1:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX2W", "POSX4W", "POSX6W", "POSX8W" }, 4);

                        pwGenerator.generateCube(new List<string>() { "POSX2W", "POSX4W", "POSX6W", "POSX8W" });
                        break;
                    case 2:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX2G", "POSX2W", "POSX2R", "POSX3R" }, 4);
                        pwGenerator.generateCube(new List<string>() { "POSX2G", "POSX2W", "POSX2R", "POSX3R" });
                        break;
                    case 3:
                        foreach (MaterialHolder materialobj in materialholderList)
                        {
                            materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                        }
                        passwordmodel = new PasswordModel(new List<String>() { "POSX6G", "POSX1W", "POSX9R", "POSX5B" }, 4);

                        pwGenerator.generateCube(new List<string>() { "POSX6G", "POSX1W", "POSX9R", "POSX5B" });
                        break;
                }
            }
        }
        /*
         * 
         *                          END
         */

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
        //passwordmodel = new PasswordModel(new List<String>() { "POSX1G", "POSX1G", "POSX1G", "POSX1G" }, 4);
        //
        //fw = GameObject.Find("Storage").GetComponent<SecurityWriter>();
        mdfw = GameObject.Find("Storage").GetComponent<MetaDataWriter>();
        fullfw = GameObject.Find("Storage").GetComponent<FullSecurityWriter>();
        fullmdfw = GameObject.Find("Storage").GetComponent<FullMetaDataWriter>();
        //fullfw.StartRecording();
        fullmdfw.StartRecording();
        //Debug.Log(GameObject.Find("Planes").transform.rotation);
        feedbackElements = new List<FeedbackModel>();
        feedbacktime = 0.4f;
    }

    private void Update()
    {
        switch (Input.inputString)
        {
            //zero switches
            case "0":
                fullmdfw.StopRecording();
                SceneManager.LoadScene("SecurityRoom");

                GlobalHandler.modality = 0;
                Init();
                break;
            case "1":
                fullmdfw.StopRecording();
                SceneManager.LoadScene("SecurityRoom");

                GlobalHandler.modality = 1;
                Init();
                break;
            case "2":
                fullmdfw.StopRecording();
                SceneManager.LoadScene("SecurityRoom");

                GlobalHandler.modality = 2;
                Init();
                break;
        }
        if (m_GripAction.GetStateDown(m_Pose.inputSource))
        {
            trials++;
            

            foreach (MaterialHolder materialobj in materialholderList)
            {
                materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;

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
                mdfw.WriteToFileCsv(new DataModel(inputtime, "correct password: " + passwordmodel.password));
                fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "correct password: " + passwordmodel.password));

                mdfw.StopRecording();
                fullmdfw.StopRecording();
                //fw.StopRecording();
               // fullfw.StopRecording();

            }
            feedbackElements.Add(new FeedbackModel(latestHitObject.gameObject, latestHitObject.gameObject.GetComponent<MeshRenderer>().material, feedbacktime));
            MeshRenderer renderer = latestHitObject.gameObject.GetComponent<MeshRenderer>();
            renderer.material = _pinhighlight;


        }
        else if (m_GrabAction.GetStateDown(m_Pose.inputSource) && !started && !stopped && current != null && GetNearestGameObject() != null && GlobalHandler.modality == 2)
        {


            Debug.Log("teeeeeeeeeeeeeeeeeeeeeeest");
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
                mdfw.WriteToFileCsv(new DataModel(inputtime, "correct password: " + passwordmodel.password));
                fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "correct password: " + passwordmodel.password));

                mdfw.StopRecording();
                fullmdfw.StopRecording();
                //fw.StopRecording();
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
                mdfw.WriteToFileCsv(new DataModel(inputtime, "correct password: " + passwordmodel.password));
                fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "correct password: " + passwordmodel.password));

                mdfw.StopRecording();
                fullmdfw.StopRecording();
                //fw.StopRecording();
                //fullfw.StopRecording();
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

                        //fw.StartRecording();
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
                        mdfw.WriteToFileCsv(new DataModel(inputtime, "correct password: " + passwordmodel.password));
                        fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "correct password: " + passwordmodel.password));

                        mdfw.StopRecording();
                        fullmdfw.StopRecording();
                        //fw.StopRecording();
                        //fullfw.StopRecording();
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
                        mdfw.WriteToFileCsv(new DataModel(inputtime, "correct password: " + passwordmodel.password));
                        fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "correct password: " + passwordmodel.password));

                        mdfw.StopRecording();
                        fullmdfw.StopRecording();
                        //fw.StopRecording();
                        //fullfw.StopRecording();
                    }

                }
            }
        }

        if (selectedPINS.Count == 4)
        {
            foreach (GameObject pin in GameObject.FindGameObjectsWithTag("PIN"))
            {
                pin.GetComponent<MeshRenderer>().material = _pinhighlight;

            }
            //selectedPINS.Add("empty");


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
        if(trials == 4)
        {
            foreach (GameObject pin in GameObject.FindGameObjectsWithTag("PIN"))
            {
                pin.GetComponent<MeshRenderer>().enabled = false;

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


    }



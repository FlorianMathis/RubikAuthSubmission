
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

public class GazeDwellHandler : MonoBehaviour
{

    // The material to use for active objects.

    public Material _inputcorrect;
    public Material _inputwrong;

    // The object that we hit.
    private ActiveObject _highlightInfo;

    public GameObject RaySource;

    // Whatever we need to run the calibration.
    private bool _calibratedSuccessfully;

    // Remember if we have saved data.
    private bool _hasSavedData;

    // Gaze trail script.
    private Tobii.Research.Unity.VRGazeTrail _gazeTrail;

    // Quit the app.
    private bool _quitTime;

    // The Unity EyeTracker helper object.
    private Tobii.Research.Unity.VREyeTracker _eyeTracker;
    //public List<Int32> PIN2;
    // scenes
    private Tobii.Research.Unity.IVRGazeData gazeData;
    public static PasswordModel passwordmodel;
    public FileWriter1 fw;
    public MetaDataWriter mdfw;


    // full tracking
    public FullFileWriter1 fullfw;
    public FullMetaDataWriter fullmdfw;
    private float fullinputtime;

    public List<MaterialHolder> materialholderList;

    private float inputtime;
    public float dwell_time;
    public float dwell_time_original;
    private GameObject currentGameobject;
    // The Unity EyeTracker helper object.
    public SteamVR_Action_Boolean m_GrabAction;
    public SteamVR_Action_Boolean m_GripAction;
    public static int counterCalls;
    // The Unity EyeTracker helper object.

    private SteamVR_Behaviour_Pose m_Pose = null;
    private bool started;
    private bool stopped;
    private List<String> selectedPINS;
    public float timerfinalgazedata;
    public int trials;


    public Material _pincorrect;
    public Material _pinwrong;
    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    void Start()
    {
        counterCalls++;
        trials = 3; // incl. 0
        Init();
    }
    public void Init()
    {
        switch (GlobalHandler.password)
        {
            case 1:
                passwordmodel = new PasswordModel(new List<String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                break;
            case 2:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX3G", "POSX4G", "POSX5G", "POSX6G" }, 4);
                break;
            case 3:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX3G", "POSX7G", "POSX9G" }, 4);
                break;
            case 4:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX9G", "POSX6G", "POSX5R", "POSX6R" }, 4);
                break;
            case 5:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX9G", "POSX1W", "POSX2W" }, 4);
                break;
            case 6:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX4G", "POSX5G", "POSX6G", "POSX5B" }, 4);
                break;
            case 7:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX4G", "POSX1W", "POSX2W", "POSX1O" }, 4);
                break;
            case 8:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX5R", "POSX1W" }, 4);
                break;
            case 9:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX5R", "POSX5B", "POSX6B" }, 4);
                break;
            case 10:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX7G", "POSX5W", "POSX9B", "POSX7R" }, 4);
                break;
            case 11:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX3G", "POSX3W", "POSX4O", "POSX5R" }, 4);
                break;
            case 12:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX3O", "POSX4B", "POSX5W" }, 4);
                break;
            case 13:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX6G", "POSX7G", "POSX8G" }, 4);
                break;
            case 14:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX6G", "POSX7G", "POSX8G", "POSX9G" }, 4);
                break;
            case 15:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX8G", "POSX9G" }, 4);
                break;
            case 16:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX7G", "POSX8G", "POSX1R", "POSX2R" }, 4);
                break;
            case 17:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX1W", "POSX2W", "POSX3W" }, 4);
                break;
            case 18:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX9G", "POSX8G", "POSX1B", "POSX2B" }, 4);
                break;
            case 19:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX4G", "POSX4W", "POSX6W", "POSX5O" }, 4);
                break;
            case 20:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX5R", "POSX1W", "POSX2W" }, 4);
                break;
            case 21:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX7G", "POSX7W", "POSX9W", "POSX7O" }, 4);
                break;
            case 22:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX2W", "POSX5B", "POSX9R" }, 4);
                break;
            case 23:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX1W", "POSX5B", "POSX6O" }, 4);
                break;
            case 24:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX1W", "POSX1R", "POSX5B" }, 4);
                break;
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

        timerfinalgazedata = 0.3f;
        started = false;
        stopped = false;
        fw = GameObject.Find("Storage").GetComponent<FileWriter1>();
        mdfw = GameObject.Find("Storage").GetComponent<MetaDataWriter>();
        fullfw = GameObject.Find("Storage").GetComponent<FullFileWriter1>();
        fullmdfw = GameObject.Find("Storage").GetComponent<FullMetaDataWriter>();
        fullfw.StartRecording();
        fullmdfw.StartRecording();
    }
    private void Update()
    {
        if (Input.inputString == "x")
        {
            fullfw.StopRecording();
            fw.StopRecording();
        }

        if ((Input.inputString == "1" || m_GripAction.GetStateDown(m_Pose.inputSource)) && stopped)
        //Input.inputString == "2" ||
        //Input.inputString == "3" ||
        //Input.inputString == "4" ||
        //Input.inputString == "5" ||
        //Input.inputString == "6")
        {

            fullfw.StopRecording();
            fw.StopRecording();
            Debug.Log(trials + "versuche");
            if (trials == 0)
            {
                GameObject.Find("Controller (left)").SetActive(false);
                return;
            }
            trials--;

            //SceneManager.LoadScene("3DAuthenticationSchemeGazeTrigger");

            foreach (MaterialHolder materialobj in materialholderList)
            {
                //Debug.Log("test");
                // Debug.Log(materialobj.Material);
                // Debug.Log(materialobj.Gameobject);
                //Debug.Log(_pincorrect);
                materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                //materialobj.Gameobject.GetComponent<MeshRenderer>().material = _pincorrect;
            }
            Init();
            dwell_time = dwell_time_original;

        }

        if (Input.inputString == "4")
        {
            // rotate cube
            // get side area (in this case left)
            GameObject.Find("Planes").transform.localRotation = Quaternion.Euler(0, 270, -90);
        }
        if (Input.inputString == "5")
        {
            // rotate cube
            // get back area
            GameObject.Find("Planes").transform.localRotation = Quaternion.Euler(-90, 270, -90);
        }
        if (Input.inputString == "6")
        {
            // rotate cube
            // bet bottom area
            GameObject.Find("Planes").transform.localRotation = Quaternion.Euler(180, 180, 0);
        }
        // track time
        inputtime += Time.deltaTime;
        fullinputtime += Time.deltaTime;

        if (m_GripAction.GetStateDown(m_Pose.inputSource))
        {
            GameObject.Find("Planes").transform.Rotate(0, 180, 0);
        }
        if (_eyeTracker.Connected)
        {
            var latestHitObject = _gazeTrail.LatestHitObject;

            if (latestHitObject != null)
            {
                if (currentGameobject == null) currentGameobject = latestHitObject.gameObject;
                if (currentGameobject == latestHitObject.gameObject)
                {
                    dwell_time -= Time.deltaTime;
                }
                else
                {
                    currentGameobject = latestHitObject.gameObject;
                    dwell_time = dwell_time_original;
                }
                if (latestHitObject.name.StartsWith("POS") && dwell_time <= 0 && !stopped)
                {
                    if (!started)
                    { // when to start with recording by using dwell time?
                        fw.StartRecording();
                        started = true;
                        mdfw.StartRecording();
                        inputtime = 0;
                    }
                    mdfw.WriteToFileCsv(new DataModel(inputtime, latestHitObject.name));
                    fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, latestHitObject.name));

                    passwordmodel.backupInput(latestHitObject.name);
                    selectedPINS.Add(currentGameobject.name);
                    if (selectedPINS.Count == 4)
                    {
                        stopped = true;
                        passwordmodel.entryError(selectedPINS);
                        mdfw.WriteToFileCsv(new DataModel(inputtime, "correct password: "+passwordmodel.password));
                        fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "correct password: " + passwordmodel.password));
                        //close all filewriter
                        mdfw.StopRecording();
                        fullmdfw.StopRecording();

                    }
                    MeshRenderer renderer = latestHitObject.gameObject.GetComponent<MeshRenderer>();
                    renderer.material = _inputcorrect;
                    dwell_time = dwell_time_original;
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

                }


            }
            _gazeTrail.ParticleColor = Color.blue;

        }
    }
}


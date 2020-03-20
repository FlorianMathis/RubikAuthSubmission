
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
using UnityEngine.UI;

public class GazeTriggerHandler : MonoBehaviour
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

    // The Unity EyeTracker helper object.
    private Tobii.Research.Unity.VREyeTracker _eyeTracker;
    //public List<Int32> PIN2;
    // scenes
    private Tobii.Research.Unity.IVRGazeData gazeData;
    public static PasswordModel passwordmodel;
    public FileWriter2 fw;
    public MetaDataWriter mdfw;

    // full tracking
    public FullFileWriter2 fullfw;
    public FullMetaDataWriter fullmdfw;

    private GameObject currentGameobject;
    // The Unity EyeTracker helper object.
    public SteamVR_Action_Boolean m_GrabAction;
    public SteamVR_Action_Boolean m_GripAction;
    // The Unity EyeTracker helper object.
    public static int counterCalls;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private bool started;
    private bool stopped;
    private float inputtime;
    private float fullinputtime;
    private List<String> selectedPINS;
    private float timerfinalgazedata;
    public Material _pincorrect;
    public Material _pinwrong;
    public List<MaterialHolder> materialholderList;
    public int trials;
    public GameObject password;
    public PasswordGenerator pwGenerator;

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    void Start()
    {
        counterCalls++;
        trials = 3; // incl. 0
        foreach (GameObject pin in GameObject.FindGameObjectsWithTag("PIN"))
        {
            materialholderList.Add(new MaterialHolder(pin.GetComponent<MeshRenderer>().material, pin.gameObject));
        }
        Init();
    }

    // called by anyone
    public void Init()
    {

        Debug.Log("trigger countercalls" + counterCalls);
        if (GlobalHandler.training)
        {
            passwordmodel = new PasswordModel(new List<String>() { "POSX1Y", "POSX2Y", "POSX3Y", "POSX4Y" }, 4);

        }
        else
        {
            switch (GlobalHandler.password)
        {
            case 1:
                passwordmodel = new PasswordModel(new List<String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword0S1");
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" });
                break;
            case 2:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX3G", "POSX4G", "POSX5G", "POSX6G" }, 4);
                //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword0S2");
                pwGenerator.generateCube(new List<string>() { "POSX3G", "POSX4G", "POSX5G", "POSX6G" });

                break;
            case 3:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX3G", "POSX7G", "POSX9G" }, 4);
                //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword0S3");
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX3G", "POSX7G", "POSX9G" });

                break;
            case 4:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX6G", "POSX7G", "POSX8G" }, 4);
                //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword1S1");
                pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX6G", "POSX7G", "POSX8G" });

                break;
            case 5:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX6G", "POSX7G", "POSX8G", "POSX9G" }, 4);
                //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword1S2");
                pwGenerator.generateCube(new List<string>() { "POSX6G", "POSX7G", "POSX8G", "POSX9G" });

                break;
            case 6:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX8G", "POSX9G" }, 4);
                //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword1S3");
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX2G", "POSX8G", "POSX9G" });

                break;
            case 7:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX9G", "POSX6G", "POSX5R", "POSX6R" }, 4);
                //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword2S1");
                pwGenerator.generateCube(new List<string>() { "POSX9G", "POSX6G", "POSX5R", "POSX6R" });

                break;
            case 8:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX9G", "POSX1W", "POSX2W" }, 4);
                //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword2S2");
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX9G", "POSX1W", "POSX2W" });

                break;
            case 9:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX4G", "POSX5G", "POSX6G", "POSX5B" }, 4);
                //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword2S3");
                pwGenerator.generateCube(new List<string>() { "POSX4G", "POSX5G", "POSX6G", "POSX5B" });

                break;
            case 10:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX7G", "POSX8G", "POSX1R", "POSX2R" }, 4);
                //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword3S1");
                pwGenerator.generateCube(new List<string>() { "POSX7G", "POSX8G", "POSX1R", "POSX2R" });

                break;
            case 11:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX1W", "POSX2W", "POSX3W" }, 4);
                //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword3S2");
                pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX1W", "POSX2W", "POSX3W" });

                break;
            case 12:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX9G", "POSX8G", "POSX1B", "POSX2B" }, 4);
                //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword3S3");
                pwGenerator.generateCube(new List<string>() { "POSX9G", "POSX8G", "POSX1B", "POSX2B" });

                break;
            case 13:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX4G", "POSX1W", "POSX2W", "POSX1O" }, 4);
                //password.GetComponent<RawImage>().texture = null;
                pwGenerator.generateCube(new List<string>() { "POSX4G", "POSX1W", "POSX2W", "POSX1O" });
                break;
            case 14:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX5R", "POSX1W" }, 4);
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX2G", "POSX5R", "POSX1W" });
                //password.GetComponent<RawImage>().texture = null;

                break;
            case 15:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX5R", "POSX5B", "POSX6B" }, 4);
                pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX5R", "POSX5B", "POSX6B" });
                //password.GetComponent<RawImage>().texture = null;

                break;
            case 16:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX4G", "POSX4W", "POSX6W", "POSX5O" }, 4);
                pwGenerator.generateCube(new List<string>() { "POSX4G", "POSX4W", "POSX6W", "POSX5O" });
                //password.GetComponent<RawImage>().texture = null;

                break;
            case 17:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX5R", "POSX1W", "POSX2W" }, 4);
                pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX5R", "POSX1W", "POSX2W" });
                //password.GetComponent<RawImage>().texture = null;

                break;
            case 18:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX7G", "POSX7W", "POSX9W", "POSX7O" }, 4);
                pwGenerator.generateCube(new List<string>() { "POSX7G", "POSX7W", "POSX9W", "POSX7O" });
                //password.GetComponent<RawImage>().texture = null;

                break;
            case 19:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX7G", "POSX5W", "POSX9B", "POSX7R" }, 4);
                pwGenerator.generateCube(new List<string>() { "POSX7G", "POSX5W", "POSX9B", "POSX7R" });
                //password.GetComponent<RawImage>().texture = null;

                break;
            case 20:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX3G", "POSX3W", "POSX4O", "POSX5R" }, 4);
                pwGenerator.generateCube(new List<string>() { "POSX3G", "POSX3W", "POSX4O", "POSX5R" });
                // password.GetComponent<RawImage>().texture = null;

                break;
            case 21:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX3O", "POSX4B", "POSX5W" }, 4);
                pwGenerator.generateCube(new List<string>() { "POSX2G", "POSX3O", "POSX4B", "POSX5W" });
                // password.GetComponent<RawImage>().texture = null;

                break;
            case 22:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX2W", "POSX5B", "POSX9R" }, 4);
                pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX2W", "POSX5B", "POSX9R" });
                //password.GetComponent<RawImage>().texture = null;

                break;
            case 23:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX1W", "POSX5B", "POSX6O" }, 4);
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX1W", "POSX5B", "POSX6O" });
                // password.GetComponent<RawImage>().texture = null;

                break;
            case 24:
                passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX1W", "POSX1R", "POSX5B" }, 4);
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX1W", "POSX1R", "POSX5B" });
                //password.GetComponent<RawImage>().texture = null;

                break;
        } }
        /*if (trials <= 1)
        {
            password.GetComponent<RawImage>().texture = null;
            foreach (MaterialHolder materialobj in materialholderList)
            {              
                materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
            }

        }*/

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
        timerfinalgazedata = 0.3f;
        fw = GameObject.Find("Storage").GetComponent<FileWriter2>();
        mdfw = GameObject.Find("Storage").GetComponent<MetaDataWriter>();
        fullfw = GameObject.Find("Storage").GetComponent<FullFileWriter2>();
        fullmdfw = GameObject.Find("Storage").GetComponent<FullMetaDataWriter>();
        fullfw.StartRecording();
        fullmdfw.StartRecording();
        Debug.Log(GameObject.Find("Planes").transform.rotation);
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
        }

       /* if (Input.inputString == "4") {
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
        }*/

        // track time
        inputtime += Time.deltaTime;
        fullinputtime += Time.deltaTime;
        //if (m_GripAction.GetStateDown(m_Pose.inputSource))
        //{
        //   GameObject.Find("Planes").transform.Rotate(0, 180, 0);
        // }

        if (_eyeTracker.Connected)
        {
            var latestHitObject = _gazeTrail.LatestHitObject;
            if (latestHitObject != null && latestHitObject.name.StartsWith("POS") && m_GrabAction.GetStateDown(m_Pose.inputSource) && !stopped)
            {
                if (!started)
                { // when to start with recording by using dwell time?
                    fw.StartRecording();
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
                    stopped = true;

                    passwordmodel.entryError(selectedPINS);
                    mdfw.WriteToFileCsv(new DataModel(inputtime, "correct password: " + passwordmodel.password));
                    fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "correct password: " + passwordmodel.password));

                    //close all filewriter
                    mdfw.StopRecording();
                    fullmdfw.StopRecording();

                }
                MeshRenderer renderer = latestHitObject.gameObject.GetComponent<MeshRenderer>();
                renderer.material = _inputcorrect;
                if (passwordmodel.getErrors() == 0 && selectedPINS.Count == 4)
                {
                    foreach (GameObject pin in GameObject.FindGameObjectsWithTag("PIN"))
                    {
                        pin.GetComponent<MeshRenderer>().material = _pincorrect;

                    }
                }
                else if(selectedPINS.Count == 4)
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



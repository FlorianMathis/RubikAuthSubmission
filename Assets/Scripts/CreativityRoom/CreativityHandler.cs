
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

public class CreativityHandler : MonoBehaviour
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

    // full tracking
    public FullCreativityWriter fullfw;
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
    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }

    void Start()
    {
        trials = 0;
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
        passwordmodel = new PasswordModel(new List<String>() { "POSX1G", "POSX1G", "POSX1G", "POSX1G" }, 4);
        //
        fw = GameObject.Find("Storage").GetComponent<CreativityWriter>();
        mdfw = GameObject.Find("Storage").GetComponent<MetaDataWriter>();
        fullfw = GameObject.Find("Storage").GetComponent<FullCreativityWriter>();
        fullmdfw = GameObject.Find("Storage").GetComponent<FullMetaDataWriter>();
        fullfw.StartRecording();
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
            trials = 0;
            foreach (GameObject pin in GameObject.FindGameObjectsWithTag("PIN"))
            {
                pin.GetComponent<MeshRenderer>().enabled = true;

            }
            Canvas weakpw = GameObject.Find("weakpw").GetComponent<Canvas>();
            Canvas mediumpw = GameObject.Find("mediumpw").GetComponent<Canvas>();
            Canvas strongpw = GameObject.Find("strongpw").GetComponent<Canvas>();

           

            // toggle display instruction
            fullfw.StopRecording();
            fullmdfw.StopRecording();
            foreach (MaterialHolder materialobj in materialholderList)
            { 
                materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
            }
            Init();
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
                mdfw.WriteToFileCsv(new DataModel(inputtime, "status: " + security));
                fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "status: " + security));

                mdfw.StopRecording();
                fullmdfw.StopRecording();
                fw.StopRecording();
                fullfw.StopRecording();

            }
            feedbackElements.Add(new FeedbackModel(latestHitObject.gameObject, latestHitObject.gameObject.GetComponent<MeshRenderer>().material, feedbacktime));
            MeshRenderer renderer = latestHitObject.gameObject.GetComponent<MeshRenderer>();
            renderer.material = _pinhighlight;


        }
        else if (m_GrabAction.GetStateDown(m_Pose.inputSource) && !started && !stopped && current != null && GetNearestGameObject() != null && GlobalHandler.modality == 2)
        {


            Debug.Log("teeeeeeeeeeeeeeeeeeeeeeest");
            fw.StartRecording();
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
                mdfw.WriteToFileCsv(new DataModel(inputtime, "status: " + security));
                fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "status: " + security));

                mdfw.StopRecording();
                fullmdfw.StopRecording();
                fw.StopRecording();
                fullfw.StopRecording();

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
                mdfw.WriteToFileCsv(new DataModel(inputtime, "status: " + security));
                fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "status: " + security));

                mdfw.StopRecording();
                fullmdfw.StopRecording();
                fw.StopRecording();
                fullfw.StopRecording();
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

                        fw.StartRecording();
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
                        mdfw.WriteToFileCsv(new DataModel(inputtime, "status: " + security));
                        fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "status: " + security));

                        mdfw.StopRecording();
                        fullmdfw.StopRecording();
                        fw.StopRecording();
                        fullfw.StopRecording();
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
                        mdfw.WriteToFileCsv(new DataModel(inputtime, "status: " + security));
                        fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "status: " + security));

                        mdfw.StopRecording();
                        fullmdfw.StopRecording();
                        fw.StopRecording();
                        fullfw.StopRecording();
                    }

                }
            }
        }

        Debug.Log("errors: " + passwordmodel.getErrors());
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
        if(trials == 3)
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



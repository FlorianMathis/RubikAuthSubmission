
//-----------------------------------------------------------------------
// Copyright © 2017 Tobii AB. All rights reserved.
//-----------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System; //This allows the IComparable Interface
using System.Collections.Generic;
using Tobii.Research.Unity.Examples;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{

    // The material to use for active objects.
    public Material _highlightMaterial;

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

    private float current_time;

    public List<String> userPassword;
    //public List<Int32> PIN2;
    private GameObject triggers;
    private SelectionController triggerScript;
    // scenes
    public String scene;
    private Tobii.Research.Unity.IVRGazeData gazeData;
    public float feedback_time;
    private float manouver_time;
    private List<FeedbackModel> feedbackmodel;
    public static PasswordModel passwordmodel;
    private int trials;
    private bool newtrial;
    public GameObject[] PINS;
    public FileWriter fw;
    private bool started;
    public string filename;

    private void Start()
    {
        manouver_time = 2;
        feedbackmodel = new List<FeedbackModel>();
        passwordmodel = new PasswordModel(new List<String>() { "POS1", "POS2", "POS3", "POS4" }, 4);
        triggers = GameObject.Find("Triggers");
        triggerScript = triggers.GetComponent<SelectionController>();
        newtrial = false;
        feedback_time = 1;
        // Get EyeTracker unity object
        _eyeTracker = Tobii.Research.Unity.VREyeTracker.Instance;
        if (_eyeTracker == null)
        {
            Debug.Log("Failed to find eye tracker, has it been added to scene?");
        }

        _gazeTrail = Tobii.Research.Unity.VRGazeTrail.Instance;

        _highlightInfo = new ActiveObject();

        started = false;

    }
    private void Update()
    {
        if (!started && triggerScript.getStatusTriggerRight())
        {
            Debug.Log("recording started");
            fw.StartRecording();
            started = true;
        }
        if (newtrial)
        {
            // reset all variables from the old trial
            for (int i = 0; i <= feedbackmodel.Count - 1; i++)
            {
                feedbackmodel[i].Gameobject.GetComponent<MeshRenderer>().enabled = false;
            }

            feedbackmodel = new List<FeedbackModel>();
            //passwordmodel = new PasswordModel(new List<String>() { "POS1", "POS2", "POS3", "POS4" }, 4);
            userPassword.Clear();
            if (trials <= 7)
            {
                // rotate back to original position

                trials++;
                newtrial = false;

            }
            else if (trials == 8)
            {
                GameObject.Find("tiger_idle").transform.Rotate(0, 90, 0);
                trials++;
                newtrial = false;
            }
            else if (trials == 9)
            {
                GameObject.Find("tiger_idle").transform.Rotate(0, 270, 0);
                trials++;
                newtrial = false;
            }
            else if (trials == 10)
            {
                GameObject.Find("tiger_idle").transform.Rotate(0, 180, 0);
                trials++;
                newtrial = false;
            }
            else SceneManager.LoadScene(sceneName: "playground");

        }
        // show 3D model and start recording (see DataHandler)
        if (Input.GetKeyDown(KeyCode.R))
        {
            newtrial = true;
            //Debug.Log("lets go");
            GameObject.Find("Tiger_Mesh").GetComponent<SkinnedMeshRenderer>().enabled = true;
            GameObject.Find("Collider").SetActive(true);

        }
        //current_time += Time.deltaTime;
        if (feedbackmodel.Count >= 0)
        {
            for (int i = 0; i <= feedbackmodel.Count - 1; i++)
            {
                feedbackmodel[i].Time -= Time.deltaTime;
                if (feedbackmodel[i].Time <= 0)
                {
                    feedbackmodel[i].Gameobject.GetComponent<MeshRenderer>().enabled = false;
                    feedbackmodel.RemoveAt(i);

                }
            }
        }

        //if (triggerScript.getStatusTriggerLeft())
        //{
        //Debug.Log("rotate graphical password");
        //  GameObject.Find("tiger_idle").transform.Rotate(0, 90, 0);
        // }


        if (_eyeTracker.Connected)
        {
            // Reset any priviously set active object and remove its highlight
            if (_highlightInfo.HighlightedObject != null)
            {
                var renderer = _highlightInfo.HighlightedObject.GetComponent<MeshRenderer>();
                _highlightInfo.HighlightedObject = null;
                _highlightInfo.OriginalObjectMaterial = null;
            }
            var latestHitObject = _gazeTrail.LatestHitObject;
            if (latestHitObject != null)
            {
                if (latestHitObject.gameObject != _highlightInfo.HighlightedObject &&
                    latestHitObject.name.StartsWith("POS") && triggerScript.getStatusTriggerRight())

                {
                    feedbackmodel.Add(new FeedbackModel(latestHitObject.gameObject, latestHitObject.gameObject.GetComponent<MeshRenderer>().material, feedback_time));
                    passwordmodel.backupInput(latestHitObject.gameObject.name);
                    // Debug.Log("selected via gaze and trigger");
                    MeshRenderer renderer = latestHitObject.gameObject.GetComponent<MeshRenderer>();
                    renderer.enabled = true;
                    renderer.material = _highlightMaterial;

                    userPassword.Add(latestHitObject.name);
                    // Debug.Log(latestHitObject.name);

                    if (userPassword.Count == 4)
                    {
                        Debug.Log("recording stopped");

                        fw.StopRecording();
                        Debug.Log("ok count 4");
                        Debug.Log(userPassword[0]);
                        Debug.Log(userPassword[1]);
                        Debug.Log(userPassword[2]);
                        Debug.Log(userPassword[3]);

                        PINS = GameObject.FindGameObjectsWithTag("PIN");
                        if (passwordmodel.checkPassword(userPassword))
                        {
                            Debug.Log("correct");
                            for (int i = 0; i <= PINS.Length - 1; i++)
                            {
                                PINS[i].GetComponent<MeshRenderer>().enabled = true;

                                PINS[i].GetComponent<MeshRenderer>().material = _inputcorrect;
                            }
                        }
                        else
                        {
                            Debug.Log("feedback wrong");

                            for (int i = 0; i <= PINS.Length - 1; i++)
                            {
                                PINS[i].GetComponent<MeshRenderer>().enabled = true;

                                PINS[i].GetComponent<MeshRenderer>().material = _inputwrong;
                            }
                        }

                        passwordmodel.entryError(userPassword);
                        manouver_time -= Time.deltaTime;
                        if (manouver_time <= 0)
                        {
                            newtrial = true;
                            manouver_time = 2;
                        }
                        // Debug.Log("new run");
                        //Debug.Log("number of errors " + passwordmodel.getErrors());
                        // Debug.Log(passwordmodel.checkPassword(userPassword));
                    }

                }
                _gazeTrail.ParticleColor = Color.blue;

            }
        }
    }
}

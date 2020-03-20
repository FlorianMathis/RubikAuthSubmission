//-----------------------------------------------------------------------
// Copyright © 2017 Tobii AB. All rights reserved.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using Tobii.Research.Unity;
using UnityEngine;
using Valve.VR;

public sealed class ActiveObject
{
    // The active GameObject.
    public GameObject HighlightedObject;

    // The previous material.
    public Material OriginalObjectMaterial;

    public ActiveObject()
    {
        HighlightedObject = null;
        OriginalObjectMaterial = null;
    }
}

public class PlaygroundController : MonoBehaviour
{
    // The text about how to start the calibration.
    public GameObject _textCalibration;

    // The background of the text.
    public GameObject _textBackground;

    // The material to use for active objects.
    public Material _highlightMaterial;

    // The object that we hit.
    private ActiveObject _highlightInfo;

    // Whatever we need to run the calibration.
    private bool _calibratedSuccessfully;

    // Remember if we have saved data.
    private bool _hasSavedData;

    // Gaze trail script.
    private VRGazeTrail _gazeTrail;

    // Toned down color when looking at sign.
    private Color _lookAtSignColor;

    // Quit the app.
    private bool _quitTime;

    // The Unity EyeTracker helper object
    private VREyeTracker _eyeTracker;
    private Collider collider;
    private bool blinkAvailable;
    private float delay;
    List<Vector3> history = new List<Vector3>();
    [SerializeField]
    [Tooltip("saccade minimum amplitude in degrees")]
    [Range(0.0f, 2.0f)]
    public float SACCADETHRESHOLD = 1.5f; // visual angle to detect saccade
                                          // smoothing parameters

    [SerializeField]
    [Tooltip("interval")]
    [Range(1, 3)]
    public float interval = 2;

    public SteamVR_Action_Vector2 touchPadAction;
    private void Start()
    {
        // Get EyeTracker unity object
        _eyeTracker = VREyeTracker.Instance;
        if (_eyeTracker == null)
        {
            Debug.Log("Failed to find eye tracker, has it been added to scene?");
        }

        _gazeTrail = VRGazeTrail.Instance;
        _lookAtSignColor = new Color(0, 1, 0, 0.2f);

        _highlightInfo = new ActiveObject();
        var textRenderer = _textCalibration.GetComponent<Renderer>();
        textRenderer.sortingOrder -= 1;
        blinkAvailable = true;
        delay = 5;
    }

    private void Update()
    {



        SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand);
        Vector2 touchpadValue = touchPadAction.GetAxis(SteamVR_Input_Sources.Any);
        // Debug.Log(touchpadValue);

        if (touchpadValue != Vector2.zero)
        {
            //Debug.Log(touchpadValue);
        }
        if (_eyeTracker.Connected)
        {
            //Debug.Log(_eyeTracker.LatestProcessedGazeData.Left.GazeRayWorldValid ? _eyeTracker.LatestProcessedGazeData.Left.GazeRayWorld.ToString() : "No gaze");
            //Debug.Log(_eyeTracker.LatestProcessedGazeData.Left.GazeRayWorldValid);
            delay -= Time.deltaTime;


            // Reset any priviously set active object and remove its highlight
            if (_highlightInfo.HighlightedObject != null)
            {
                if (delay <= 0 && !_eyeTracker.LatestProcessedGazeData.Left.GazeRayWorldValid && _highlightInfo.HighlightedObject.tag == "Interactable" && _highlightInfo.HighlightedObject.name.StartsWith("Stone"))
                {
                    //_highlightInfo.HighlightedObject.transform.Rotate(new Vector3(45, 45, 0));
                    _highlightInfo.HighlightedObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 11, 1.6f);

                    //Debug.Log("rotating object");
                    delay = 5;

                }

                // implement move object with saccades
                // if(delay <= 0 && saccadedetected (based on two windows)
                interval -= Time.deltaTime;
                history.Add(_eyeTracker.LatestGazeData.CombinedGazeRayWorld.direction);
                // Debug.Log("current direction of gaze: " + _eyeTracker.LatestGazeData.CombinedGazeRayWorld.direction);
                /*if (history.Count >= 2 && interval <= 0 )
                {
                    // check angle of current and previous gaze sample. if bigger, its a saccade, and then reset history
                    Vector3 currentgaze = history[history.Count - 1];
                    Vector3 lastgaze = history[history.Count - 2];

                    Vector3 origin = Vector3.zero;
                    Vector3 currentGazeDirection = currentgaze - origin;
                    Vector3 lastGazeDirection = lastgaze - origin;
                    
                    float currentangle = Vector3.Angle(currentGazeDirection, lastGazeDirection);
                    
                    if (currentangle > SACCADETHRESHOLD && _highlightInfo.HighlightedObject.GetComponent<Rigidbody>() != null)
                    {
                        //saccade
                        Debug.Log("it's a saccade!");
                        // move active object to the right
                        //_highlightInfo.HighlightedObject.transform.Translate(new Vector3(0, 1, 1));
                        _highlightInfo.HighlightedObject.GetComponent<Rigidbody>().velocity = new Vector3(1,10,1);


                    }
                    interval = 2; 

                */


                var renderer = _highlightInfo.HighlightedObject.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = _highlightInfo.OriginalObjectMaterial;
                }

                _highlightInfo.HighlightedObject = null;
                _highlightInfo.OriginalObjectMaterial = null;
            }

            var latestHitObject = _gazeTrail.LatestHitObject;

            if (latestHitObject != null)
            {
                if (latestHitObject.gameObject != _highlightInfo.HighlightedObject &&
                    (latestHitObject.name.StartsWith("Throw") || latestHitObject.name.StartsWith("Cylinder") || latestHitObject.name.StartsWith("Stone")))
                {
                    MeshRenderer renderer = latestHitObject.gameObject.GetComponent<MeshRenderer>();
                    if (renderer != null)
                    {
                        _highlightInfo.HighlightedObject = latestHitObject.gameObject;
                        _highlightInfo.OriginalObjectMaterial = renderer.material;
                        renderer.material = _highlightMaterial;
                    }
                }

                if (latestHitObject.gameObject == _textBackground || latestHitObject.gameObject == _textCalibration)
                {
                    _gazeTrail.ParticleColor = _lookAtSignColor;
                }
                else
                {
                    _gazeTrail.ParticleColor = Color.blue;
                }
            }
        }
    }


}

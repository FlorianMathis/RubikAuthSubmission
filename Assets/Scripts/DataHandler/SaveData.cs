using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tobii.Research.Unity;
using UnityEngine;

public class SaveData : MonoBehaviour {
	public string fileName = "TrackData/TestTime.csv";
	public delegate void RecordStartEventHandler (float recordStart);

	public static event RecordStartEventHandler OnRecordStart;
	private float recordingStart = 0;
	private Camera cam;
	private VREyeTracker eyeTracker;
	private IVRGazeData currentGazeData;
	private StreamWriter sw;
	public int bufferSize = 65536;
	public bool isRecording = false;
	private int isCreat = 0;

	public int number;
	private long currTobiiTime;
	public GameObject motion1;
	public GameObject motion2;
	private Vector3 target1;
	private Vector3 target2;

	// Use this for initialization
	void Start () {
		cam = Camera.main;

		target1 = cam.WorldToScreenPoint (motion1.transform.position);
		target2 = cam.WorldToScreenPoint (motion2.transform.position);

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.R)) {
			isCreat++;
			isRecording = !isRecording;

			if (isCreat == 1) {
				createCsvFile (fileName);
				isCreat++;
			}

			if (isRecording) {
				StartRecording ();
			} else {
				StopRecording ();
				isCreat = 0;
			}
		}
	}

	void StartRecording () {

		Tracking_Data_Manager.OnNewGazeData += WriteToFileCsv;

		recordingStart = Time.unscaledTime;

		if (OnRecordStart != null) {
			OnRecordStart (recordingStart);
		} else {
			// Debug.Log ("There is no subscriptions to OnRecordStart");
		}
	}

	void StopRecording () {

		Tracking_Data_Manager.OnNewGazeData -= WriteToFileCsv;
		closeCsvFile ();
		if (OnRecordStart != null) {
			OnRecordStart (-1f);
		} else {
			// Debug.Log ("There is no subscriptions to OnRecordStart");
		}

	}

	void createCsvFile (string path) {
		try {
			sw = new StreamWriter (path, true, Encoding.UTF8, bufferSize);
			string header = string.Format (
				"{0},{1},{2},{3},{4},{5},{6},{7}, {8}",
				"Nr.",
				"Time",
				"Tobii_Timestamp",
				"Gaze_x",
				"Gaze_Y",
				"Motion1_X",
				"Motion1_Y",
				"Motion2_X",
				"Motion2_Y"

			);

			sw.WriteLine (header);
			// Debug.Log ("Opening StreamWriter");
		} catch (IOException e) {
			Debug.LogError (e.Message);
		}
	}

	string FormatInputToCsv (IVRGazeData input) {

		Vector3 headDirection = input.Pose.Rotation * Vector3.forward;
		Vector2 gazePosLeft = input.Left.PupilPosiitionInTrackingArea;
		Vector2 gazePosRight = input.Right.PupilPosiitionInTrackingArea;

		Vector2 gazePoint = new Vector2 ((gazePosLeft.x + gazePosRight.x) / 2, (gazePosLeft.y + gazePosRight.y) / 2);

		double t1X = target1.x / cam.pixelWidth;
		double t1Y = 1 - target1.y / cam.pixelHeight;

		double t2X = target2.x / cam.pixelWidth;
		double t2Y = 1 - target2.y / cam.pixelHeight;

		// Debug.Log ("play1: " + tX + "   " + tY);
		// Debug.Log ("gaze " + gazePoint.x + "   " + gazePoint.y);

		string line = string.Format (
			"{0},{1},{2},{3},{4},{5},{6},{7}, {8}",
			number,
			(Time.unscaledTime - recordingStart).ToString (),
			currTobiiTime,
			gazePoint.x,
			gazePoint.y,
			t1X,
			t1Y,
			t2X,
			t2Y

		);
		// Debug.Log (line);
		return line;
	}

	void WriteToFileCsv (IVRGazeData input) {
		if (input != null) {
			currTobiiTime = input.TimeStamp;
			if (FormatInputToCsv (input) == null) {
				// Debug.Log ("There is nothing");
			}

			sw.WriteLine (FormatInputToCsv (input));
		}

	}

	void closeCsvFile () {
		// Debug.Log ("Closing StreamWriter");
		sw.Flush ();
		sw.Dispose ();
		sw.Close ();
	}
}
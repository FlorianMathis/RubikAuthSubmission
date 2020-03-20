using Tobii.Research.Unity;
using UnityEngine;

public class Tracking_Data_Manager : MonoBehaviour {

	private VREyeTracker eyeTracker;
	private IVRGazeData m_currentGazeData;
	public delegate void NewGazeDataEventHandler (IVRGazeData newGazeData);
	public static event NewGazeDataEventHandler OnNewGazeData;

	void Start () {
		eyeTracker = VREyeTracker.Instance;
	}

	void Update () {
		while (eyeTracker.GazeDataCount > 0) {
			currentGazeData = eyeTracker.NextData;
		}
	}

	public IVRGazeData currentGazeData {
		get {
			return m_currentGazeData;
		}
		set {
			if (m_currentGazeData == value) {
				return;
			}
			m_currentGazeData = value;
			if (OnNewGazeData != null) {
				OnNewGazeData (m_currentGazeData);
			}
		}
	}

}
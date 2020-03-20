using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tobii.Research.Unity;
using UnityEngine;

public class SecurityWriter : MonoBehaviour
{

    //public string fileName = "";
    private StreamWriter sw;
    public int bufferSize = 65536;

    public delegate void RecordStartEventHandler(float recordStart);
    public static event RecordStartEventHandler OnRecordStart;
    private float recordingStart = 0;
    private bool isRecording = false;
    private GameObject nonDominantHand;
    private GameObject dominantHand;
    private Camera m_Camera;
    public PasswordModel pm;
    public string fileName;

    // Use this for initialization
    void Start()
    {
        // change this to tappinghandler or InputHandler if that scene
        //pm = InputHandler.passwordmodel;
        //pm = TappingHandler.passwordmodel;
    }

    // Update is called once per frame
    public void StartRecording()
    {

        //pm = TappingHandler.passwordmodel;
        // pm = GazeDwellHandler.passwordmodel;
        //pm = GazeTriggerHandler.passwordmodel;
        pm = CreativityHandler.passwordmodel;
        Debug.Log(fileName);
        createCsvFile(GlobalHandler.dynamicpath + fileName);
        Tracking_Data_Manager.OnNewGazeData += WriteToFileCsv;

        isRecording = !isRecording;
        recordingStart = Time.unscaledTime;
        if (OnRecordStart != null)
        {
            OnRecordStart(recordingStart);
        }
        else
        {
            Debug.Log("There is no subscriptions to OnRecordStart");
        }
    }

    public void StopRecording()
    {
        isRecording = !isRecording;
        Tracking_Data_Manager.OnNewGazeData -= WriteToFileCsv;
        closeCsvFile();
        if (OnRecordStart != null)
        {
            OnRecordStart(-1f);
        }
        else
        {
            Debug.Log("There is no subscriptions to OnRecordStart");
        }

    }

    void WriteToFileCsv(IVRGazeData input)
    {
        //Debug.Log("wirting to file");
        sw.WriteLine(FormatInputToCsv(input));
    }

    string FormatInputToCsv(IVRGazeData input)
    {
        //Debug.Log("erroooooooooooooors" + pm.getErrors());
        //Vector3 headDirection = input.Pose.Rotation * Vector3.forward; 
        //headDirection.normalized.x,
        //headDirection.normalized.y,
        //headDirection.normalized.z,
        Debug.Log(input.Left.PupilDiameter);
        /* string line = string.Format(
                 "{0},{1}",
             (Time.unscaledTime - recordingStart).ToString(),
             input.TimeStamp.ToString()
          );
         return line;*/

        string line = string.Format(
              "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}," +
              "{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25}," +
              "{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43}," +
              "{44},{45},{46},{47},{48},{49},{50},{51},{52},{53},{54},{55},{56},{57},{58},{59},{60},{61},{62}",
          (Time.unscaledTime - recordingStart).ToString(),
          input.TimeStamp.ToString(),
          // global elements for hand movements
          // dominanthand global rotation x - z
          GameObject.FindGameObjectWithTag("Right").transform.rotation.x,
          GameObject.FindGameObjectWithTag("Right").transform.rotation.y,
          GameObject.FindGameObjectWithTag("Right").transform.rotation.z,
          // dominanthand position x - z
          GameObject.FindGameObjectWithTag("Right").transform.position.x,
          GameObject.FindGameObjectWithTag("Right").transform.position.y,
          GameObject.FindGameObjectWithTag("Right").transform.position.z,
          // non-dominanthand rotation x - z
          GameObject.FindGameObjectWithTag("Left").transform.rotation.x,
          GameObject.FindGameObjectWithTag("Left").transform.rotation.y,
          GameObject.FindGameObjectWithTag("Left").transform.rotation.z,
          // non-dominanthand position x - z
          GameObject.FindGameObjectWithTag("Left").transform.position.x,
          GameObject.FindGameObjectWithTag("Left").transform.position.y,
          GameObject.FindGameObjectWithTag("Left").transform.position.z,
          // local elements for hand movements (can get calculated from the camera)
          // dominanthand local rotation x - z
          GameObject.FindGameObjectWithTag("Right").transform.localRotation.x,
          GameObject.FindGameObjectWithTag("Right").transform.localRotation.y,
          GameObject.FindGameObjectWithTag("Right").transform.localRotation.z,
          // dominanthand position x - z
          GameObject.FindGameObjectWithTag("Right").transform.localPosition.x,
          GameObject.FindGameObjectWithTag("Right").transform.localPosition.y,
          GameObject.FindGameObjectWithTag("Right").transform.localPosition.z,
          // non-dominanthand rotation x - z
          GameObject.FindGameObjectWithTag("Left").transform.localRotation.x,
          GameObject.FindGameObjectWithTag("Left").transform.localRotation.y,
          GameObject.FindGameObjectWithTag("Left").transform.localRotation.z,
          // non-dominanthand position x - z
          GameObject.FindGameObjectWithTag("Left").transform.localPosition.x,
          GameObject.FindGameObjectWithTag("Left").transform.localPosition.y,
          GameObject.FindGameObjectWithTag("Left").transform.localPosition.z,
          // camera (fov) information
          Camera.main.transform.rotation.x,
          Camera.main.transform.rotation.y,
          Camera.main.transform.rotation.z,
          Camera.main.transform.position.x,
          Camera.main.transform.position.y,
          Camera.main.transform.position.z,
          input.Pose.Rotation.x,
          input.Pose.Rotation.y,
          input.Pose.Rotation.z,
          input.Pose.Position.x,
          input.Pose.Position.y,
          input.Pose.Position.z,
          input.Left.GazeOrigin.x,
          input.Left.GazeOrigin.y,
          input.Left.GazeOrigin.z,
          input.Right.GazeOrigin.x,
          input.Right.GazeOrigin.y,
          input.Right.GazeOrigin.z,
          input.Left.GazeDirection.x,
          input.Left.GazeDirection.y,
          input.Left.GazeDirection.z,
          input.Right.GazeDirection.x,
          input.Right.GazeDirection.y,
          input.Right.GazeDirection.z,
          input.CombinedGazeRayWorld.direction.normalized.x,
          input.CombinedGazeRayWorld.direction.normalized.y,
          input.CombinedGazeRayWorld.direction.normalized.z,
          input.CombinedGazeRayWorld.origin.x,
          input.CombinedGazeRayWorld.origin.y,
          input.CombinedGazeRayWorld.origin.z,
          input.Left.PupilDiameter,
          input.Right.PupilDiameter,
          input.Left.PupilPosiitionInTrackingArea.x, // tobii sdk fix Grant [Tobii]
          input.Left.PupilPosiitionInTrackingArea.y,
          input.Right.PupilPosiitionInTrackingArea.x,
          input.Right.PupilPosiitionInTrackingArea.y,
          //todo input data, class communication/singleton?
          //pm.getErrors(),
          //pm.getErrorPos()[0],
          // pm.getErrorPos()[1],
          ////pm.getErrorPos()[2],
          //pm.getErrorPos()[3],
          pm.getUserInput()
          //GameObject.Find("Triggers").GetComponent<SelectionController>().getStatusTriggerRight()
        );
        return line;
    }

    void createCsvFile(string path)
    {
        try
        {
            sw = new StreamWriter(path, true, Encoding.UTF8, bufferSize);
            /*string header = string.Format(
                "{0},{1}",
                "Time",
                "Tobii_Timestamp"
            );*/


            string header = string.Format(
                "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}," +
                "{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25}," +
                "{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43}," +
                "{44},{45},{46},{47},{48},{49},{50},{51},{52},{53},{54},{55},{56},{57},{58},{59},{60},{61},{62}",
                "Time",
                "Tobii_Timestamp",
                "GlobalDominantHand_Rotation_X",
                "GlobalDominantHand_Rotation_Y",
                "GlobalDominantHand_Rotation_Z",
                "GlobalDominantHand_Position_X",
                "GlobalDominantHand_Position_Y",
                "GlobalDominantHand_Position_Z",
                "GlobalNonDominantHand_Rotation_X",
                "GlobalNonDominantHand_Rotation_Y",
                "GlobalNonDominantHand_Rotation_Z",
                "GlobalNonDominantHand_Position_X",
                "GlobalNonDominantHand_Position_Y",
                "GlobalNonDominantHand_Position_Z",
                "LocalDominantHand_Rotation_X",
                "LocalDominantHand_Rotation_Y",
                "LocalDominantHand_Rotation_Z",
                "LocalDominantHand_Position_X",
                "LocalDominantHand_Position_Y",
                "LocalDominantHand_Position_Z",
                "LocalNonDominantHand_Rotation_X",
                "LocalNonDominantHand_Rotation_Y",
                "LocalNonDominantHand_Rotation_Z",
                "LocalNonDominantHand_Position_X",
                "LocalNonDominantHand_Position_Y",
                "LocalNonDominantHand_Position_Z",
                "Camera_Rotation_X",
                "Camera_Rotation_Y",
                "Camera_Rotation_Z",
                "Camera_Position_X",
                "Camera_Position_Y",
                "Camera_Position_Z",
                "Head_Rotation_X",
                "Head_Rotation_Y",
                "Head_Rotation_Z",
                "Head_Position_X",
                "Head_Position_Y",
                "Head_Position_Z",
                "Eye_GazePosition_Left_Position_X",
                "Eye_GazePosition_Left_Position_Y",
                "Eye_GazePosition_Left_Position_Z",
                "Eye_GazePosition_Right_Position_X",
                "Eye_GazePosition_Right_Position_Y",
                "Eye_GazePosition_Right_Position_Z",
                "Eye_GazeDirection_Left_Position_X",
                "Eye_GazeDirection_Left__Position_Y",
                "Eye_GazeDirection_Left__Position_Z",
                "Eye_GazeDirection_Right_Position_X",
                "Eye_GazeDirection_Right__Position_Y",
                "Eye_GazeDirection_Right__Position_Z",
                "CombinedGazeRayWorld_Eye_Direction_X",
                "CombinedGazeRayWorld_Eye_Direction_Y",
                "CombinedGazeRayWorld_Eye_Direction_Z",
                "CombinedGazeRayWorld_Eye_Position_X",
                "CombinedGazeRayWorld_Eye_Position_Y",
                "CombinedGazeRayWorld_Eye_Position_Z",
                "PupilDiameter_Left",
                "PupilDiameter_Right",
                "PupilPositionInTrackingArea_Left_X",
                "PupilPositionInTrackingArea_Left_Y",
                "PupilPositionInTrackingArea_Right_X",
                "PupilPositionInTrackingArea_Right_Y",
                "UserAreaInput"
            );


            sw.WriteLine(header);
            Debug.Log("Opening StreamWriter");
        }
        catch (IOException e)
        {
            Debug.LogError(e.Message);
        }
    }

    void closeCsvFile()
    {
        Debug.Log("Closing StreamWriter");
        sw.Flush();
        sw.Dispose();
        sw.Close();
    }

}
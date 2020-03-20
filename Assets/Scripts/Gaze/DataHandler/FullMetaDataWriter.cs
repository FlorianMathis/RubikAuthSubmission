using System.IO;
using System.Text;
using Tobii.Research.Unity;
using UnityEngine;

public class FullMetaDataWriter : MonoBehaviour
{

    //public string fileName = "";
    private StreamWriter sw;
    public int bufferSize = 65536;
    public string fileName;
    public void StartRecording()
    {
        createCsvFile(GlobalHandler.dynamicpath+fileName);
    }

    public void StopRecording()
    {
        closeCsvFile();
           }

    public void WriteToFileCsv(DataModel data)
    {
        //Debug.Log("wirting to file");
        sw.WriteLine(FormatInputToCsv(data));
    }

    string FormatInputToCsv(DataModel data)
    {
        string line = string.Format(
             "{0},{1}",
          data.Timestamp,
          data.UserInput
          //pm.getUserInput()
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
                "{0},{1}",
                "Timestamp",
                "UserInput"

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
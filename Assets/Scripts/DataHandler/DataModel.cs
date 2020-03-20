using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataModel : MonoBehaviour
{
    // Start is called before the first frame update
    public float Timestamp;
    public string UserInput;

    public DataModel(float timestamp, string userInput)
    {
        Timestamp = timestamp;
        UserInput = userInput;
    }
   
}

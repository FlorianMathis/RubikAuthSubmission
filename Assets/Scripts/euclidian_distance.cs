using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class euclidian_distance : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject digit2, digit1;
    public float distance;

    public string fileName = "euclidian_distance.txt";
    public GameObject origin, destination;
   
    void Start()
    {

        String[] digits = { "POSX1G", "POSX2G", "POSX3G", "POSX4G", "POSX5G", "POSX6G", "POSX7G", "POSX8G", "POSX9G",
                            "POSX1W", "POSX2W", "POSX3W", "POSX4W", "POSX5W", "POSX6W", "POSX7W", "POSX8W", "POSX9W",
                            "POSX1R", "POSX2R", "POSX3R", "POSX4R", "POSX5R", "POSX6R", "POSX7R", "POSX8R", "POSX9R",
                            "POSX1O", "POSX2O", "POSX3O", "POSX4O", "POSX5O", "POSX6O", "POSX7O", "POSX8O", "POSX9O",
                            "POSX1B", "POSX2B", "POSX3B", "POSX4B", "POSX5B", "POSX6B", "POSX7B", "POSX8B", "POSX9B"};
        List<float> distances = new List<float>();
        foreach (String digit in digits)
        {
            origin = GameObject.Find(digit);
            foreach (String digitcomp in digits)
            {
                destination = GameObject.Find(digitcomp);
                distance = Vector3.Distance(origin.transform.GetComponent<Renderer>().bounds.center, destination.transform.GetComponent<Renderer>().bounds.center);
                distances.Add(distance);
            }
        }
        Debug.Log(distances.Count);
        if (File.Exists(fileName))
        {
            Debug.Log(fileName + " already exists.");
            return;
        }
        var sr = File.CreateText(fileName);
        foreach (float distance in distances)
        {
            sr.WriteLine(distance);
        }
        sr.Close(); 



        //7G - 7W; 7G - 9B
        /* digit1 = GameObject.Find("POSX7G");
         digit2 = GameObject.Find("POSX7W");
         distance = Vector3.Distance(digit1.transform.GetComponent<Renderer>().bounds.center, digit2.transform.GetComponent<Renderer>().bounds.center);
         Debug.Log("euclidian distance 7g - 7w start");
    // https://docs.microsoft.com/en-us/dotnet/api/system.numerics.vector3.distance?view=netframework-4.8
         Debug.Log(distance);
         Debug.Log("euclidian distance stop");
         digit1 = GameObject.Find("POSX7G");
         digit2 = GameObject.Find("POSX9B");
         distance = Vector3.Distance(digit1.transform.GetComponent<Renderer>().bounds.center, digit2.transform.GetComponent<Renderer>().bounds.center);
         Debug.Log("euclidian distance 7g - 9b start");
         // https://docs.microsoft.com/en-us/dotnet/api/system.numerics.vector3.distance?view=netframework-4.8
         Debug.Log(distance);
         Debug.Log("euclidian distance stop");*/



        /*if (File.Exists(fileName))
        {
            Debug.Log(fileName + " already exists.");
            return;
        }
        var sr = File.CreateText(fileName);
        sr.WriteLine(distances);
        
        sr.Close();*/

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

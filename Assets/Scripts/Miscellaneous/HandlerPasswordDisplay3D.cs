using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class HandlerPasswordDisplay3D : MonoBehaviour
{
    public PasswordGenerator pwGenerator;
    // Update is called once per frame
    public List<MaterialHolder> materialholderList;

    public SteamVR_Action_Boolean m_GripAction;
    private SteamVR_Behaviour_Pose m_Pose = null;
    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }
    private void Start()
    {
        GlobalHandler.representation = 1;
        foreach (GameObject pin in GameObject.FindGameObjectsWithTag("PIN"))
        {
            materialholderList.Add(new MaterialHolder(pin.GetComponent<MeshRenderer>().material, pin.gameObject));
        }
    }
    void Update()
    {
        //if (m_GripAction.GetStateDown(m_Pose.inputSource))
        //{
         //   GameObject.Find("Planes").transform.Rotate(0, 180, 0);
       // }
        switch (Input.inputString)
        {
            //zero switches
            case "1":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" });
                GlobalHandler.password = 1;
                break;
            case "2":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX3G", "POSX4G", "POSX5G", "POSX6G" });
                GlobalHandler.password = 2;
                break;
            case "3":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX3G", "POSX7G", "POSX9G" });
                GlobalHandler.password = 3;
                break;
            case "4":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX6G", "POSX7G", "POSX8G" });
                GlobalHandler.password = 4;
                break;
            case "5":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX6G", "POSX7G", "POSX8G", "POSX9G" });
                GlobalHandler.password = 5;
                break;

            case "6":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX2G", "POSX8G", "POSX9G" });
                GlobalHandler.password = 6;
                break;
            // 1 switch
            case "7":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX9G", "POSX6G", "POSX5R", "POSX6R" });
                GlobalHandler.password = 7;
                break;
            case "8":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX9G", "POSX1W", "POSX2W" });
                GlobalHandler.password = 8;
                break;
            case "9":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX4G", "POSX5G", "POSX6G", "POSX5B" });
                GlobalHandler.password = 9;
                break;
            case "0":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX7G", "POSX8G", "POSX1R", "POSX2R" });
                GlobalHandler.password = 10;

                break;
            case "-":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX1W", "POSX2W", "POSX3W" });
                GlobalHandler.password = 11;

                break;

            case "=":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX9G", "POSX8G", "POSX1B", "POSX2B" });
                GlobalHandler.password = 12;

                break;
            // 2 switches
            case "q":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX4G", "POSX1W", "POSX2W", "POSX1O" });
                GlobalHandler.password = 13;
                break;
            case "w":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX2G", "POSX5R", "POSX1W" });
                GlobalHandler.password = 14;
                break;
            case "e":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX5R", "POSX5B", "POSX6B" });
                GlobalHandler.password = 15;
                break;

            case "r":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX4G", "POSX4W", "POSX6W", "POSX5O" });
                GlobalHandler.password = 16;

                break;
            case "t":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX5R", "POSX1W", "POSX2W" });
                GlobalHandler.password = 17;

                break;
            case "y":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX7G", "POSX7W", "POSX9W", "POSX7O" });
                GlobalHandler.password = 18;

                break;
            // 3 switches
            case "u":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX7G", "POSX5W", "POSX9B", "POSX7R" });
                GlobalHandler.password = 19;
                break;
            case "i":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX3G", "POSX3W", "POSX4O", "POSX5R" });
                GlobalHandler.password = 20;
                break;
            case "o":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX2G", "POSX3O", "POSX4B", "POSX5W" });
                GlobalHandler.password = 21;
                break;
            case "p":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX2W", "POSX5B", "POSX9R" });
                GlobalHandler.password = 22;
                break;
            case "[":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX1W", "POSX5B", "POSX6O" });
                GlobalHandler.password = 23;
                break;
            case "]":
                foreach (MaterialHolder materialobj in materialholderList)
                {
                    materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                }
                pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX1W", "POSX1R", "POSX5B" });
                GlobalHandler.password = 24;
                break;
           
        }
    }
}

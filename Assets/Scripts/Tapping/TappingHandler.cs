using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Valve.VR;

public class TappingHandler : MonoBehaviour
{

    public SteamVR_Action_Boolean m_GrabAction = null;
    private SteamVR_Behaviour_Pose m_Pose = null;
    public SteamVR_Action_Vibration hapticAction;
    private FixedJoint m_Joint = null;
    public Material tapped;
    private Interactable m_CurrentInteractable = null;
    public List<Interactable> m_ContactInteractables = new List<Interactable>();
    private List<GameObject> current = new List<GameObject>();
    public List<GameObject> Userinput = new List<GameObject>();

    public FileWriter fw;
    public MetaDataWriter mdfw;
    private float inputtime;
    private bool started;
    private bool stopped;
    private int count;
    public SteamVR_Action_Boolean m_GripAction;
    public PasswordGenerator pwGenerator;

    // The Unity EyeTracker helper object.

    public static PasswordModel passwordmodel;


    // full tracking
    public FullFileWriter fullfw;
    public FullMetaDataWriter fullmdfw;
    private float fullinputtime;
    public static int counterCalls;
    public Material _pincorrect;
    public Material _pinwrong;
    public List<MaterialHolder> materialholderList;
    public int trials;
    // The Unity EyeTracker helper object.
    private Hand hand;
    public GameObject password;

    // Start is called before the first frame update
    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject pin in GameObject.FindGameObjectsWithTag("Interactable"))
        {
            materialholderList.Add(new MaterialHolder(pin.GetComponent<MeshRenderer>().material, pin.gameObject));
            Debug.Log("added");
        }
        counterCalls++;
        trials = 3; // incl. 0
        Init();

    }


    public void Init()
    {
        if (GlobalHandler.training)
        {
            passwordmodel = new PasswordModel(new List<String>() { "POSX1Y", "POSX2Y", "POSX3Y", "POSX4Y" }, 4);

        }
        else
        {
            switch (GlobalHandler.password)
            {
                case 1:
                    passwordmodel = new PasswordModel(new List<String>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" }, 4);
                    //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword0S1");
                    pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX2G", "POSX3G", "POSX4G" });
                    break;
                case 2:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX3G", "POSX4G", "POSX5G", "POSX6G" }, 4);
                    //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword0S2");
                    pwGenerator.generateCube(new List<string>() { "POSX3G", "POSX4G", "POSX5G", "POSX6G" });

                    break;
                case 3:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX3G", "POSX7G", "POSX9G" }, 4);
                    //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword0S3");
                    pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX3G", "POSX7G", "POSX9G" });

                    break;
                case 4:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX6G", "POSX7G", "POSX8G" }, 4);
                    //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword1S1");
                    pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX6G", "POSX7G", "POSX8G" });

                    break;
                case 5:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX6G", "POSX7G", "POSX8G", "POSX9G" }, 4);
                    //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword1S2");
                    pwGenerator.generateCube(new List<string>() { "POSX6G", "POSX7G", "POSX8G", "POSX9G" });

                    break;
                case 6:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX8G", "POSX9G" }, 4);
                    //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword1S3");
                    pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX2G", "POSX8G", "POSX9G" });

                    break;
                case 7:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX9G", "POSX6G", "POSX5R", "POSX6R" }, 4);
                    //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword2S1");
                    pwGenerator.generateCube(new List<string>() { "POSX9G", "POSX6G", "POSX5R", "POSX6R" });

                    break;
                case 8:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX9G", "POSX1W", "POSX2W" }, 4);
                    //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword2S2");
                    pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX9G", "POSX1W", "POSX2W" });

                    break;
                case 9:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX4G", "POSX5G", "POSX6G", "POSX5B" }, 4);
                    //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword2S3");
                    pwGenerator.generateCube(new List<string>() { "POSX4G", "POSX5G", "POSX6G", "POSX5B" });

                    break;
                case 10:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX7G", "POSX8G", "POSX1R", "POSX2R" }, 4);
                    //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword3S1");
                    pwGenerator.generateCube(new List<string>() { "POSX7G", "POSX8G", "POSX1R", "POSX2R" });

                    break;
                case 11:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX1W", "POSX2W", "POSX3W" }, 4);
                    //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword3S2");
                    pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX1W", "POSX2W", "POSX3W" });

                    break;
                case 12:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX9G", "POSX8G", "POSX1B", "POSX2B" }, 4);
                    //password.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Passwords/2DPassword3S3");
                    pwGenerator.generateCube(new List<string>() { "POSX9G", "POSX8G", "POSX1B", "POSX2B" });

                    break;
                case 13:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX4G", "POSX1W", "POSX2W", "POSX1O" }, 4);
                    //password.GetComponent<RawImage>().texture = null;
                    pwGenerator.generateCube(new List<string>() { "POSX4G", "POSX1W", "POSX2W", "POSX1O" });
                    break;
                case 14:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX2G", "POSX5R", "POSX1W" }, 4);
                    pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX2G", "POSX5R", "POSX1W" });
                    //password.GetComponent<RawImage>().texture = null;

                    break;
                case 15:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX5R", "POSX5B", "POSX6B" }, 4);
                    pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX5R", "POSX5B", "POSX6B" });
                    //password.GetComponent<RawImage>().texture = null;

                    break;
                case 16:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX4G", "POSX4W", "POSX6W", "POSX5O" }, 4);
                    pwGenerator.generateCube(new List<string>() { "POSX4G", "POSX4W", "POSX6W", "POSX5O" });
                    //password.GetComponent<RawImage>().texture = null;

                    break;
                case 17:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX5R", "POSX1W", "POSX2W" }, 4);
                    pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX5R", "POSX1W", "POSX2W" });
                    //password.GetComponent<RawImage>().texture = null;

                    break;
                case 18:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX7G", "POSX7W", "POSX9W", "POSX7O" }, 4);
                    pwGenerator.generateCube(new List<string>() { "POSX7G", "POSX7W", "POSX9W", "POSX7O" });
                    //password.GetComponent<RawImage>().texture = null;

                    break;
                case 19:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX7G", "POSX5W", "POSX9B", "POSX7R" }, 4);
                    pwGenerator.generateCube(new List<string>() { "POSX7G", "POSX5W", "POSX9B", "POSX7R" });
                    //password.GetComponent<RawImage>().texture = null;

                    break;
                case 20:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX3G", "POSX3W", "POSX4O", "POSX5R" }, 4);
                    pwGenerator.generateCube(new List<string>() { "POSX3G", "POSX3W", "POSX4O", "POSX5R" });
                    // password.GetComponent<RawImage>().texture = null;

                    break;
                case 21:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX2G", "POSX3O", "POSX4B", "POSX5W" }, 4);
                    pwGenerator.generateCube(new List<string>() { "POSX2G", "POSX3O", "POSX4B", "POSX5W" });
                    // password.GetComponent<RawImage>().texture = null;

                    break;
                case 22:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX5G", "POSX2W", "POSX5B", "POSX9R" }, 4);
                    pwGenerator.generateCube(new List<string>() { "POSX5G", "POSX2W", "POSX5B", "POSX9R" });
                    //password.GetComponent<RawImage>().texture = null;

                    break;
                case 23:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX1W", "POSX5B", "POSX6O" }, 4);
                    pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX1W", "POSX5B", "POSX6O" });
                    // password.GetComponent<RawImage>().texture = null;

                    break;
                case 24:
                    passwordmodel = new PasswordModel(new List<System.String>() { "POSX1G", "POSX1W", "POSX1R", "POSX5B" }, 4);
                    pwGenerator.generateCube(new List<string>() { "POSX1G", "POSX1W", "POSX1R", "POSX5B" });
                    //password.GetComponent<RawImage>().texture = null;

                    break;
            }
        }
        /*if (trials <= 1)
        {
            password.GetComponent<RawImage>().texture = null;
            foreach (MaterialHolder materialobj in materialholderList)
            {              
                materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
            }

        }*/
        /*if (trials <= 1)
        {
            password.GetComponent<RawImage>().texture = null;
            foreach (MaterialHolder materialobj in materialholderList)
            {              
                materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
            }

        }*/
        started = false;
        stopped = false;
        fw = GameObject.Find("Storage").GetComponent<FileWriter>();
        mdfw = GameObject.Find("Storage").GetComponent<MetaDataWriter>();
        hand = GameObject.FindWithTag("Right").GetComponent<Hand>();
        //hand.Userinput.Clear();
        fullfw = GameObject.Find("Storage").GetComponent<FullFileWriter>();
        fullmdfw = GameObject.Find("Storage").GetComponent<FullMetaDataWriter>();
        fullfw.StartRecording();
        fullmdfw.StartRecording();
        inputtime = 0;
        fullinputtime = 0;
        //passwordmodel.reset();

    }
    // Update is called once per frame
    void Update()
    {
        if(Input.inputString == "x")
        {
            fullfw.StopRecording();
            fw.StopRecording();
        }
  
        if ((Input.inputString == "1" || m_GripAction.GetStateDown(m_Pose.inputSource)) && stopped)
            //Input.inputString == "2" ||
            //Input.inputString == "3" ||
            //Input.inputString == "4" ||
            //Input.inputString == "5" ||
            //Input.inputString == "6")
        {
            
            fullfw.StopRecording();
            fw.StopRecording();
            Debug.Log(trials + "versuche");
            if (trials == 0)
            {
                GameObject.Find("Controller (left)").SetActive(false);
                return;
            }
            trials--;

            //if (!stopped)
            // {
            //    mdfw.StopRecording();
            //     fullmdfw.StopRecording();
            //    stopped = true;
            // }
            //SceneManager.LoadScene("3DAuthenticationSchemeGazeTrigger");

            foreach (MaterialHolder materialobj in materialholderList)
            {
                //Debug.Log("test");
                // Debug.Log(materialobj.Material);
                // Debug.Log(materialobj.Gameobject);
                //Debug.Log(_pincorrect);
                materialobj.Gameobject.GetComponent<MeshRenderer>().material = materialobj.Material;
                //materialobj.Gameobject.GetComponent<MeshRenderer>().material = _pincorrect;
            }
            Init();
        }

        /*if (Input.inputString == "4")
        {
            // rotate cube
            // get side area (in this case left)
            GameObject.Find("Planes").transform.localRotation = Quaternion.Euler(0, 270, -90);
        }
        if (Input.inputString == "5")
        {
            // rotate cube
            // get back area
            GameObject.Find("Planes").transform.localRotation = Quaternion.Euler(-90, 270, -90);
        }
        if (Input.inputString == "6")
        {
            // rotate cube
            // bet bottom area
            GameObject.Find("Planes").transform.localRotation = Quaternion.Euler(180, 180, 0);
        } */
        // track time
        inputtime += Time.deltaTime;
        fullinputtime += Time.deltaTime;

        //if (m_GripAction.GetStateDown(m_Pose.inputSource))
        //{
        //   GameObject.Find("Planes").transform.Rotate(0, 180, 0);
        // }
        // Down
        if (m_GrabAction.GetStateDown(m_Pose.inputSource) && !started && !stopped)
        {
            if (current != null && GetNearestGameObject() != null)
            {
                fw.StartRecording();
                //Debug.Log("recording started");
                started = true;
                mdfw.StartRecording();
                inputtime = 0;
                passwordmodel.backupInput(GetNearestGameObject().name);
                mdfw.WriteToFileCsv(new DataModel(inputtime, GetNearestGameObject().name));
                fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, GetNearestGameObject().name));
                GetNearestGameObject().GetComponent<MeshRenderer>().material = tapped;
                Userinput.Add(GetNearestGameObject());
            }

            //call method Input();
            //Pickup();
        }


        //Up
        else if(m_GrabAction.GetStateDown(m_Pose.inputSource) && started && !stopped)
        {
            if (current != null && GetNearestGameObject() != null)
            {
                passwordmodel.backupInput(GetNearestGameObject().name);

            GetNearestGameObject().GetComponent<MeshRenderer>().material = tapped;
            Userinput.Add(GetNearestGameObject());
            mdfw.WriteToFileCsv(new DataModel(inputtime, GetNearestGameObject().name));
            fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, GetNearestGameObject().name));
            }
        }

        //workaround bug
        Debug.Log(Userinput.Count);
        if (Userinput.Count == 4 && !stopped)
        {

            stopped = true;
            List<String> userinputStrings = new List<String>();
            foreach (var userinput in Userinput)
            {
                userinputStrings.Add(userinput.name);
            }
            //passwordmodel.checkPassword(userinputStrings);
            passwordmodel.entryError(userinputStrings);
            mdfw.WriteToFileCsv(new DataModel(inputtime, "correct password: " + passwordmodel.password));
            fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, "correct password: " + passwordmodel.password));
            //mdfw.WriteToFileCsv(new DataModel(inputtime, passwordmodel.getUserInput()));
            //fullmdfw.WriteToFileCsv(new DataModel(fullinputtime, passwordmodel.getUserInput()));
            Userinput.Clear();

            Debug.Log("recording stopped");
           // fw.StopRecording();
            mdfw.StopRecording();
            //fullfw.StopRecording();
            fullmdfw.StopRecording();

            if (passwordmodel.getErrors() == 0)
            {
                foreach (GameObject pin in GameObject.FindGameObjectsWithTag("Interactable"))
                {
                    pin.GetComponent<MeshRenderer>().material = _pincorrect;

                }
            }
            else
            {
                foreach (GameObject pin in GameObject.FindGameObjectsWithTag("Interactable"))
                {
                    pin.GetComponent<MeshRenderer>().material = _pinwrong;

                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.gameObject.CompareTag("Interactable"))
            return;
        current.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandTappingOnly : MonoBehaviour
{
    public SteamVR_Action_Boolean m_GrabAction = null;
    private SteamVR_Behaviour_Pose m_Pose = null;
    public SteamVR_Action_Vibration hapticAction;
    private FixedJoint m_Joint = null;
    public Material tapped;
    private Interactable m_CurrentInteractable = null;
    public List<Interactable> m_ContactInteractables = new List<Interactable>();
    private List<GameObject> current = new List<GameObject>();
    public PasswordModel pm;
    public List<GameObject> Userinput = new List<GameObject>();


    // Start is called before the first frame update
    private void Start()
    {
        pm = TappingOnlyHandler.passwordmodel;

    }
    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();

    }

    // Update is called once per frame
    private void Update()
    {
       
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.gameObject.CompareTag("Interactable"))
            return;
        //m_ContactInteractables.Add(other.gameObject.GetComponent<Interactable>());
        if(other.gameObject != null)
        {
            Userinput.Add(other.gameObject);
            //Debug.Log(other.gameObject.name);
            pm.backupInput(other.gameObject.name);
            other.gameObject.GetComponent<MeshRenderer>().material = tapped;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;
        current.Remove(other.gameObject);
        // m_ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
    }

    private Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach (Interactable interactable in m_ContactInteractables)
        {

            distance = (interactable.transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = interactable;
            }
        }
        return nearest;
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

    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        print("hello");
        hapticAction.Execute(5, duration, frequency, amplitude, source);
        print("Pulse" + "" + source.ToString());
    }

}

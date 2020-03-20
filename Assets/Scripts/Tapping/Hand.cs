using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
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
        pm = TappingHandler.passwordmodel;

    }
    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();

    }

    // Update is called once per frame
    private void Update()
    {
        // Down
        if (m_GrabAction.GetStateDown(m_Pose.inputSource))
        {
            Pulse(0, 75, 1, SteamVR_Input_Sources.RightHand); // doesn't work atm
            Debug.Log("haptic feedback");

            print(m_Pose.inputSource + "Trigger down");
            if (current != null && GetNearestGameObject() != null)
            {
                pm.backupInput(GetNearestGameObject().name);

                GetNearestGameObject().GetComponent<MeshRenderer>().material = tapped;
                Userinput.Add(GetNearestGameObject());
            }

            //call method Input();
            //Pickup();
        }
        //Up
        if (m_GrabAction.GetStateUp(m_Pose.inputSource))
        {
            print(m_Pose.inputSource + "Trigger up");
            //Drop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.gameObject.CompareTag("Interactable"))
            return;
        //m_ContactInteractables.Add(other.gameObject.GetComponent<Interactable>());
        current.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;
        current.Remove(other.gameObject);
        // m_ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
    }

    /*public void Pickup()
    {

        // get nearest
        m_CurrentInteractable = GetNearestInteractable();

        Debug.Log(m_CurrentInteractable);
        // null check
        if (!m_CurrentInteractable)
            return;

        Debug.Log("picked up element");
        // already in other controllers hand 
        if (m_CurrentInteractable.m_activeHand)
            m_CurrentInteractable.m_activeHand.Drop();
        // position
        m_CurrentInteractable.transform.position = transform.position;
        // attach
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        m_Joint.connectedBody = targetBody;
        // set active hand
        m_CurrentInteractable.m_activeHand = this;

    }

    public void Drop()
    {

        // null check
        if (!m_CurrentInteractable)
            return;

        // apply velocity
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = m_Pose.GetVelocity();
        targetBody.angularVelocity = m_Pose.GetAngularVelocity();
        // detach
        m_Joint.connectedBody = null;
        // clear
        m_CurrentInteractable.m_activeHand = null;
        m_CurrentInteractable = null;

    }
    */
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    [Tooltip("This key will show or hide the view.")]
    private KeyCode _toggleKey = KeyCode.None;

    [SerializeField]
    [Tooltip("Activate or deactivate the view.")]
    private bool active;

    [SerializeField]
    private GameObject triggerobject;
    

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_toggleKey))
        {
            Debug.Log("pressed");
            if (active)
            {
                triggerobject.SetActive(false);
                active = false;
                Debug.Log("inactive");
            }
            else
            {
                Debug.Log("active");
                triggerobject.SetActive(true);
                active = true;


            }
        }
    }
}

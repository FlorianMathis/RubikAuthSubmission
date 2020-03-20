using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPassword : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    [Tooltip("This key will show or hide the positioning guide.")]
    private KeyCode _toggleKey = KeyCode.None;


    [SerializeField]
    [Tooltip("Activate or deactivate the positioning guide.")]
    private bool active;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(_toggleKey))
            {
                if (active)
                {
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    gameObject.GetComponent<MeshCollider>().enabled = false;

                    active = false;
 
                }
                else
                {
          
                    gameObject.GetComponent<MeshRenderer>().enabled = true;
                    gameObject.GetComponent<MeshCollider>().enabled = true;

                    active = true;


                }
            }
        
    }
}

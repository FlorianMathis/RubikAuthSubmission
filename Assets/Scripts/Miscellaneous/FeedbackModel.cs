using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class FeedbackModel : MonoBehaviour
{
    public GameObject Gameobject { get; set; }
    public float Time { get; set; }

    public Material Material { get; set; }

    public FeedbackModel(GameObject gameobject, Material material, float time)
    {
        Gameobject = gameobject;
        Material = material;
        Time = time;
    }


}

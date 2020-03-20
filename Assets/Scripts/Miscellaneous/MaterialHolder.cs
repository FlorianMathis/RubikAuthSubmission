using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public Material Material;
    public GameObject Gameobject;
    public MaterialHolder(Material material, GameObject gameobject)
    {
        Material = material;
        Gameobject = gameobject;

    }

}

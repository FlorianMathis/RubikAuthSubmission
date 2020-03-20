using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordGenerator : MonoBehaviour
{
    private List<string> pins;
    public Material _firstInput;
    public Material _secondInput;
    public Material _thirdInput;
    public Material _fourthInput;

    public void generateCube(List<string> password)
    {
        for (int pos = 0; pos < password.Count; pos++)
        {
            switch (pos)
            {
                case 0:
                    GameObject.Find(password[pos]).GetComponent<MeshRenderer>().material = _firstInput;
                    break;
                case 1:
                    GameObject.Find(password[pos]).GetComponent<MeshRenderer>().material = _secondInput;
                    break;
                case 2:
                    GameObject.Find(password[pos]).GetComponent<MeshRenderer>().material = _thirdInput;
                    break;
                case 3:
                    GameObject.Find(password[pos]).GetComponent<MeshRenderer>().material = _fourthInput;
                    break;
            }

        }
    }
}

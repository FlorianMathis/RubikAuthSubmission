using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    private string scene1;
    private string scene2;
    private string scene3;
    private List<string> scenes;
    // todo add all scenes for the study

    private void Start()
    {
        scenes = new List<string>();
        scenes.Add("SetupScene");
        scenes.Add("2DPasswordDisplay");
        scenes.Add("3DPasswordDisplay");
        //scenes.Add("3DAuthenticationSchemeGazeDwell");
        scenes.Add("3DAuthenticationSchemeGazeTrigger");
        scenes.Add("3DAuthenticationSchemeHeadPose");
        scenes.Add("3DAuthenticationSchemeTapping");
        scenes.Add("CreativityRoom");
        scenes.Add("MemorabilityRoom");
        scenes.Add("SecurityRoom");



    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(scenes[0]);
        }
        //if (Input.GetKeyDown(KeyCode.F2)) {
          //  SceneManager.LoadScene(scenes[1]);
       // }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene(scenes[2]);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene(scenes[3]);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene(scenes[4]);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            SceneManager.LoadScene(scenes[5]);
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            SceneManager.LoadScene(scenes[6]);
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            SceneManager.LoadScene(scenes[7]);
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            SceneManager.LoadScene(scenes[8]);
        }

        /*        switch (Input.inputString)
                {
                    // KeyCode.F1
                    case "282":
                        SceneManager.LoadScene(scenes[0]);
                        break;
                    case "283":
                        SceneManager.LoadScene(scenes[1]);
                        break;
                    case "284":
                        SceneManager.LoadScene(scenes[2]);
                        break;
                    case "285":
                        SceneManager.LoadScene(scenes[3]);
                        break;
                    case "286":
                        SceneManager.LoadScene(scenes[4]);
                        break;
                    case "287":
                        SceneManager.LoadScene(scenes[5]);
                        break;
                    case "288":
                        SceneManager.LoadScene(scenes[6]);
                        break;
                   // case "'":
                      //  SceneManager.LoadScene(scenes[7]);
                       // break;
                }
                */
    }
}

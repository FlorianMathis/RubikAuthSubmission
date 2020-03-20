using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalHandler 
{


    //security study
    private static string participant = "p15_expert"; // change this for each participant if not training
    // set to 99 for expert attack
    // and change participant string to px_expert
    public static int attackerscenario = 99;  // 1 pen and paper, 2 3D imitation or 3 smartphone

    public static int modality = 1;    // 0 eye gaze, 1 headpose, 2 tapping


    // 1 if training, 0 else
    public static int password = 1;
    public static bool training = true;
    // pref. input modality memorability 
 
    //participant number
    public static int participant_number = 23;
    // 0 if 2D, 1 if 3D
    public static int representation = 0;


    public static string dynamicpath = "UserData/" + participant;

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PasswordModel : MonoBehaviour
{
     List<string> Validpassword;
    public string password;
    int Length;
    int errors;
    Int32[] errorPos = new Int32[4];

    public List<string> Userpassword;

    // Start is called before the first frame update

    public PasswordModel(List<string> validpassword, int length)
    {
        Validpassword = validpassword;
        Length = length;
        Userpassword = new List<String>();
        foreach(string pin in validpassword)
        {
            password += pin+";";
        }
        password = password.Remove(password.Length - 1);
    }

    public void reset()
    {
        Userpassword.Clear();
       // Validpassword.Clear();
    }


    // make sure that every possible selectable field in the VR environment has an unique name/id
    public bool checkPassword(List<string> userpassword)
    {
        for (int i = 0; i <= Length - 1; i++)
        {
            if (userpassword[i] != Validpassword[i])
            {
                return false;
            }
        }
        return true;
    }

    public void entryError(List<string> userpassword)
    {
        for (int i = 0; i <= Length - 1; i++)
        {
            if (userpassword[i] != Validpassword[i])
            {
                errors += 1;
                errorPos[i] = 1;

            }
            else errorPos[i] = 0;
        }
        // write to file
        //Debug.Log("errors: " + sumerror);
    }

    public int getErrors()
    {
        return errors;
    }

    public Int32[] getErrorPos()
    {
        return errorPos;
    }

    public void backupInput(string selected)
    {
        Userpassword.Add(selected);
    }

    public String getUserInput()
    {
        if (Userpassword != null)
            return String.Join(", ", Userpassword.ToArray()); ;
        return "-";
    }
}

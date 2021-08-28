using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private string[] inputButtonName;
    private string[] inputStickName;
    private string[] inputTrigerName;
    private string[][] InputControllerName;
    void Start()
    {
        inputButtonName = new string[] { "Squarebutton", "×button", "Circlebutton", "Trianglebutton", "L1button", "R1button", "L2button", "R2button", "Lstickbutton", "Rstickbutton", "Optionsbutton" };
        inputStickName = new string[] { "Lstick_v", "Lstick_h", "Rstick_v", "Rstick_h" };
        inputTrigerName = new string[] { "L2trigger", "R2trigger" };
        //InputControllerName = new string[][] { inputButtonName, inputStickName, inputTrigerName };
    }

void Update()
    {
        
    }
    public string this[int index]
    {
        get { return inputButtonName[index]; }
        private set { inputButtonName[index] = value; }
    }
    //public string this[int index]
    //{
    //    get { return inputStickName[index]; }
    //    private set { inputStickName[index] = value; }
    //}
    //public string this[int index]
    //{
    //    get { return inputTrigerName[index]; }
    //    private set { inputTrigerName[index] = value; }
    //}
}

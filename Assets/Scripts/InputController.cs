using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public enum InputType
    {
        up,
        down,
        normal
    }
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
}

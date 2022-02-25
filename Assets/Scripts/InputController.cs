using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : SingletonMonoBehaviour<InputController>
{
    UserInput m_input;

    public enum InputType
    {
        up,
        down,
        normal
    }

    void OnEnable()
    {
        m_input = new UserInput();
        m_input.Enable();
    }

    void OnDisable()
    {
        m_input.Disable();
    }

    void OnDestroy()
    {
        m_input.Dispose();
    }

    public Vector2 Move()
    {
        return m_input.Player.Move.ReadValue<Vector2>();
    }

    public bool Jump()
    {
        return m_input.Player.Jump.WasPressedThisFrame();
    }

    public bool Run()
    {
        return m_input.Player.Run.IsPressed();
    }

    public Vector2 CameraMove()
    {
        return m_input.Camra.Move.ReadValue<Vector2>();
    }

    public bool Reset()
    {
        return m_input.Camra.Reset.WasPressedThisFrame();
    }

    public Vector2 CommandMove()
    {
        return m_input.Command.Move.ReadValue<Vector2>();
    }

    public bool Decide()
    {
        return m_input.Command.Decide.WasPressedThisFrame();
    }

    public bool Open()
    {
        return m_input.Command.Open.WasPressedThisFrame();
    }
}

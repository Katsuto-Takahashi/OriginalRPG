using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : SingletonMonoBehaviour<InputController>
{
    UserInput m_input;

    protected override void Awake()
    {
        base.Awake();
        m_input = new UserInput();
    }

    void OnEnable()
    {
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

    public float Zoom()
    {
        return m_input.Camra.Zoom.ReadValue<Vector2>().normalized.y;
    }

    public Vector2 CommandMove()
    {
        return m_input.Command.Move.ReadValue<Vector2>();
    }

    public bool Decide()
    {
        return m_input.Command.Decide.WasPressedThisFrame();
    }

    public bool Menu()
    {
        return m_input.Command.Menu.WasPressedThisFrame();
    }

    public bool Option()
    {
        return m_input.Option.Button.IsPressed();
    }
}

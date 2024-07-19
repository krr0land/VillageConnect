using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Singleton pattern

    private static InputManager _instance;

    public static bool Exists => _instance is not null;

    public static InputManager Instance
    {
        get
        {
            if (_instance is null)
                throw new Exception("Instance is NULL");
            return _instance;
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    #endregion

    private InputActions _inputActions;

    private void Awake()
    {
        if (_instance is not null)
            throw new Exception("Instance already exists");
        _instance = this;

        _inputActions = new InputActions();
        _inputActions.Enable();
    }

    public Vector2 GetNormalizedMoveVector()
    {
        return _inputActions.Player.Move.ReadValue<Vector2>().normalized;
    }

    public bool IsSprinting()
    {
        return _inputActions.Player.Sprint.IsPressed();
    }

    public float GetZoom()
    {
        return _inputActions.Player.Zoom.ReadValue<float>();
    }

    public bool IsBuilding()
    {
        return _inputActions.Player.Build.IsPressed();
    }

    public bool IsDestroying()
    {
        return _inputActions.Player.Destroy.IsPressed();
    }

    public Vector2 GetCursorPosition()
    {
        return _inputActions.Player.Cursor.ReadValue<Vector2>();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;

public class InputManager : Singleton<InputManager>
{
    public delegate void DevicesChanged(InputDevice device, InputDeviceChange changeType);
    public delegate void InputDataWithVector2(Vector2 value);
    public delegate void InputDataWithFloat(float value);
    public delegate void InputButtonPerformed();

    public event DevicesChanged DevicesChangedEvent;
    public event InputDataWithVector2 MoveEvent;
    public event InputDataWithVector2 LookAtEvent;
    public event InputDataWithFloat FireEvent;
    public event InputButtonPerformed UIEvent;

    public enum Devices
    {
        Keyboard = 0,
        Gamepad = 1,
    }
    public Devices DevicesType { get; private set; }

    Keyboard Keyboard => Keyboard.current;
    Mouse Mouse => Mouse.current;
    Gamepad Gamepad => Gamepad.current;

    Vector2 _leftStickValue = Vector2.zero;
    Vector2 _rightStickValue = Vector2.zero;
    float _rightTriggerValue = 0;

    void Start() {
        Init();
    }

    void Update() {
        switch (DevicesType)
        {
            case Devices.Keyboard:
                KeyboardInput();
                break;
            case Devices.Gamepad:
                GamepadInput();
                break;
            default:
                Debug.LogError("Trying to set device but is not recognized by Unity InputSystem!");
                break;
        }

        if (GameManager.I.isGameOver) return;

        MoveEvent?.Invoke(_leftStickValue);
        LookAtEvent?.Invoke(_rightStickValue);
        FireEvent?.Invoke(_rightTriggerValue);

    }

    void OnDestroy() {

    }

    void Init()
    {
        RefreshInputType();

        InputSystem.onDeviceChange += OnDevicesChanged;

        DontDestroyOnLoad(this);
    }

    void RefreshInputType() {
        if (Mouse != null && Keyboard != null)
        {
            DevicesType = Devices.Keyboard;
        }
        if (Gamepad != null)
        {
            DevicesType = Devices.Gamepad;
        }
    }

    void OnDevicesChanged(InputDevice device, InputDeviceChange changeType)
    {
        switch (changeType)
        {
            case InputDeviceChange.Added:
            case InputDeviceChange.Removed:
            case InputDeviceChange.Disconnected:
            case InputDeviceChange.Reconnected:
                RefreshInputType();
                DevicesChangedEvent?.Invoke(device, changeType);
                break;
        }

    }

    void KeyboardInput() {
        Vector2 inputValue = Vector2.zero;
        if (Keyboard.wKey.isPressed)
        {
            inputValue.y = 1;
        }
        else if (Keyboard.sKey.isPressed)
        {
            inputValue.y = -1;
        }
        if (Keyboard.aKey.isPressed)
        {
            inputValue.x = -1;
        }
        else if (Keyboard.dKey.isPressed)
        {
            inputValue.x = 1;
        }
        _leftStickValue = inputValue;
        _rightStickValue = Mouse.position.ReadValue();
        _rightTriggerValue = Mouse.leftButton.isPressed ? 1 : 0;
    }

    void GamepadInput() {
        _leftStickValue = Gamepad.leftStick.ReadValue();
        _rightStickValue = Gamepad.rightStick.ReadValue();
        _rightTriggerValue = Gamepad.rightTrigger.ReadValue();
    }

}


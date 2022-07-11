using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
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

    Vector2 _leftStcikValue = Vector2.zero;
    Vector2 _rightStcikValue = Vector2.zero;
    float _rightTriggerValue = 0;

    void Start() {
        RefreshInputType();
        InputSystem.onDeviceChange += OnDevicesChanged;

        //DontDestroyOnLoad(this);
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

        MoveEvent?.Invoke(_leftStcikValue);
        LookAtEvent?.Invoke(_rightStcikValue);
        FireEvent?.Invoke(_rightTriggerValue);

    }

    void OnDestroy() {

    }

    void OnDevicesChanged(InputDevice device, InputDeviceChange changeType) {
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
        _leftStcikValue = inputValue;
        _rightStcikValue = Mouse.position.ReadValue();
        _rightTriggerValue = Mouse.leftButton.isPressed ? 1 : 0;
    }

    void GamepadInput() {
        _leftStcikValue = Gamepad.leftStick.ReadValue();
        _rightStcikValue = Gamepad.rightStick.ReadValue();
        _rightTriggerValue = Gamepad.rightTrigger.ReadValue();
    }

}


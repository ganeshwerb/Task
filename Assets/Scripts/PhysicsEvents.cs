using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsEvents : MonoBehaviour
{
    [SerializeField] float _threshold;
    [SerializeField] float _deadZone;

    bool _isPressed;
    Vector3 _startPos;
    ConfigurableJoint _joint;

    public UnityEvent onPressed, onReleased;

    private void Start()
    {
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {
        if (!_isPressed && GetValue() + _threshold >= 1)
            ButtonPressed();
        if(_isPressed && GetValue() - _threshold <=0)
            ButtonReleased();
    }

    float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;
        if(Mathf.Abs(value) < _deadZone)
        {
            value = 0;
        }
        return Mathf.Clamp(value, -1f, 1f);

    }
    void ButtonPressed()
    {
        _isPressed = true;
        onPressed.Invoke();
        Debug.Log("Button Pressed");
    }

    void ButtonReleased()
    {
        _isPressed = false;
        onReleased.Invoke();
        Debug.Log("Button Released");
    }
}

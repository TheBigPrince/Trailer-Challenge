using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{

    private float currentAngle = 0f;


    /// <summary>
    /// Rotates the wheel based on a vehicles velocity.
    /// </summary>
    /// <param name="_vehicle">The transform of the vehicle that is moving.</param>
    /// <param name="_velocity">The velocity of the vehicle that is moving.</param>
    /// <param name="_speedMultiplier">A number that should be between 0 and 1 that controls the speed of the wheels rotation.</param>
    public void Rotate(Transform _vehicle, Vector3 _velocity, float _speedMultiplier = 1f)
    {
        _speedMultiplier = Mathf.Clamp(_speedMultiplier, 0f, 1f);
        float dot = Vector3.Dot(_vehicle.forward, _vehicle.forward + _velocity) - 1;
        currentAngle = (currentAngle + dot * _speedMultiplier) % 360f;
        transform.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);
    }


}

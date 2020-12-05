using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MouseSettings : ScriptableObject {
    public float pauseSpeed = 0.25f;
    public float minSpeed = 2;
    public float maxSpeed = 5;
    public float maxSteerForce = 3;
    public LayerMask colliderMask;
    public float avoidCollisionWeight = 10;
    public float avoidCollisionDistance = 5;
    public float targetWeight = 1;
}
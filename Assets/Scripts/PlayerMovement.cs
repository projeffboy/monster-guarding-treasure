﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// From Brackey's FPS tutorial

public class PlayerMovement : MonoBehaviour {
    public float speed = 12f;

    void Update() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        GetComponent<CharacterController>().Move(move * speed * Time.deltaTime);
    }

    void LateUpdate() {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, 1.25f, pos.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// From Brackey's FPS tutorial

public class PlayerLook : MonoBehaviour {
    public float mouseSensitivity = 100f;
    public Transform playerContainer;

    private float xRotation = 0;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        float mult = mouseSensitivity * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * mult;
        playerContainer.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mult;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour {
    public Text shieldValueText;
    public Text shieldActivityText;

    private float shieldValue = 10;
    private bool shieldIsActive = false;
    private bool shieldIsOut = false;

    void Update() {   
        if (shieldIsOut) {
            return;
        }

        if (Input.GetButtonDown("Jump")) {
            shieldIsActive = !shieldIsActive;

            shieldActivityText.text = "Shield is "
                + (shieldIsActive ? "" : "not ")  + "up.";
        }

        if (shieldIsActive) {
            shieldValue -= Time.deltaTime;

            shieldValueText.text = "Shield value: "
                + System.Math.Round(shieldValue, 2);
        }

        if (shieldValue <= 0) {
            shieldIsOut = true;

            shieldActivityText.text = "Shield is out of ammo.";
            shieldActivityText.color = Color.red;
            shieldValueText.text = "Shield value: 0";
        }
    }
}

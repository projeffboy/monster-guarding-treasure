﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseCondition : MonoBehaviour {
    public int Lives = 2;
    public Shield ShieldScript;
    public Text LivesText;
    public Text GameOverText;

    public void DecrementLife() {
        if (!ShieldScript.IsShieldActive()) {
            Lives--;

            LivesText.text = "Lives: " + Lives;

            if (Lives <= 0) {
                Time.timeScale = 0;

                GameOverText.text = "You lose";
                GameOverText.color = Color.red;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour {
    public GameObject Treasure;
    public GameObject BackEntrance;
    public GameObject FrontEntrance;
    public Material FinishEntranceMaterial;
    public Text GameOverText;

    private bool reachedTreasure = false;

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (!reachedTreasure) {
            if (hit.gameObject == Treasure) {
                reachedTreasure = true;

                Destroy(hit.gameObject);

                FrontEntrance.GetComponent<Renderer>().material
                    = FinishEntranceMaterial;
                BackEntrance.GetComponent<Renderer>().material
                    = FinishEntranceMaterial;
            }
        } else {
            if (hit.gameObject == BackEntrance) {
                Time.timeScale = 0;

                GameOverText.text = "You win";
                GameOverText.color = Color.green;
            }
        }
    }
}

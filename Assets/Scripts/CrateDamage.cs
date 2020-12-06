using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateDamage : MonoBehaviour {
    private bool lethal = false;
    private Vector3 originalPosition;

    void Start() {
        originalPosition = transform.position;

        StartCoroutine(CheckLethal());
    }

    private IEnumerator CheckLethal() {
        while (transform.position == originalPosition) {
            yield return null;
        }
        
        lethal = true;
    }

    void OnCollisionEnter(Collision collision) {
        if (lethal) {
            GameObject otherObj = collision.gameObject;

            if (
                otherObj.CompareTag("Floor")
                || otherObj.CompareTag("Player")
            ) {
                StartCoroutine(Disappear());
            }

            if (otherObj.CompareTag("Player")) {
                Debug.Log("You got hit");

                //LoseCondition script = otherObj.GetComponent<LoseCondition>();
                
                //script.DecrementLife();
            }
        }
    }

    private IEnumerator Disappear() {
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}

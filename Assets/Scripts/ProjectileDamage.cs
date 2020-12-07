using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour {
    public bool IsCrate = true;

    private bool lethal = false;
    private bool hitPlayer = false;
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

            if (otherObj.CompareTag("Floor")|| otherObj.CompareTag("Player")) {
                StartCoroutine(Aftermath());
            }

            if (otherObj.CompareTag("Player") && !hitPlayer) {
                LoseCondition script = otherObj.GetComponent<LoseCondition>();
                
                script.DecrementLife();

                hitPlayer = true;
            }
        }
    }

    private IEnumerator Aftermath() {
        yield return new WaitForSeconds(0.5f);

        lethal = false;
        hitPlayer = false;
        if (IsCrate) {
            gameObject.SetActive(false);
        }
    }
}

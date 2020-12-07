using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour {
    public bool IsCrate = true;

    private bool lethal = false;
    private bool hitPlayer = false;

    public bool IsLethal() {
        return lethal;
    }

    void Start() {
        StartCoroutine(CheckLethal());
    }

    private IEnumerator CheckLethal() {
        while (transform.position.y < 2) {
            yield return null;
        }
        
        lethal = true;
    }

    void OnCollisionEnter(Collision collision) {
        if (lethal) {
            GameObject otherObj = collision.gameObject;

            if (otherObj.CompareTag("Floor")) {
                // Either disappear or make it non-lethal
                StartCoroutine(Aftermath());
            }

            if (otherObj.CompareTag("Player") && !hitPlayer) {
                LoseCondition script = otherObj.GetComponent<LoseCondition>();
                
                script.DecrementLife();

                hitPlayer = true;
                
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator Aftermath() {
        yield return new WaitForSeconds(0.5f);

        lethal = false;
        hitPlayer = false;
        if (IsCrate) {
            gameObject.SetActive(false);
        } else {
            StartCoroutine(CheckLethal());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLoneObstacles : MonoBehaviour {
    public GameObject Crate;
    public GameObject Rock;
    public LayerMask obstacleLayer;
    public GameObject Monster;

    void Start() {
        for (int i = 0; i < 10; i++) {
            float posX = Random.Range(-9f, 9f);
            float posZ = Random.Range(-2f, -9f);
            Vector3 pos = new Vector3(posX, 1, posZ);

            Collider[] colliderInOverlapBox = new Collider[1];
            int numCollidersFound = Physics.OverlapBoxNonAlloc(
                pos,
                Vector3.one,
                colliderInOverlapBox,
                Quaternion.identity,
                obstacleLayer
            );

            // Debug.Log("number of colliders found " + numCollidersFound);

            if (numCollidersFound == 0) {
                // Debug.Log("spawned obstacle");
                GameObject obstacle = Instantiate(Obstacle(), transform);
                obstacle.transform.localPosition = pos;
                Physics.SyncTransforms();
            } /*else {
                Debug.Log(
                    "name of collider 0 found" + colliderInOverlapBox[0].name
                );
            }*/
        }

        Monster.SetActive(true);
    }

    private GameObject Obstacle() {
        return Random.value < 0.5f ? Crate : Rock;
    }
}

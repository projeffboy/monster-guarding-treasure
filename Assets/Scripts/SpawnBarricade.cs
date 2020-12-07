using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBarricade : MonoBehaviour {
    public GameObject Crate;
    public GameObject Rock;
    public GameObject LoneObstacles;

    void Start() {
        float posX = 0;
        float posZ = 0;

        // Randomly but diagonally spawns a barricade
        for (int i = 0; i < 10; i++) {
            int outcome = Random.Range(0, 3);
            if ((5 <= i && i <= 7) || outcome == 0 || outcome == 2) { // Taking care not to block player entrance
                posX += 1.5f;
            }
            if (i < 4 || outcome == 1 || outcome == 2) { // Taking care not to encroach monster territory
                posZ += -1.5f;
            }

            Vector3 pos = new Vector3(posX, 1, posZ);

            GameObject newObstacle = Instantiate(Obstacle(), transform, false);
            newObstacle.transform.localPosition = pos;
        }

        Instantiate(Obstacle(), transform, false);

        LoneObstacles.SetActive(true);
    }

    private GameObject Obstacle() {
        return Random.value < 0.5f ? Crate : Rock;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseDirections {

    const int numDirections = 32;
    public static readonly Vector3[] directions;

    static MouseDirections() {
        directions = new Vector3[numDirections];

        float rad = Mathf.PI / 2;
        float radRange = 3 * Mathf.PI / 4;
        for (int i = 0; i < numDirections; i++) {
            rad = Mathf.PI - rad;
            if (i % 2 == 0) {
                rad += radRange / (numDirections / 2);
            }

            float x = Mathf.Cos(rad);
            float z = Mathf.Sin(rad);
            // Debug.Log("(" + x + ", " + z + ")");
            directions[i] = new Vector3(x, 0, z);
        }
    }
}
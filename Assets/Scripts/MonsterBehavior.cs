using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBehavior : MonoBehaviour {
    public Text EnemyPlan;
    public MeshRenderer Mesh;
    public CapsuleCollider Collider;
    public Transform Player;
    public LayerMask PlayerMask;
    public LayerMask CrateMask;
    public LayerMask RockMask;
    public float JitterDamp = 0.1f;
    public float RadiusPlayerDetection = 5;
    public float RadiusObstacleDetection = 20;
    public float ThrowingForce = 50f;
    public float RotationSpeed = 60f;

    // World State
    private float timeShakedJustNow = 0;
    private bool isRed = false;
    private float cooldownJustNow = 0;
    private GameObject obstacleToPickUp = null;
    private float degreesRotated = 0;

    private Vector3 originalPosition;
    private Color purple;

    private string plan = "";

    void Start() {
        originalPosition = transform.position;
        purple = Mesh.material.color;
    }

    void Update() {
        if (plan == "") {
            Collider[] colliders = Physics.OverlapSphere(
                transform.position, RadiusPlayerDetection, PlayerMask
            );
            
            if (colliders.Length > 0) {
                if (!isRed) {
                    plan = "AB";
                }

                plan += "Z";
                plan += "CDE";
                plan += "ZZ";

                UpdateEnemyPlanText();
            } else {
                plan += "NZOPZ";
            }
        } else {
            bool complete = false;
            switch (plan[0]) {
                // Non-idle tasks
                case 'A':
                    complete = Shake();
                    break;
                case 'B':
                    complete = TurnRed();
                    break;
                case 'C':
                    complete = GoToRandomCrate();
                    break;
                case 'D':
                    complete = PickUp0bstacle();
                    break;
                case 'E':
                    complete = ThrowObstacle();
                    break;
                // Idle tasks
                case 'N':
                    complete = TurnPurple();
                    break;
                case 'O':
                    complete = MoveSideways();
                    break;
                case 'P':
                    complete = DoubleBarrelRoll();
                    break;
                // Other
                case 'Z':
                    complete = Cooldown();
                    break;
            }

            if (complete) {
                plan = plan.Substring(1);

                UpdateEnemyPlanText();
            }
        }
    }

    private void UpdateEnemyPlanText() {
        EnemyPlan.text = "Enemy Plan (Next 5 Tasks):\n";
        foreach (char task in plan) {
            switch (task) {
                // Non-idle tasks
                case 'A':
                    EnemyPlan.text += "Shake in Fury";
                    break;
                case 'B':
                    EnemyPlan.text += "Turn Red from Anger";
                    break;
                case 'C':
                    EnemyPlan.text += "Go to Random Crate";
                    break;
                case 'D':
                    EnemyPlan.text += "Pick Up Obstacle";
                    break;
                case 'E':
                    EnemyPlan.text += "Throw Obstacle";
                    break;
                // Idle tasks
                case 'N':
                    EnemyPlan.text += "Turn Purple";
                    break;
                case 'O':
                    EnemyPlan.text += "Move Sideways";
                    break;
                case 'P':
                    EnemyPlan.text += "Barrel Roll";
                    break;
                // Other
                case 'Z':
                    EnemyPlan.text += "Cooldown";
                    break;
            }
            EnemyPlan.text += "\n";
        }
    }

    private bool Cooldown() { // Z
        cooldownJustNow += Time.deltaTime;

        if (cooldownJustNow >= 1) {
            cooldownJustNow = 0;
            
            return true;
        } else {
            return false;
        }
    }

    private bool Shake() { // A
        float x = Random.value;
        float z = Random.value;
        Vector3 jitter = new Vector3(x, 0, z) * JitterDamp;
        transform.position = originalPosition + jitter;

        timeShakedJustNow += Time.deltaTime;

        if (timeShakedJustNow >= 2) {
            timeShakedJustNow = 0;

            return true;
        } else {
            return false;
        }
    }

    private bool TurnRed() { // B
        return TurnRedOverPurple(true);
    }

    private bool GoToRandomCrate() { // C
        Collider[] colliders = Physics.OverlapSphere(
            transform.position, RadiusObstacleDetection, CrateMask
        );

        if (colliders.Length > 0) {
            // Debug.Log(colliders[0]);
            Vector3 position = colliders[0].transform.position;
            transform.position = new Vector3(
                position.x, originalPosition.y, position.z
            );

            obstacleToPickUp = colliders[0].gameObject;
        }

        return true;
    }

    private bool PickUp0bstacle() { // D
        obstacleToPickUp.transform.Translate(
            new Vector3(0, Collider.height + 0.5f, 0)
        );

        return true;
    }

    private bool ThrowObstacle() { // E
        Vector3 throwDirection = Player.position - transform.position;
        throwDirection = new Vector3(throwDirection.x, 0, throwDirection.z);

        obstacleToPickUp.GetComponent<Rigidbody>().AddForce(
            throwDirection * ThrowingForce
        );

        obstacleToPickUp = null;

        return true;
    }

    private bool TurnPurple() { // N
        return TurnRedOverPurple(false);
    }

    private bool MoveSideways() { // O
        float x = Mathf.Sin(Time.time);
        float z = Mathf.Sin(Time.time);
        transform.Translate(new Vector3(x, 0, z) * Time.deltaTime);

        timeShakedJustNow += Time.deltaTime;

        return true;
    }

    private bool DoubleBarrelRoll() { // P
        float rotatedAmount = RotationSpeed * Time.deltaTime;

        transform.Rotate(0, 0, rotatedAmount);

        degreesRotated += rotatedAmount;

        if (degreesRotated >= 360) {
            degreesRotated = 0;
            transform.rotation = Quaternion.identity;
            
            return true;
        } else {
            return false;
        }
    }

    // Helper Tasks
    private bool TurnRedOverPurple(bool turnRed) {
        Mesh.material.color = turnRed ? Color.red : purple;
        isRed = turnRed;

        return true;
    }
}

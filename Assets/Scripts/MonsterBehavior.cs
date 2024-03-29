﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;

public class MonsterBehavior : MonoBehaviour {
    public Text EnemyPlan;
    public MeshRenderer Mesh;
    public CapsuleCollider Collider;
    public Transform Player;
    public LayerMask PlayerMask;
    public LayerMask CrateMask;
    public LayerMask RockMask;
    public NavMeshAgent Agent;
    public float JitterDamp = 0.1f;
    public float RadiusPlayerDetection = 5;
    public float RadiusObstacleDetection = 20;
    public float ThrowingForce = 50;
    public float RotationSpeed = 60;
    public float moveInCircleSpeed = 2;
    public float PathingTimeout = 5;
    public AudioSource Sound;

    // Variables that have to do with when to stop tasks
    // (Nothing to do with world state)
    private float timeShakedJustNow = 0;
    private bool isRed = false;
    private float cooldownJustNow = 0;
    private GameObject obstacleToPickUp = null;
    private float degreesRotated = 0;
    private bool isMoving = false;
    private float moveInCircleAngle = 0;
    private float pathingTimeout;

    private Vector3 originalPosition;
    private Vector3 shakePosition;
    private Color purple;

    // HTN Related
    private SpecificHtnTree htnTree;
    private SpecificHtnPlanner planner;
    private string currentTask;
    private bool newTask = true;

    void Start() {
        pathingTimeout = PathingTimeout;
        originalPosition = new Vector3(0, 1.583f, 7.5f); // transform.position;
        purple = Mesh.material.color;

        htnTree = new SpecificHtnTree();
        // htnTree.Print();

        planner = new SpecificHtnPlanner();
        GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate");
        planner.state.Edit(false, false, false, crates.Length); // initialize world state
    }

    private void CreatePlanHelper(bool playerMustBeInRange) {
        // Refactoring into this function means calling overlapSphere a bit less
        if (playerMustBeInRange) {
            planner.state.SetIsPlayerInRange(true);
        } else {
            // SENSOR
            Collider[] colliders = Physics.OverlapSphere(
                    transform.position, RadiusPlayerDetection, PlayerMask
                );

            planner.state.SetIsPlayerInRange(colliders.Length > 0);
        }

        planner.createPlan(htnTree.GetRoot());
    }

    void Update() {
        if (planner.plan.Count == 0) {
            CreatePlanHelper(false);
        } else {
            if (newTask) {
                UpdateEnemyPlanText();

                if (!planner.state.isPlayerInRange) {

                    Collider[] colliders = Physics.OverlapSphere(
                        transform.position, RadiusPlayerDetection, PlayerMask
                    );

                    if (colliders.Length > 0) {
                        // In other words, world state and actual world state are now out of sync in terms of isPlayerInRange, so we replan since it's not valid anymore
                        planner.plan.Clear();
                        newTask = true;
                        CreatePlanHelper(true);

                        return;
                    }
                }

                currentTask = planner.plan.Dequeue();
            }
            
            bool complete = false;

            switch (currentTask) {
                case "Cooldown":
                    complete = Cooldown();
                    break;
                case "Growl":
                    complete = Growl();
                    break;
                case "Shake":
                    complete = Shake();
                    break;
                case "Turn Red":
                    complete = TurnRed();
                    break;
                case "Stay Red":
                    complete = true;
                    break;
                case "Go to Nearest Crate":
                    complete = GoToNearestCrate();
                    break;
                case "Go to Nearest Rock":
                    complete = GoToNearestRock();
                    break;
                case "Pick It Up":
                    complete = PickItUp();
                    break;
                case "Throw Crate":
                case "Throw Rock":
                    complete = ThrowObstacle();
                    break;
                case "Go to Spawn Location":
                    complete = GoToSpawnLocation();
                    break;
                case "Turn Purple":
                    complete = TurnPurple();
                    break;
                case "Move Left":
                    complete = MoveLeft();
                    break;
                case "Move Right":
                    complete = MoveRight();
                    break;
                case "Double Barrel Roll":
                    complete = DoubleBarrelRoll();
                    break;
                case "Move in Circle":
                    complete = MoveInCircle();
                    break;
                default:
                    Debug.Log(
                        "The task " + currentTask +  " is not registered."
                    );
                    complete = true;
                    break;
            }

            newTask = complete;
        }
    }

    private void UpdateEnemyPlanText() {
        EnemyPlan.text = "Enemy Plan:\n";

        var planCopy = new Queue<string>(planner.plan);

        while(planCopy.Count > 0) {
            string taskMsg = planCopy.Dequeue();

            EnemyPlan.text += taskMsg + "\n";
        }
    }

    private bool Cooldown() { // Z
        cooldownJustNow += Time.deltaTime;

        if (cooldownJustNow >= 1.5f) {
            cooldownJustNow = 0;
            
            return true;
        } else {
            return false;
        }
    }

    private bool Growl() {
        Sound.Play();

        return true;
    }

    private bool Shake() { // A
        if (timeShakedJustNow == 0) {
            shakePosition = transform.position;
        } else if (timeShakedJustNow < 1.5f) {
            float x = Random.value;
            float z = Random.value;
            Vector3 jitter = new Vector3(x, 0, z) * JitterDamp;

            transform.position = shakePosition + jitter;
        } else {
            timeShakedJustNow = 0;

            return true;
        }

        timeShakedJustNow += Time.deltaTime;

        return false;
    }

    private bool TurnRed() { // B
        return TurnRedOverPurple(true);
    }

    private bool GoToNearestCrate() {
        return GoToNearestObstacle(CrateMask);
    }

    private bool GoToNearestRock() {
        return GoToNearestObstacle(RockMask);
    }

    private bool GoToNearestObstacle(LayerMask mask) { // C
        pathingTimeout -= Time.deltaTime;
        if (pathingTimeout <= 0) {
            pathingTimeout = PathingTimeout;
            isMoving = false;

            return true;
        }

        if (!isMoving) {
            Collider[] colliders = Physics.OverlapSphere(
                transform.position, RadiusObstacleDetection, mask
            );

            if (colliders.Length > 0) {
                // would be faster if i didn't sort them all
                Collider[] orderedColliders = colliders.OrderBy(
                    c => Vector3.Distance(
                        transform.position, c.transform.position
                    )
                ).ToArray();

                Vector3 position = colliders[0].transform.position;
                position -= (position - transform.position) * 0.1f; // otherwise monster may take the long route
                Agent.SetDestination(new Vector3(
                    position.x, originalPosition.y, position.z
                ));

                obstacleToPickUp = colliders[0].gameObject;
                
                isMoving = true;
            } else {
                return true;
            }

        } else if (Agent.remainingDistance <= 0.1f) {
            isMoving = false;

            return true;
        }

        // Debug.Log(Agent.remainingDistance);

        return false;
    }

    private bool PickItUp() { // D
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

    private bool GoToSpawnLocation() { // M
        return Move(originalPosition);
    }

    private bool TurnPurple() { // N
        return TurnRedOverPurple(false);
    }

    private bool MoveLeft() { // O
        return Move(transform.position - new Vector3(2, 0, 0));
    }

    private bool MoveRight() { // P
        return Move(transform.position + new Vector3(2, 0, 0));
    }

    private bool DoubleBarrelRoll() { // Q
        Agent.enabled = false;

        float rotatedAmount = RotationSpeed * Time.deltaTime;

        transform.Rotate(rotatedAmount, 0, 0, Space.World);

        degreesRotated += rotatedAmount;

        if (degreesRotated >= 360) {
            degreesRotated = 0;
            transform.rotation = Quaternion.identity;
            
            Agent.enabled = true;

            return true;
        } else {
            return false;
        }
    }

    private bool MoveInCircle() {
        moveInCircleAngle += Time.deltaTime * moveInCircleSpeed;

        Vector3 center = originalPosition - new Vector3(0, 0, 1);

        var offset = new Vector3(
            Mathf.Sin(moveInCircleAngle),
            0,
            Mathf.Cos(moveInCircleAngle)
        );

        transform.position = center + offset;

        if (moveInCircleAngle >= 6.28f) {
            moveInCircleAngle = 0;

            return true;
        } else {
            return false;
        }
    }

    // Helper Tasks
    private bool Move(Vector3 newSpot) {
        pathingTimeout -= Time.deltaTime;
        if (pathingTimeout <= 0) {
            pathingTimeout = PathingTimeout;
            isMoving = false;

            return true;
        }

        if (!isMoving) {
            Agent.SetDestination(newSpot);

            isMoving = true;
        } else if (Agent.remainingDistance <= 0.1f) {
            isMoving = false;

            return true;
        }

        return false;
    }

    // so turn red or turn purple
    private bool TurnRedOverPurple(bool turnRed) {
        Mesh.material.color = turnRed ? Color.red : purple;
        isRed = turnRed;

        return true;
    }
}

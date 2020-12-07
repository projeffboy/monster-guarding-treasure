// THIS CODE IS BASED ON THIS VIDEO ON BOIDS https://www.youtube.com/watch?v=bqtqltqcQhw
// MY VERSION IS IN 2D, HAS NO FLOCKING BEHAVIOR (EACH BOID HAS ITS OWN COLLIDER), AND HAS NO BOID MANAGER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour {
    public MouseSettings settings;
    public int id;

    // State
    private Vector3 velocity;
    private bool changeDirection = false;
    // private Vector3 target;

    // Cached
    private float boundsRadius;
    private Transform cachedTransform;

    void Start() {
        boundsRadius = GetComponent<CapsuleCollider>().radius;
        cachedTransform = transform;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = cachedTransform.forward * startSpeed;

        /*
        float x = Random.Range(-19f, 19f);
        float z = Random.Range(-19f, 19f);
        target = new Vector3(x, 0.2f, z);
        */
    }

    void Update() {
        Vector3 acceleration = Vector3.zero;

        /*
        if (target != null) {
            Vector3 offsetToTarget = target - position;
            acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
        }
        */

        if (IsHeadingForCollision()) {
            Vector3 avoidCollisionDir = ObstacleRays();
            Vector3 avoidCollisionForce = SteerTowards(avoidCollisionDir)
                * settings.avoidCollisionWeight;
            acceleration += avoidCollisionForce;
        }

        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;

        if (System.Math.Truncate(Time.time) % 21 == (id * 3)) {
            speed = settings.pauseSpeed;
            
            changeDirection = true;
        } else {
            if (changeDirection == true) {
                dir = new Vector3(Random.value, 0, Random.value);

                changeDirection = false;
            }

            speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
        }

        velocity = dir * speed;

        cachedTransform.position += velocity * Time.deltaTime;
        cachedTransform.forward = dir;
    }

    private bool IsHeadingForCollision() {
        Ray ray = new Ray(cachedTransform.position, cachedTransform.forward);

        return SphereCastHelper(ray);
    }

    private Vector3 ObstacleRays() {
        Vector3[] rayDirections = MouseDirections.directions;

        for (int i = 0; i < rayDirections.Length; i++) {
            Vector3 dir = cachedTransform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(cachedTransform.position, dir);
            
            if (!SphereCastHelper(ray)) {
                return dir;
            }
        }

        return cachedTransform.forward;
    }

    private bool SphereCastHelper(Ray ray) {
        return Physics.SphereCast(
            ray,
            boundsRadius,
            settings.avoidCollisionDistance,
            settings.colliderMask
        );
    }

    private Vector3 SteerTowards(Vector3 vector) {
        Vector3 v = vector.normalized * settings.maxSpeed - velocity;

        return Vector3.ClampMagnitude(v, settings.maxSteerForce);
    }

    /*
    void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.gameObject);
    }
    */
}
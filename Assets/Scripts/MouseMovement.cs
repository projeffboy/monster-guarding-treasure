// THIS CODE IS BASED ON THIS VIDEO ON BOIDS https://www.youtube.com/watch?v=bqtqltqcQhw
// MY VERSION IS IN 2D, HAS NO FLOCKING BEHAVIOR (EACH BOID HAS ITS OWN COLLIDER), HAS NO TARGET, AND HAS NO BOID MANAGER
// ALSO CREDITS TO HIM FOR THE CONE MODEL

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour {
    public MouseSettings settings;
    public int id;

    // State
    private Vector3 velocity;
    private bool changeDirection = false;

    // Cached
    private float boundsRadius;
    private Transform transformCache;

    void Start() {
        boundsRadius = GetComponent<CapsuleCollider>().radius;
        transformCache = transform;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = transformCache.forward * startSpeed;
    }

    // Mice can be destroyed
    void OnCollisionEnter(Collision collision) {
        GameObject otherObj = collision.gameObject;

        if (otherObj.CompareTag("Monster")) {
            Debug.Log("Mouse was destroyed by monster");
            
            gameObject.SetActive(false);
        } else if (
            otherObj.CompareTag("Crate")
            || otherObj.CompareTag("Rock")
        ) {
            if (otherObj.GetComponent<ProjectileDamage>().IsLethal()) {
                Debug.Log("Mouse was destroyed by projectile");
                gameObject.SetActive(false);
            }
        }
    }

    void Update() {
        Vector3 acceleration = new Vector3(0, 0, 0);

        if (IsHeadingForCollision()) {
            Vector3 avoidCollisionDir = ObstacleRays();
            Vector3 avoidCollisionForce = SteerTowards(avoidCollisionDir)
                * settings.avoidCollisionWeight;
            
            acceleration += avoidCollisionForce;
        }
        velocity += acceleration * Time.deltaTime;

        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        // For the brief pauses
        if (System.Math.Truncate(Time.time) % 21 == (id * 3)) {
            speed = settings.pauseSpeed;
            
            changeDirection = true;
        } else {
            if (changeDirection == true) {
                dir = new Vector3(Random.value, 0, Random.value);

                changeDirection = false;
            }

            // Don't want it to go too fast
            speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
        }
        velocity = dir * speed;

        transformCache.position += velocity * Time.deltaTime;
        transformCache.forward = dir;
    }

    private bool IsHeadingForCollision() {
        Ray ray = new Ray(transformCache.position, transformCache.forward);

        return SphereCastHelper(ray);
    }

    private Vector3 ObstacleRays() {
        Vector3[] rayDirections = MouseDirections.directions;

        for (int i = 0; i < rayDirections.Length; i++) {
            Vector3 dir = transformCache.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(transformCache.position, dir);
            
            if (!SphereCastHelper(ray)) {
                return dir;
            }
        }

        return transformCache.forward;
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
        Vector3 newVelocity = vector.normalized * settings.maxSpeed - velocity;

        return Vector3.ClampMagnitude(newVelocity, settings.maxSteerForce);
    }
}
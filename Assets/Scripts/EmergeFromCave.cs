using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EmergeFromCave : MonoBehaviour{
    public GameObject Treasure;
    public MonsterBehavior MonsterBehaviorScript;
    public NavMeshAgent Agent;

    void Start() {
        Agent.SetDestination(transform.position - new Vector3(0, 0, 3.1f));
        StartCoroutine(Begin());
    }

    private IEnumerator Begin() {
        while (Agent.remainingDistance > 0.1f) {
            yield return null;
        }

        Treasure.SetActive(true); // if treasure was active at the beginning it would overlap with the monster
        MonsterBehaviorScript.enabled = true;
    }
}

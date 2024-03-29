using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class SpecificHtnPlanner {
    public Queue<String> plan = new Queue<string>();
    private Stack<HtnTask> tasks = new Stack<HtnTask>();
    public WorldState state = new WorldState(true, false, false, 10);

    private Queue<string> backupPlan;
    private Stack<HtnTask> backupTasks;
    private WorldState backupState;
    
    private void SaveState(
        Queue<string> plan, Stack<HtnTask> tasks, HtnTask t, WorldState state
    ) {
        backupPlan = new Queue<string>(plan);
        backupTasks = new Stack<HtnTask>(tasks);
        backupTasks.Push(t);
        backupState = state.Clone();
    }

    private void RestoreSavedState() {
        plan = backupPlan;
        tasks = backupTasks;
        state = backupState;
    }

    public void createPlan(HtnTask root) {
        tasks.Push(root);

        while (tasks.Count > 0) {
            var t = tasks.Pop();

            if (t.IsCompound()) {
                var m = t.FindRandomMethod();

                if (m != null) {
                    SaveState(plan, tasks, t, state);

                    foreach (HtnTask task in m.tasks.AsEnumerable().Reverse()) {
                        tasks.Push(task);
                    }
                } else {
                    RestoreSavedState();
                }
            } else {
                if (Precondition(t.name, state)) {
                    Postcondition(t.name, state);
                    plan.Enqueue(t.name);
                } else {
                    RestoreSavedState();
                }
            }
        }
        /* don't uncomment or else there will be no plan to execute
        while (plan.Count > 0) {
            Debug.Log(plan.Dequeue());
        }
        */
    }

    private bool Precondition(string taskName, WorldState state) {
        switch (taskName) {
            case "Growl":
                return state.isPlayerInRange;
            case "Go to Spawn Location":
                return !state.isPlayerInRange;

            case "Shake":
                return !state.isRed;
            case "Stay Red":
                return state.isRed;
            
            case "Throw Crate":
            case "Throw Rock":
                return state.isHoldingObstacle;
            default:
                return true;

            case "Go to Nearest Crate":
                return state.numCrates > 0;
        }
    }

    private void Postcondition(string taskName, WorldState state) {
        switch (taskName) {
            case "Turn Red":
                state.isRed = true;
                break;
            case "Turn Purple":
                state.isRed = false;
                break;
            
            case "Pick It Up":
                state.isHoldingObstacle = true;
                break;
            case "Throw Rock":
                state.isHoldingObstacle = false;
                break;
            case "Throw Crate":
                state.isHoldingObstacle = false;
                state.numCrates--;
                break;
        }
    }
}
using System;
using System.Collections.Generic;

public class SpecificHtnTree {
    private HtnTask root;

    public HtnTask GetRoot() {
        return root;
    }

    public SpecificHtnTree() {
        root = new HtnTask("Be a Monster");
        
        root.methods.Add(new HtnMethod("Attack"));
        root.methods.Add(new HtnMethod("Idle"));

        root.methods[0].tasks.AddRange(
            new List<HtnTask> {
                new HtnTask("Growl"),
                new HtnTask("Cooldown"),
                new HtnTask("React Angrily"),
                new HtnTask("Projectile Attack")
            }
        );
        root.methods[0].tasks[2].methods.AddRange(
            new List<HtnMethod> {
                new HtnMethod("Get Angry"),
                new HtnMethod("Stay Angry")
            }
        );
        root.methods[0].tasks[2].methods[0].tasks.AddRange(
            new List<HtnTask> {
                new HtnTask("Shake"),
                new HtnTask("Turn Red"),
            }
        );
        root.methods[0].tasks[2].methods[1].tasks.AddRange(
            new List<HtnTask> {
                new HtnTask("Stay Red")
            }
        );
        root.methods[0].tasks[3].methods.AddRange(
            new List<HtnMethod> {
                new HtnMethod("Crate Attack"),
                new HtnMethod("Rock Attack")
            }
        );
        root.methods[0].tasks[3].methods[0].tasks.AddRange(
            new List<HtnTask> {
                new HtnTask("Go to Nearest Crate"),
                new HtnTask("Pick It Up"),
                new HtnTask("Throw Crate")
            }
        );
        root.methods[0].tasks[3].methods[1].tasks.AddRange(
            new List<HtnTask> {
                new HtnTask("Go to Nearest Rock"),
                new HtnTask("Pick It Up"),
                new HtnTask("Throw Rock")
            }
        );

        root.methods[1].tasks.AddRange(
            new List<HtnTask> {
                new HtnTask("Cooldown"),
                new HtnTask("Go to Spawn Location"),
                new HtnTask("Turn Purple"),
                new HtnTask("Idle Behavior")
            }
        );
        root.methods[1].tasks[3].methods.AddRange(
            new List<HtnMethod> {
                new HtnMethod("Pace Back and Forth"),
                new HtnMethod("Move in Circles")
            }
        );
        root.methods[1].tasks[3].methods[0].tasks.AddRange(
            new List<HtnTask> {
                new HtnTask("Move Left"),
                new HtnTask("Move Right"),
                new HtnTask("Move Right"),
                new HtnTask("Move Left"),
                new HtnTask("Double Barrel Roll")
            }
        );
        root.methods[1].tasks[3].methods[1].tasks.AddRange(
            new List<HtnTask> {
                new HtnTask("Move in Circle"),
                new HtnTask("Move in Circle")
            }
        );
    }

    public void Print() {
        RecursiveTask(root, 0);
    }

    private void RecursiveTask(HtnTask t, int indent) {
        Console.WriteLine(new String(' ', indent * 4) + t.name);

        foreach (HtnMethod m in t.methods) {
            RecursiveMethod(m, indent + 1);
        }
    }

    private void RecursiveMethod(HtnMethod m, int indent) {
        Console.WriteLine(new String(' ', indent * 4) + m.name);

        foreach (HtnTask t in m.tasks) {
            RecursiveTask(t, indent + 1);
        }
    }
}
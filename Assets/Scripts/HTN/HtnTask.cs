using System.Collections.Generic;
using UnityEngine;

public class HtnTask {
    public List<HtnMethod> methods = new List<HtnMethod>();
    public string name;
    public WorldState effects;

    public HtnTask(string name) {
        this.name = name;
    }

    public bool IsCompound() {
        return methods.Count > 0;
    }

    public HtnMethod FindRandomMethod() {
        int rndIndex = Random.Range(0, methods.Count);

        return methods[rndIndex];
    }
}
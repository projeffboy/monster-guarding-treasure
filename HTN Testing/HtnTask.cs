using System.Collections.Generic;
using System;

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
        Random rnd = new Random();
        int rndIndex = rnd.Next(0, methods.Count);

        return methods[rndIndex];
    }
}
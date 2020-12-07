using System;

namespace HTN_Testing {
    class Program {
        static void Main(string[] args) {
            var tree = new SpecificHtnTree();
            HtnTask root = tree.GetRoot();
            // tree.Print();

            var planner = new SpecificHtnPlanner();
            planner.createPlan(root);
        }
    }
}

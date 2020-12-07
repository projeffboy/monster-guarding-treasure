public class WorldState {
    public bool isPlayerInRange;
    public bool isRed;
    public bool isHoldingObstacle;
    public int numCrates;

    public WorldState(bool a, bool b, bool c, int d) {
        isPlayerInRange = a;
        isRed = b;
        isHoldingObstacle = c;
        numCrates = d;
    }

    public void Edit(bool a, bool b, bool c, int d) {
        isPlayerInRange = a;
        isRed = b;
        isHoldingObstacle = c;
        numCrates = d;
    }

    public void SetIsPlayerInRange(bool a) {
        isPlayerInRange = a;
    }

    public WorldState Clone() {
        return new WorldState(
            isPlayerInRange, isRed, isHoldingObstacle, numCrates
        );
    }
}
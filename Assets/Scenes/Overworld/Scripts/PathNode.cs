using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {

    private Grid<PathNode> grid;

    public int x, y;                // Holds the x and y coordinates of the node
    public int G, H, F;             // Holds respective g, h, and f cost values
    public bool isWalkable;         // Can the tile be walked on or not
    public PathNode previousNode;   // Node that was traversed previously to this one

    public PathNode(Grid<PathNode> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public void CalculateFCost() {
        F = G + H;
    }

    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString() {
        return x + ", " + y;
    }
}

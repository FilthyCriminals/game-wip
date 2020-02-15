using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {

    private Grid<PathNode> grid;
    public int x, y;

    // Holds respective g, h, and f cost values
    public int G, H, F;

    // Node that was traversed previously to this one
    public PathNode previousNode;

    public PathNode(Grid<PathNode> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void CalculateFCost() {
        F = G + H;
    }

    public override string ToString() {
        return x + ", " + y;
    }
}

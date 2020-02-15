using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {

    private const int MOVE_DIAGONAL_COST = 14, MOVE_STRAIGHT_COST = 10;

    private Grid<PathNode> grid;
    private List<PathNode> openList, closedList;

    public Pathfinding(int width, int height) {
        grid = new Grid<PathNode>(width, height, 10f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public Grid<PathNode> GetGrid() {
        return grid;
    }

    public PathNode GetNode(int x, int y) {
        return grid.GetGridObject(x, y);
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                PathNode pathNode = grid.GetGridObject(x, y);
             
                pathNode.G = int.MaxValue;
                pathNode.CalculateFCost();

                pathNode.previousNode = null;
            }
        }

        startNode.G = 0;
        startNode.H = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        // Iterate over all nodes in the list to process
        while (openList.Count > 0) {
            PathNode currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode)
                return CalculatePath(endNode);

            // Remove node from list to process and add to processed node list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // Check neighbor nodes of current nodes
            foreach (PathNode neighborNode in GetNeighborList(currentNode)) {
                if (closedList.Contains(neighborNode))
                    continue;

                int tentativeG = currentNode.G + CalculateDistanceCost(currentNode, neighborNode);

                if (tentativeG < neighborNode.G) {
                    neighborNode.previousNode = currentNode;
                    neighborNode.G = tentativeG;
                    neighborNode.H = CalculateDistanceCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if (!openList.Contains(neighborNode)) {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        // Out of nodes in open list
        return null;
    }

    private List<PathNode> GetNeighborList(PathNode currentNode) {
        List<PathNode> neighborList = new List<PathNode>();

        if (currentNode.x - 1 >= 0) {
            // Left
            neighborList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Bottom left
            if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Top left
            if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth()) {
            // Right
            neighborList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Bottom right
            if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Top right
            if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Top middle
        if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Bottom middle
        if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighborList;
    }

    // Traces path back after list iteration is complete
    private List<PathNode> CalculatePath(PathNode endNode) {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.previousNode != null) {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }

        path.Reverse();

        return path;
    }

    // Calculates the h-cost (heuristic estimate) for distance to the end node
    private int CalculateDistanceCost(PathNode a, PathNode b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) {
        PathNode lowestFNode = pathNodeList[0];

        for (int i = 1; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].F < lowestFNode.F)
                lowestFNode = pathNodeList[i];
        }

        return lowestFNode;
    }
}

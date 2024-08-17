using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
 * Maze Solver uses the A* algorithm. It was chosen over Dijkstra because A*
 * implementations are preferable when there is a specific start coordinate as
 * well as a specific goal coordinate, perfect for a maze. 
 * 
 * Additionally, algorithms like DFS were also considered, but ultimately, A*
 * appeared to be the best option due to the fact that A* will look for the 
 * shortest path to the goal coordinate, a popular objective in maze games.
 * This algorithm would be useful with a sample input with numerous maze 
 * solutions
 * 
 * Finally, concerns about memory and time complexity do not need to be 
 * prioritized with mazes of this size, or games with this simplicity. 
 * So, A* appeared to be the best option and has been implemented below. 
 */

public class MazeSolver : MonoBehaviour
{
    /* In keeping with the classic reference to the search tree,
     * the maze path will be treated like a tree, with each 
     * open path unit referred to as the maze node.
     */
    public List<Vector2> FindPathThroughMaze(int[,] matrix, Vector2 start, Vector2 end)
    {
        int startX = (int)start.x;
        int startY = (int)start.y;
        int endX = (int)end.x;
        int endY = (int)end.y;

        // Initialize open and closed sets
        Dictionary<string, MazeNode> openSet = new Dictionary<string, MazeNode>();
        HashSet<string> closedSet = new HashSet<string>();

        // Create the start and end nodes
        MazeNode startNode = new MazeNode { X = startX, Y = startY, CostFromStart = 0, CostToEnd = CalculateDistance(startX, startY, endX, endY) };
        MazeNode endNode = new MazeNode { X = endX, Y = endY };

        string startKey = GetKey(startNode.X, startNode.Y);
        openSet.Add(startKey, startNode);

        while (openSet.Count > 0)
        {
            // Get the node with the lowest TotalCost (CostFromStart + CostToEnd)
            MazeNode currentNode = openSet.Values.OrderBy(n => n.TotalCost).First();
            string currentKey = GetKey(currentNode.X, currentNode.Y);

            // Move current node from open to closed set
            openSet.Remove(currentKey);
            closedSet.Add(currentKey);

            // Check if we've reached the goal coordinate
            if (currentNode.X == endNode.X && currentNode.Y == endNode.Y)
            {
                return BuildSolutionPath(currentNode); // Return the solution path
            }

            // Explore neighbor nodes
            foreach (var neighbor in GetNeighborNodes(matrix, currentNode, endNode))
            {
                string neighborKey = GetKey(neighbor.X, neighbor.Y);

                if (closedSet.Contains(neighborKey))
                    continue;

                if (!openSet.ContainsKey(neighborKey))
                {
                    openSet.Add(neighborKey, neighbor);
                }
                else
                {
                    var existingNeighbor = openSet[neighborKey];
                    if (neighbor.CostFromStart < existingNeighbor.CostFromStart)
                    {
                        existingNeighbor.CostFromStart = neighbor.CostFromStart;
                        existingNeighbor.PrevNode = currentNode;
                    }
                }
            }
        }

        // No path found
        return null;
    }

    // Helper function to reconstruct the solution path and return it as a list of Vector2s
    private List<Vector2> BuildSolutionPath(MazeNode endNode)
    {
        List<Vector2> path = new List<Vector2>();
        MazeNode currentNode = endNode;

        while (currentNode != null)
        {
            path.Add(new Vector2(currentNode.X, currentNode.Y));
            currentNode = currentNode.PrevNode;
        }

        path.Reverse();
        return path;
    }

    private float CalculateDistance(int startX, int startY, int endX, int endY)
    {
        return Mathf.Abs(startX - endX) + Mathf.Abs(startY - endY);
    }

    private string GetKey(int x, int y)
    {
        return $"{x},{y}";
    }
    //Search through all the neigbors in all directions and add to the list of neighbor nodes if the path is open. 
    private List<MazeNode> GetNeighborNodes(int[,] matrix, MazeNode currentNode, MazeNode endNode)
    {
        int[][] directions = new int[][]
        {
            new int[] { 0, -1 },  new int[] { 1, 0 },new int[] { 0, 1 },  new int[] { -1, 0 }
        };

        List<MazeNode> neighbors = new List<MazeNode>();
        foreach (var dir in directions)
        {
            int newX = currentNode.X + dir[0];
            int newY = currentNode.Y + dir[1];

            if (IsWithinBounds(matrix, newX, newY) && matrix[newY, newX] == 0)
            {
                neighbors.Add(new MazeNode
                {
                    X = newX,
                    Y = newY,
                    CostFromStart = currentNode.CostFromStart + 1,
                    CostToEnd = CalculateDistance(newX, newY, endNode.X, endNode.Y),
                    PrevNode = currentNode
                });
            }
        }

        return neighbors;
    }

    private bool IsWithinBounds(int[,] matrix, int x, int y)
    {
        return x >= 0 && y >= 0 && x < matrix.GetLength(1) && y < matrix.GetLength(0);
    }

    // Node class represents each point in the maze
    private class MazeNode
    {
        public int X, Y;
        public float CostFromStart = float.MaxValue;
        public float CostToEnd;
        public float TotalCost => CostFromStart + CostToEnd;
        public MazeNode PrevNode;
    }
}

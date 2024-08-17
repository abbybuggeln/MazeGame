using System.Collections.Generic;
using UnityEngine;

/*
 * This is the main controller file that handles the execution 
 * of game logic and holds references to all the other files. 
 * It is the only file that makes calls to functions in the other files,
 * to keep the project as decoupled and readable and possible. 
 * 
 */
public class MazeController : MonoBehaviour
{
    public InputHandler inputHandler;
    public MazeGenerator mazeGenerator;
    public MazeSolver mazeSolver;
    public CharacterAnimator characterAnimator;
    public GameObject characterPrefab;

    void Start()
    {
        int[,] maze = inputHandler.ParseMaze();
        mazeGenerator.GenerateMaze(maze);

        Vector2 start = new Vector2(0, 0);
        Vector2 goal = new Vector2(maze.GetLength(1) - 1, maze.GetLength(0) - 1);

        List<Vector2> solutionPath = mazeSolver.FindPathThroughMaze(maze, start, goal);

        //Debug: If the camera is not centered on the maze, there is an issue parsing or solving the maze. 
        if (solutionPath != null && solutionPath.Count > 0)
        {
            CenterCameraOnMaze(maze);
            characterAnimator.AnimateCharacter(solutionPath, characterPrefab);
        }
        else
        {
            Debug.Log("No solution found!");
        }
    }

    private void CenterCameraOnMaze(int[,] maze)
    {
        Camera.main.orthographicSize = maze.GetLength(0) / 2f;
        Camera.main.transform.position = new Vector3((maze.GetLength(1) - 1) / 2f, -(maze.GetLength(0) - 1) / 2f, -10);
    }
}

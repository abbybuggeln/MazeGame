using UnityEngine;

/*
 * Maze Generator Takes in the maze in the format of a 2D array
 * and loops through the row array + length array to instantiate 
 * walls or paths based identifying 1's and 0's 
 */
public class MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject pathPrefab;

    public void GenerateMaze(int[,] maze)
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                GameObject toInstantiate = maze[i, j] == 1 ? wallPrefab : pathPrefab;
                Instantiate(toInstantiate, new Vector3(j, -i, 0), Quaternion.identity);
            }
        }
    }
}

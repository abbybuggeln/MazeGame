using System.Collections.Generic;
using UnityEngine;

/*
 * This class holds the sample string input for the ascii maze.
 * It parses the maze and stores it in a simple data structure
 * that will be used by the maze generator. 
 */
public class InputHandler : MonoBehaviour
{
    private string mazeInput = @"
+--+--+--+--+--+--+--+--+--+--+
                  |        |  |
+--+--+  +--+--+  +  +--+  +  +
|     |  |  |     |  |     |  |
+  +  +  +  +  +--+--+  +--+  +
|  |  |  |  |        |     |  |
+  +--+  +  +--+--+  +  +  +  +
|           |     |     |     |
+--+--+  +--+  +  +--+--+--+  +
|     |     |  |  |        |  |
+  +  +  +  +  +  +  +--+  +  +
|  |     |  |  |     |     |  |
+  +--+--+  +  +--+--+  +--+  +
|        |     |  |     |     |
+--+--+  +--+--+  +  +--+--+--+
|           |  |     |        |
+  +--+--+  +  +--+--+  +--+  +
|        |     |     |  |     |
+--+--+  +  +--+  +  +  +  +--+
|        |        |     |      
+--+--+--+--+--+--+--+--+--+--+";

    /*
    * ParseMaze: Parser that takes the string input 
    * of the maze and stores it in a 2D array.
    * The biggest challenge I faced with this maze parser was actually 
    * in formatting my maze correctly, so ParseMaze has some format issue 
    * handling as well. 
    */
    public int[,] ParseMaze()
    {
    
        mazeInput = mazeInput.Trim();  //Remove white space to prevent format irregularities 

       //Create a string of lines from the ascii input
        string[] lines = mazeInput.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        // Find the max row length
        int maxLength = 0;
        foreach (string line in lines)
        {
            maxLength = Mathf.Max(maxLength, line.Length);
        }

        // Pad rows to ensure all have the same length
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].PadRight(maxLength);
        }

        int rows = lines.Length;
        int cols = maxLength;
        int[,] maze = new int[rows, cols];

        // Comvert maze 
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                char c = lines[i][j];
                if (c == '+' || c == '-' || c == '|') 
                {
                    maze[i, j] = 1;
                }
                else 
                {
                    maze[i, j] = 0;
                }
            }
        }
        return maze;
    }
}
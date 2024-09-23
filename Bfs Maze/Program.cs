using System.Drawing;
using System.IO;
using System.Net;

namespace Bfs_Maze
{
    //1. **Maze Representation**: Represent a maze as a 2D grid where:
    //- `0` represents an open path.
    //- `1` represents a wall.
    //- `S` represents the starting point.
    //- `E` represents the endpoint.
    // The program should take a maze as input and display the steps taken to solve it.
    //- The output should clearly show the path from the starting point to the endpoint, using * to represent the path taken.
    // Going to calculate the shortest path because there may be many paths

    public class Program
    {
        // global variables

        static Stack<(int, int)> stack = new Stack<(int, int)>(); // can use a single stack with a tuple 


        static Stack<(int,int)> pathStack = new Stack<(int,int)> (); // stack to store the path

        // direction vectors, up down left right
        static int[] dr = [-1, 1, 0, 0];
        static int[] dc = [0, 0, 1, -1];

        static Stack<int> rowStack = new Stack<int>(); // seperate stacks to add nodes. A node is made up of a set of int co-ordinates
        static Stack<int> colStack = new Stack<int>();

        public static void traverseDfs(char[,] maze)
        {

            // Calls method to get start position row and column values
            var startPosition = FindStartPosition(maze);
            int startRow = startPosition.Value.row;
            int startColumn = startPosition.Value.col;

            // Variable to track whether 'E' ever gets reached
            bool reachedEnd = false; 

            // R x C matrix of false values used to track whether node has been visited
            bool[,] visited = InitializeVisitedArray(maze);
           

            // solve with depth first search
            rowStack.Push(startRow); // adding starting node to the stack
            colStack.Push(startColumn);

            visited[startRow,startColumn] = true; // setting visited to true 

            while (rowStack.Count != 0) // while there is still a node in the stack
            { 
                int rowNode = rowStack.Pop(); // gets current node
                int colNode = colStack.Pop();

                pathStack.Push((rowNode, colNode)); // adds current node to the path

                if (maze[rowNode, colNode] == 'E') // if we have reached the end 
                { 
                    reachedEnd = true;
                    Console.WriteLine("You have reached the end of the maze"); // need retrace steps and mark them with a * and then display the maze
                    break;
                }

                // explore neighbours and add them to the stack
                ExploreNeighbours(rowNode, colNode,maze,visited);



            }

            if (reachedEnd)
            {
                MarkPath(maze);
            }
            else
            {
                Console.WriteLine("No path was found");
            }

        }

        // Method to trace path and mark with *
        public static void MarkPath(char[,] maze)
        {
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);

            while (pathStack.Count != 0)
            {
                var (r, c) = pathStack.Pop();

                if (maze[r, c] != 'S' && maze[r, c] != 'E')
                {
                    maze[r, c] = '*';
                }

            }

            // printing maze

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {

                    Console.Write(maze[i, j] + " ");
                }
                Console.WriteLine();
            }
        }



        public static void ExploreNeighbours( int r, int c, char[,] maze, bool[,] visited)
        {

            // rows and column
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);

            // getting neighbour columns by manipulating the position of the co-ordinate of the matrix
            for (int i = 0; i < 4; i++)
            {
                int rr = r + dr[i];
                int cc = c + dc[i];

                // skips out of bounds locations
                if (rr < 0 | cc < 0) continue;
                if (rr >= rows | cc >= cols) continue;

                // Skips visited locations and blocked cells
                if (visited[rr, cc]) continue;
                if (maze[rr,cc] == '1') continue;

                // adding the node to the stack
                rowStack.Push(rr);
                colStack.Push(cc);

                // marking as visited
                visited[rr, cc] = true;                  
            }
        }


        // method to find start position of the maze, returns a tuple
        public static (int row, int col)? FindStartPosition(char[,] maze)
        {
            int rows = maze.GetLength (0);
            int cols = maze.GetLength (1);

            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < cols; j++) {

                    if (maze[i,j] == 'S')
                    {
                        return(i,j);
                    }
                    
                }
            
            }
            return null;
        }

        public static bool[,] InitializeVisitedArray(char[,] maze) 
        { 
            int rows = maze.GetLength (0);
            int cols = maze.GetLength (1);
            bool[,]  visited = new bool [rows, cols];

            for (int i = 0; i < rows; i++) 
            {
                for (int j = 0; j < cols; j++)
                {
                    visited[i, j] = false;
                }            
            }
        return visited;

        }

        public static void Main(string[] args)
        {
            char[,] maze = new char[,] {
        { 'S', '1', '0', '0', '0', '0' },
        { '0', '1', '0', '1', '1' ,'0' },
        { '0', '0', '0', '1', '1' ,'0' },
        { '1', '1', '0', '1', '1' ,'0' },
        { '0', '0', '0', '0', '1' ,'E' },

    };
            traverseDfs(maze);
            }
        }
    }


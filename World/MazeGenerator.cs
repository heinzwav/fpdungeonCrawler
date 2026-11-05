using DungeonCrawlerMitHandy.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawlerMitHandy.World
{
    public class MazeGenerator
    {
        public GeneratedMap Generate(int width, int height, int seed)
        {
            // Die Welt muss ungerade sein, sonst hat man außenrum nie Wände
            if (width % 2 == 0) throw new ArgumentOutOfRangeException(nameof(width), "Breite muss ungerade sein!");
            if (height % 2 == 0) throw new ArgumentOutOfRangeException(nameof(height), "Höhe muss ungerade sein!");

            // Die Welt muss mindestens 5x5 groß sein, sonst entstehen keine Gänge
            if (width < 5) throw new ArgumentOutOfRangeException(nameof(width), "Breite muss mindestens 5 sein!");
            if (height < 5) throw new ArgumentOutOfRangeException(nameof(height), "Höhe muss mindestens 5 sein!");
            
            
            //==== Start der Generation ====\\            
            
            
            // Zuert die Welt komplett mit Wänden befüllen
            Tile[,] maze = new Tile[width, height];
            
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    maze[x, y] = new Tile { Type = TileType.Wall };
                }

            // StartTile festlegen und in den Stack pushen
            Stack<GridPos> stack = new Stack<GridPos>();
            GridPos start = new(1, 1);
            maze[start.X, start.Y].Type = TileType.Floor;
            stack.Push(start);

            Random random = new Random(seed);

            // Die while-Schleife läuft solange, bis es keine validen Tiles mehr gibt
            while (stack.Count > 0)
            {
                GridPos currentTile = stack.Peek();               
                List<Direction> validDirections = new();

                foreach (Direction direction in Enum.GetValues<Direction>())
                {
                    // Das Tile berechnen, welches Punkte in die Richtung geht
                    GridPos targetTile = currentTile + (direction.ToStep() * 2);

                    // Prüfen ob das Tile eine Wand ist (also bereits gesetzt), oder ob es noch in Bounds ist
                    if (IsInsideBorder(targetTile, width, height) 
                        && maze[targetTile.X, targetTile.Y].Type == TileType.Wall)
                    {
                        validDirections.Add(direction);
                    }               
                }

                // Falls keine Richtungen existieren, die man entlang gehen kann, entfernt man das oberste Tile
                if (validDirections.Count == 0) stack.Pop();
                else
                {
                    Direction chosen = validDirections[random.Next(validDirections.Count)];
                    GridPos tileBetween = currentTile + chosen.ToStep();
                    GridPos targetTile = currentTile + (chosen.ToStep() * 2);
                    maze[tileBetween.X, tileBetween.Y].Type = TileType.Floor;
                    maze[targetTile.X, targetTile.Y].Type = TileType.Floor;
                    stack.Push(targetTile);
                }
            }
            
            GridPos stairs = GetStairway(maze, start);
            maze[stairs.X, stairs.Y].Type = TileType.Stairs;

            return new GeneratedMap(maze, start, stairs);
        }

        private bool IsInsideBorder(GridPos pos, int width, int height)        
            => pos.X >= 1 && pos.X < width -1 && pos.Y >= 1 && pos.Y < height -1;

        private GridPos GetStairway(Tile[,] maze, GridPos start)
        {
            int[,] distances = DistanceMap.Calculate(maze, start);

            GridPos farthest = start;
            int maxDistance = 0;

            for (int x = 0; x < distances.GetLength(0); x++)
                for (int y = 0; y < distances.GetLength(1); y++)
                {
                    if (distances[x, y] > maxDistance)
                    {
                        maxDistance = distances[x, y];
                        farthest = new GridPos(x, y);
                    }
                }

            return farthest;
        }

    }
}

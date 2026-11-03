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
            maze[1, 1].Type = TileType.Floor;
            stack.Push(new GridPos(1, 1));

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

            return new GeneratedMap(maze, new GridPos(1, 1), new GridPos(1, 1));
        }

        private bool IsInsideBorder(GridPos pos, int width, int height)        
            => pos.X >= 1 && pos.X < width -1 && pos.Y >= 1 && pos.Y < height -1;
        
        private GridPos GetStairway(Tile[,] finishedWorld, GridPos start)
        {
            int[,] array = new int[finishedWorld.GetLength(0), finishedWorld.GetLength(1)];

            for (int x = 0; x < array.GetLength(0); x++)
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    array[x, y] = -1;
                }            
            array[start.X, start.Y] = 0;
            int currentNumber = 0;
            bool found = true;

            while(found)
            {
                found = false;

                for (int x = 0; x < array.GetLength(0); x++)
                    for (int y = 0; y < array.GetLength(1); y++)
                    {
                        GridPos currentTile = new(x, y);
                        foreach (Direction direction in Enum.GetValues<Direction>())
                        {
                            // Das Tile berechnen, welches Punkte in die Richtung geht
                            GridPos targetTile = currentTile 

                            // Prüfen ob das Tile eine Wand ist (also bereits gesetzt), oder ob es noch in Bounds ist
                            if (IsInsideBorder(targetTile, width, height)
                                && maze[targetTile.X, targetTile.Y].Type == TileType.Wall)
                            {
                                validDirections.Add(direction);
                            }
                        }
                    }
            }
            
        }

    }
}

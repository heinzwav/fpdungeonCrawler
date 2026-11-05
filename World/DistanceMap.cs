using DungeonCrawlerMitHandy.Core;

namespace DungeonCrawlerMitHandy.World
{
    public static class DistanceMap
    {
        // Markierung für Felder, die vom Start aus nicht erreichbar sind
        public const int Unreachable = -1;

        public static int[,] Calculate(Tile[,] tiles, GridPos start)
        {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);

            if (!IsInArray(start, width, height))
                throw new ArgumentOutOfRangeException(nameof(start), $"Startpunkt {start} liegt außerhalb der Map.");

            int[,] distances = new int[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    distances[x, y] = Unreachable;

            distances[start.X, start.Y] = 0;
            int currentNumber = 0;
            bool found = true;

            while (found)
            {
                found = false;

                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        // Nur von Feldern der aktuellen Nummer weitermachen
                        if (distances[x, y] != currentNumber) continue;

                        GridPos currentTile = new(x, y);

                        foreach (Direction direction in Enum.GetValues<Direction>())
                        {
                            GridPos neighbour = currentTile + direction.ToStep();

                            if (IsInArray(neighbour, width, height)
                                && tiles[neighbour.X, neighbour.Y].Type != TileType.Wall
                                && distances[neighbour.X, neighbour.Y] == Unreachable)
                            {
                                distances[neighbour.X, neighbour.Y] = currentNumber + 1;
                                found = true;
                            }
                        }
                    }

                currentNumber++;
            }

            return distances;
        }

        private static bool IsInArray(GridPos pos, int width, int height)
            => pos.X >= 0 && pos.X < width && pos.Y >= 0 && pos.Y < height;
    }
}
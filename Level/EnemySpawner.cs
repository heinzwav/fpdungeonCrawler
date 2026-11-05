using DungeonCrawlerMitHandy.Core;
using DungeonCrawlerMitHandy.Entities;
using DungeonCrawlerMitHandy.World;

namespace DungeonCrawlerMitHandy.Level
{
    public class EnemySpawner
    {
        private EnemyFactory Factory { get; }

        public EnemySpawner(EnemyFactory factory)
        {
            Factory = factory;
        }

        public void SpawnEnemies(GameWorld world, GeneratedMap map, Random random)
        {
            /* Minimaler Abstand zum Spawn wächst mit der Größe der Map.
             * Maximal ist 10 möglich */
            int width = map.Tiles.GetLength(0);
            int minDistance = Math.Clamp(width / 2, 3, 10);
            int minEnemyDistance = 3;

            // Laufwege vom Start berechnen
            int[,] distancesFromStart = DistanceMap.Calculate(map.Tiles, map.Start);

            List<GridPos> candidates = new();

            // Tile speichern, wenn es ein Floor ist und weit genug vom Start entfernt
            for (int x = 0; x < width; x++)
                for (int y = 0; y < map.Tiles.GetLength(1); y++)
                {
                    if (map.Tiles[x, y].Type == TileType.Floor &&
                        distancesFromStart[x, y] >= minDistance)
                        candidates.Add(new GridPos(x, y));
                }

            // Maximale Anzahl abhängig davon, wie viel Platz es gibt
            int maxEnemies = Math.Clamp(candidates.Count / 5, 1, 10);

            EnemyType[] types = Enum.GetValues<EnemyType>();
            int spawned = 0;

            while (spawned < maxEnemies && candidates.Count > 0)
            {
                int index = random.Next(0, candidates.Count);
                GridPos pos = candidates[index];
                candidates.RemoveAt(index);

                EnemyType type = types[random.Next(0, types.Length)];
                Direction facing = GetEntityDirection(distancesFromStart, pos, random);

                Enemy enemy = Factory.Create(type, pos, facing, random);
                world.Spawn(pos, enemy);
                candidates.RemoveAll(c => c.DistanceTo(pos) < minEnemyDistance);
                spawned++;
            }
        }

        private Direction GetEntityDirection(int[,] distances, GridPos entityPos, Random random)
        {
            int distance = distances[entityPos.X, entityPos.Y];

            if (distance <= 0)
                throw new InvalidOperationException(
                    $"Entity auf {entityPos} hat Abstand {distance} zum Start, Richtung zum Start nicht bestimmbar.");

            List<Direction> candidates = new();

            foreach (Direction direction in Enum.GetValues<Direction>())
            {
                GridPos neighbourPos = entityPos + direction.ToStep();
                int neighbourValue = distances[neighbourPos.X, neighbourPos.Y];

                if (neighbourValue == distance - 1)
                    candidates.Add(direction);
            }

            if (candidates.Count == 0)
                throw new InvalidOperationException(
                    $"Entity auf {entityPos}: Kein Nachbar führt Richtung Start.");

            return candidates[random.Next(candidates.Count)];
        }
    }
}
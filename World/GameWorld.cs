using DungeonCrawlerMitHandy.Core;
using DungeonCrawlerMitHandy.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawlerMitHandy.World
{
    public class GameWorld
    {
        public int Width { get; }
        public int Height { get; }
        private Tile[,] Tiles { get; set; }
        private Dictionary<GridPos, Entity> EntityByPosition { get; set; } = new();
        private Dictionary<Entity, GridPos> PositionByEntity { get; set; } = new();

        public GameWorld(Tile[,] tiles)
        {
            Tiles = tiles;
            Width = tiles.GetLength(0);
            Height = tiles.GetLength(1);
        }

        public Tile? GetTile(GridPos pos)
            => IsInBounds(pos) ? Tiles[pos.X, pos.Y] : null;

        public bool IsInBounds(GridPos pos)
        {
            return pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height;
        }

        public void Spawn(GridPos pos, Entity entity)
        {
            if (!IsInBounds(pos)) throw new ArgumentOutOfRangeException(nameof(pos));
            if (GetEntityAtTile(pos) != null) throw new InvalidOperationException($"{pos} besitzt bereits eine Entity!");
            if (Tiles[pos.X, pos.Y].Type != TileType.Floor) throw new InvalidOperationException($"{pos} darf nur vom TileType Floor sein!");
            if (PositionByEntity.ContainsKey(entity)) throw new InvalidOperationException($"({entity.Name}) existiert bereits in der Welt");

            EntityByPosition.Add(pos, entity);
            PositionByEntity.Add(entity, pos);
        }

        public Entity? GetEntityAtTile(GridPos pos)
            => EntityByPosition.GetValueOrDefault(pos);

        public GridPos? GetPositionOfEntity(Entity entity)
            => PositionByEntity.TryGetValue(entity, out GridPos pos) ? pos : null;



        public bool TryMove(Entity entity, Direction direction)
        {
            if (GetPositionOfEntity(entity) is not GridPos currentPosition)
                throw new InvalidOperationException($"{entity.Name} existiert nicht in der Welt!");

            GridPos targetPosition = currentPosition + direction.ToStep();

            if (!IsInBounds(targetPosition)) return false;
            if (GetEntityAtTile(targetPosition) != null) return false;
            if (Tiles[targetPosition.X, targetPosition.Y].Type == TileType.Wall) return false;
            
            EntityByPosition.Remove(currentPosition);
            PositionByEntity.Remove(entity);

            EntityByPosition.Add(targetPosition, entity);
            PositionByEntity.Add(entity, targetPosition);

            return true;
        }

        public void RemoveEntity(Entity entity)
        {
            if (!PositionByEntity.TryGetValue(entity, out GridPos pos)) return;
            EntityByPosition.Remove(pos);
            PositionByEntity.Remove(entity);
        }

        public Entity? GetFirstEntityInDirection(GridPos from, Direction direction, int maxDistance)
        {
            for (int distance = 1; distance <= maxDistance; distance++)
            {
                GridPos targetTile = from + direction.ToStep() * distance;

                // Wenn dort eine Wand ist, braucht man gar nicht mehr weitersuchen
                Tile? tile = GetTile(targetTile);
                if (tile == null || tile.Type == TileType.Wall) return null;

                Entity? entity = GetEntityAtTile(targetTile);
                if (entity != null) return entity;
            }

            return null;
        }
    }



}


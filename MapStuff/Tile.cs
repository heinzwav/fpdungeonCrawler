using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace fpdungeonCrawler.Map
{
    public class Tile
    {
        public Entity? entity { get; set; }
        public Entity? player { get; set; }
        public enum TileType : int {Stairs = 2, Wall = 1, Floor = 0 }
        public TileType Type { get; set; }

        public int TilePositionX { get; set; }
        public int TilePositionY { get; set; }


        public Image imgWall = Properties.Resources.WallTile;
        public Image imgFloor = Properties.Resources.FloorTile;
        public Image imgStairs = Properties.Resources.Stairs;

        public bool isVisited;
        public bool invalidFloorPosition;
    }
}

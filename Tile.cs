using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public class Tile
    {
        public Entity? entity { get; set; }
        public enum TileType : int { Wall = 1, Floor = 0 }
        public TileType Type { get; set; }

        public int TilePositionX { get; set; }
        public int TilePositionY { get; set; }


        public Image imgWall = Properties.Resources.WallTile;
        public Image imgFloor = Properties.Resources.FloorTile;
        public PictureBox PicBox;

        public bool isWall;


        //public Tile AddTile(Map map)
        //{
        //    Tile tile = map.mapTiles.
        //    if (tile.TilePositionX < 0 || tile.TilePositionX > map.SizeX || tile.TilePositionY < 0 || tile.TilePositionY > map.SizeY)
        //    {
        //        Tile replacementtile = new();
        //        return replacementtile;
        //    }
        //    else
        //    {
        //        return 
        //    }

        //}
    }
}

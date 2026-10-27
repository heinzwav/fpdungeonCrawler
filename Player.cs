using fpdungeonCrawler.Map;
using fpdungeonCrawler.UI;
using System;
using System.Collections.Generic;
using System.Media;
using System.Text;

namespace fpdungeonCrawler
{

    public class Player : Entity
    {
        public new (int Column, int Row) 
            StartPosition { get; set; } = new(1, 1);

        public Tile[] View = new Tile[9];

        public bool StateHit;

      
        public Player GeneratePlayer(fpdungeonCrawler.Map.Map map)
        {
            Player player = new Player();
            Tile tile = new Tile();
            tile.TilePositionX = player.StartPosition.Column;
            tile.TilePositionY = player.StartPosition.Row;
            tile.player = player;
            map.mapTiles[player.StartPosition.Column, player.StartPosition.Row] = tile;
            player.baseDamage = 100;
            player.orientation = Player.EntityOrientation.Right;
            player.CurrentPosition = player.StartPosition;
            player.healthpoints = 100;
            player.EquippedWeapon = ItemDatabase.Sword;

            player.attackAnimation = AnimationDatabase.swordhit;
            return player;
        }

    }


}

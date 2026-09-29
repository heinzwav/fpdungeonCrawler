using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{

    public class Player : Entity
    {
        public enum PlayerOrientation { Left, Right, Up, Down }
        public PlayerOrientation orientation = PlayerOrientation.Up;

        public (int Column, int Row) CurrentPosition { get; set; } = new(1, 7);

        public (int Column, int Row) StartPosition { get; set; } = new(1, 7);

        public Tile[] View = new Tile[9];

       
        
    }


}

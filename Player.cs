using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public class Player : Entity
    {
        enum PlayerOrientatione { Left, Right, Up, Down }
        
        
        public PictureBox pbPlayer { get; set; } = new();



        //Map map = new();
               

        //private void GetPlayerView()
        //{
        //    currentPos = map.Minimap.GetCellPosition(pbPlayer);
        //    int k = 0;
        //    for (int i = currentPos.Row - 2; i <= currentPos.Row; i++)
        //    {
        //        for (int j = currentPos.Column - 1; j <= currentPos.Column + 1; j++)
        //        {
        //            map.PlayerViewTiles[k] = new TableLayoutPanelCellPosition(i, j);
        //            k += 1;
        //        }

        //    }
        //}
    }


}

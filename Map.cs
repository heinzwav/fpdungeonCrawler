using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Formats.Asn1;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace fpdungeonCrawler
{
    public class Map
    {
        int[,] mapCoordinates = new int[10, 10];
        public TableLayoutPanelCellPosition[] PlayerViewTiles = new TableLayoutPanelCellPosition[9];
        public TableLayoutPanel Minimap { get; set; } = new();
        public Player player = new();



        public void GenerateMap()
        {
            //Minimap
            Minimap.AutoSize = true;
            Minimap.ColumnCount = 10;
            Minimap.RowCount = 10;
            Minimap.Location = new System.Drawing.Point(25, 25);


            for (int i = 0; i < mapCoordinates.GetLength(0); i++)
            {
                for (int j = 0; j < mapCoordinates.GetLength(1); j++)
                {
                    Tile Wall = new Tile();
                    Wall.TilePic.Size = new Size(15, 15);
                    Wall.TilePic.BackColor = Color.Black;
                    Wall.TilePic.Tag = "Wall";
                    Minimap.Controls.Add(Wall.TilePic);
                    Minimap.SetCellPosition(Wall.TilePic, new TableLayoutPanelCellPosition(i, j));
                }
            }


            //Emptys

            for (int i = 3; i < 9; i++)
            {
                GeneratePath(1, i, "Empty");
            }

            for (int i = 2; i < 8; i++)
            {
                GeneratePath(i, 5, "Empty");
            }
            for (int i = 4; i < 9; i++)
            {
                GeneratePath(6, i, "Empty");
            }

            //SetPlayerPosition

        }

        private void GeneratePath(int col, int row, string tag)
        {
            Tile Empty = new Tile();
            Empty.TilePic.Size = new Size(15, 15);
            Empty.TilePic.BackColor = Color.AntiqueWhite;
            Empty.TilePic.Tag = tag;
            Control replacePic = Minimap.GetControlFromPosition(col, row);
            replacePic.Dispose();
            Minimap.Controls.Add(Empty.TilePic);
            Minimap.SetCellPosition(Empty.TilePic, new TableLayoutPanelCellPosition(col, row));
        }
        public void InitializePlayer()
        {
            player.pbPlayer.BackColor = SystemColors.ButtonHighlight;
            player.pbPlayer.BackgroundImage = Properties.Resources.PlayerArrow;
            player.pbPlayer.BackgroundImageLayout = ImageLayout.Stretch;
            player.pbPlayer.Location = new Point(25, 120);
            player.pbPlayer.Margin = new Padding(2);
            player.pbPlayer.Name = "pbPlayer";
            player.pbPlayer.Size = new Size(15, 15);
            player.pbPlayer.TabIndex = 1;
            player.pbPlayer.TabStop = false;
            Minimap.Controls.Add(player.pbPlayer);
            Control replacePic = Minimap.GetControlFromPosition(1, 8);
            replacePic.Dispose();
            Minimap.SetCellPosition(player.pbPlayer, new TableLayoutPanelCellPosition(1, 8));

        }
    }

    public class Tile
    {
        public enum Type : int { Wall = 1, Empty = 0 }
        public PictureBox TilePic = new();
    }
}

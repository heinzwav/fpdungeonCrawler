using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Formats.Asn1;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace fpdungeonCrawler
{
    public class Map
    {
        int SizeX = 10;
        int SizeY = 10;

        public Tile[,] mapTiles = new Tile[10, 10];
        public PictureBox[,] pictureboxes = new PictureBox[10, 10];
        public TableLayoutPanel minimap = new();

        TableLayoutPanelCellPosition[] PlayerViewTiles = new TableLayoutPanelCellPosition[9];


        public void GenerateMap()
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    Tile tile = new Tile();
                    tile.TilePositionX = i;
                    tile.TilePositionY = j;
                    tile.Type = Tile.TileType.Wall;
                    mapTiles[i, j] = tile;
                }

                GeneratePaths();
            }
        }

        //Minimap  
        public void GenerateMinimapUI()
        {
            minimap.AutoSize = true;
            minimap.ColumnCount = 10;
            minimap.RowCount = 10;
            minimap.Location = new System.Drawing.Point(25, 25);
            minimap.BackColor = Color.Black;
            minimap.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            minimap.AutoSize = true;
            minimap.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            foreach (Tile tile in mapTiles)
            {
                PictureBox PicBox = new();
                PicBox.Size = new Size(18, 18);
                minimap.Controls.Add(PicBox);
                minimap.SetCellPosition(PicBox, new TableLayoutPanelCellPosition(tile.TilePositionX, tile.TilePositionY));
                PicBox.BackgroundImageLayout = ImageLayout.Stretch;
                PicBox.Margin = new Padding(0);
                pictureboxes[tile.TilePositionX, tile.TilePositionY] = PicBox;
                //tile.Type = Tile.TileType.Wall;
                
                
                switch (tile.Type)
                    {
                        case Tile.TileType.Wall:
                            if (tile.entity == null) PicBox.BackgroundImage = Properties.Resources.WallTile;
                            break;

                        case Tile.TileType.Floor:
                            PicBox.BackgroundImage = Properties.Resources.FloorTile;
                            break;
                        default:
                            break;
                    }
                }
            }

        public void UpdateMinimapUI()
        {
            foreach (Tile tile in mapTiles)
            {
                PictureBox PicBox = pictureboxes[tile.TilePositionX, tile.TilePositionY];

                if (tile.entity == null)
                {
                    switch (tile.Type)
                    {
                        case Tile.TileType.Wall:
                            PicBox.BackgroundImage = Properties.Resources.WallTile;
                            break;

                        case Tile.TileType.Floor:
                            PicBox.BackgroundImage = Properties.Resources.FloorTile;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    PicBox.BackgroundImage = Properties.Resources.PlayerArrowNew;
                }

            }
        }


        public void GeneratePaths()
        {
            for (int i = 3; i < 9; i++)
            {
                GenerateFloor(1, i);
            }

            for (int i = 2; i < 8; i++)
            {
                GenerateFloor(i, 5);
            }
            for (int i = 4; i < 9; i++)
            {
                GenerateFloor(6, i);
            }
        }


        public void GenerateFloor(int col, int row)
        {
            Tile tile = new Tile();
            tile.TilePositionX = col;
            tile.TilePositionY = row;
            tile.Type = Tile.TileType.Floor;
            mapTiles[col, row] = tile;
        }

        public void GenerateEmptyTile(int col, int row)
        {
            Tile Empty = new Tile();
            Empty.PicBox.Size = new Size(15, 15);
            Empty.PicBox.BackColor = Color.AntiqueWhite;
            Empty.PicBox.Tag = "Empty";
            //minimap.Controls.Add(Empty.PicBox);
            //Minimap.SetCellPosition(Empty.PicBox, new TableLayoutPanelCellPosition(col, row));
        }

        //public Player InitializePlayer()
        //{
        //    Player player = new Player();
        //    player.pbPlayer.BackColor = SystemColors.ButtonHighlight;
        //    player.pbPlayer.BackgroundImage = Properties.Resources.PlayerArrowNew;
        //    player.pbPlayer.BackgroundImageLayout = ImageLayout.Stretch;
        //    player.pbPlayer.Location = new Point(25, 120);
        //    player.pbPlayer.Margin = new Padding(2);
        //    player.pbPlayer.Name = "pbPlayer";
        //    player.pbPlayer.Size = new Size(15, 15);
        //    player.pbPlayer.TabIndex = 1;
        //    player.pbPlayer.TabStop = false;
        //    Control replacePic = Minimap.GetControlFromPosition(1, 8);
        //    replacePic.Dispose();
        //    Minimap.SetCellPosition(player.pbPlayer, new TableLayoutPanelCellPosition(1, 8));

        //    return player;

        //}


    }
}

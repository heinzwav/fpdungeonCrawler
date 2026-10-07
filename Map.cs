using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Formats.Asn1;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Timers;
using static fpdungeonCrawler.Player;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace fpdungeonCrawler
{
    public class Map
    {
        public int SizeX = 10;
        public int SizeY = 10;

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
                    tile.isWall = true;
                    mapTiles[i, j] = tile;
                }                
            }
            GeneratePaths();
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

        //public void UpdateMinimapUI(Player player)
        //{
        //    foreach (Tile tile in mapTiles)
        //    {
        //        PictureBox PicBox = pictureboxes[tile.TilePositionX, tile.TilePositionY];

        //        if (tile.entity == null)
        //        {
        //            switch (tile.Type)
        //            {
        //                case Tile.TileType.Wall:
        //                    PicBox.BackgroundImage = Properties.Resources.WallTile;
        //                    break;

        //                case Tile.TileType.Floor:
        //                    PicBox.BackgroundImage = Properties.Resources.FloorTile;
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //        else
        //        {
        //            if(tile.entity == player)
        //            {
        //                PicBox.BackgroundImage = ChangePlayerArrowDirection(player);
        //            }
        //            else if (tile.entity.isEnemy)
        //            {
        //                PicBox.BackgroundImage = Properties.Resources.OrkIdle1;
        //            }

        //        }
        //    }
        //}

        public void UpdateMinimapUI(Player player)
        {
            player.PositionInFront = player.GetPositionInFront(player, 1);
            player.PositionBehind = player.GetPositionBehind(player);
            Tile[] tiles = new Tile[6];
            tiles[0] = mapTiles[player.PositionInFront.Column, player.PositionInFront.Row];
            tiles[1] = mapTiles[player.CurrentPosition.Column, player.CurrentPosition.Row];
            tiles[2] = mapTiles[player.PositionBehind.Column, player.PositionBehind.Row];
            tiles[3] = mapTiles[player.GetPositionInFront(player, 2).Column, player.GetPositionInFront(player, 2).Row];
            tiles[4] = mapTiles[player.GetPositionInFront(player, 3).Column, player.GetPositionInFront(player, 3).Row];
            tiles[5] = mapTiles[player.GetPositionInFront(player, 4).Column, player.GetPositionInFront(player, 4).Row];


            for (int i = 0; i < tiles.Length; i++)
            {
                PictureBox PicBox = pictureboxes[tiles[i].TilePositionX, tiles[i].TilePositionY];

                if (tiles[i].entity == null)
                {
                    switch (tiles[i].Type)
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
                    if (tiles[i].entity == player)
                    {
                        PicBox.BackgroundImage = ChangePlayerArrowDirection(player);
                    }
                    else if (tiles[i].entity.isEnemy)
                    {
                        PicBox.BackgroundImage = Properties.Resources.OrkIdle1;
                    }
                }
            }
        }

        public Image ChangePlayerArrowDirection(Player player)
        {
            Image bmPlayerArrow = Properties.Resources.PlayerArrowNew;

            switch (player.orientation)
            {
                case PlayerOrientation.Up:
                    break;

                case PlayerOrientation.Down:
                    bmPlayerArrow.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;

                case PlayerOrientation.Left:
                    bmPlayerArrow.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;

                case PlayerOrientation.Right:
                    bmPlayerArrow.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
            }
            return bmPlayerArrow;
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
            tile.isWall = false;
            mapTiles[col, row] = tile;
        }


        public void UpdateEntityLocation(Entity entity)
        {
            foreach (Tile tile in mapTiles)
            {
                if (tile.TilePositionX == entity.CurrentPosition.Column & tile.TilePositionY == entity.CurrentPosition.Row)
                {
                    tile.entity = entity;
                }
            }
        }


    }
}

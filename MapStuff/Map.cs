using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Formats.Asn1;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Timers;
using static fpdungeonCrawler.Player;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace fpdungeonCrawler.Map
{
    public class Map
    {
        public int SizeX = 10;
        public int SizeY = 10;

        public Tile[,] mapTiles = new Tile[10, 10];
        public PictureBox[,] pictureboxes = new PictureBox[10, 10];
        public TableLayoutPanel minimap = new();

        Tile[] playerViewRange = new Tile[6];
        private bool FirstStartUp = true;

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

        public void UpdateMinimapUI(Player player)
        {
            if (!FirstStartUp)
            {
                ResetPlayerViewRange();
            }
            FirstStartUp = false;

            player.PositionInFront = player.GetPositionInFront(player, 1);
            player.PositionBehind = player.GetPositionBehind(player);

            Tile tile;
            playerViewRange = new Tile[6];


            for (int j = -1; j < 5; j++)
            {
                if (player.GetPositionInFront(player, j).Column < 0 || player.GetPositionInFront(player, j).Column >= 9
                    || player.GetPositionInFront(player, j).Row < 0 || player.GetPositionInFront(player, j).Row >= 9)
                {
                    playerViewRange[j + 1] = new Tile();
                }
                else
                {
                    playerViewRange[j + 1] = mapTiles[player.GetPositionInFront(player, j).Column, player.GetPositionInFront(player, j).Row];
                }
            }

            for (int i = 0; i < playerViewRange.Length; i++)
            {
                PictureBox PicBox = pictureboxes[playerViewRange[i].TilePositionX, playerViewRange[i].TilePositionY];

                if (playerViewRange[i].entity == null)
                {
                    switch (playerViewRange[i].Type)
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
                    if (playerViewRange[i].player == player)
                    {
                        PicBox.BackgroundImage = ChangePlayerArrowDirection(player);
                    }
                    else if (playerViewRange[i].entity.isEnemy)
                    {
                        if (i < 5)
                        {
                            PicBox.BackgroundImage = playerViewRange[i].entity.mapIcon;
                        }
                        else
                        {
                            PicBox.BackgroundImage = Properties.Resources.FloorTile;
                        }
                    }
                }
            }
        }

        public void ResetPlayerViewRange()
        {
            for (int i = 0; i < playerViewRange.Length; i++)
            {
                PictureBox PicBox = pictureboxes[playerViewRange[i].TilePositionX, playerViewRange[i].TilePositionY];

                switch (playerViewRange[i].Type)
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
        }

        public Image ChangePlayerArrowDirection(Player player)
        {
            Image bmPlayerArrow = Properties.Resources.PlayerArrowNew;

            switch (player.orientation)
            {
                case Entity.EntityOrientation.Up:
                    break;

                case Entity.EntityOrientation.Down:
                    bmPlayerArrow.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;

                case Entity.EntityOrientation.Left:
                    bmPlayerArrow.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;

                case Entity.EntityOrientation.Right:
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
                if (tile.TilePositionX == entity.CurrentPosition.Column && tile.TilePositionY == entity.CurrentPosition.Row && entity is Player)
                {
                    tile.player = entity;
                    Debug.WriteLine(tile.player.CurrentPosition.ToString);
                }
                else if (tile.TilePositionX == entity.CurrentPosition.Column && tile.TilePositionY == entity.CurrentPosition.Row)
                {
                    tile.entity = entity;
                    Debug.WriteLine(tile.entity.CurrentPosition.ToString);
                }
            }
        }


    }
}

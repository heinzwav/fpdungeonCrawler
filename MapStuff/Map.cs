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

        private (int X, int Y) StartPosition = (1, 1);
        private bool FloorGenerated;

        Tile[] playerViewRange = new Tile[6];
        private bool FirstStartUp = true;

        TableLayoutPanelCellPosition[] PlayerViewTiles = new TableLayoutPanelCellPosition[9];


        //Generiert eine Map mit SizeX * SizeY die zunächst nur aus Walls besteht.
        //
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

        //Generiert Hardcoded Paths
        //
        //public void GeneratePaths()
        //{
        //    for (int i = 3; i < 9; i++)
        //    {
        //        GenerateFloor(1, i);
        //    }

        //    for (int i = 2; i < 8; i++)
        //    {
        //        GenerateFloor(i, 5);
        //    }
        //    for (int i = 4; i < 9; i++)
        //    {
        //        GenerateFloor(6, i);
        //    }
        //}


        //Procedural PathGeneration
        public void GeneratePaths()
        {
            int currentTileX = StartPosition.X;
            int currentTileY = StartPosition.Y;
            List<Tile> chosenTiles = new List<Tile>();

            while (!FloorGenerated)
            {
                List<Tile> tiles = new List<Tile>();
                GenerateFloor(currentTileX, currentTileY);

                //Check 2 Tiles above StartTile and Keep Dungeon Border as Wall
                if (currentTileY - 2 > 0)
                {
                    Tile tile = mapTiles[currentTileX, currentTileY - 2];

                    if (!tile.isVisited)
                    {
                        tiles.Add(tile);
                    }
                }

                //Check 2 Tiles beneath StartTile
                if (currentTileY + 2 < SizeY - 1)
                {
                    Tile tile = mapTiles[currentTileX, currentTileY + 2];

                    if (!tile.isVisited)
                    {
                        tiles.Add(tile);
                    }
                }

                //Check 2 Tiles left of StartTile

                if (currentTileX - 2 > 0)
                {
                    Tile tile = mapTiles[currentTileX - 2, currentTileY];

                    if (!tile.isVisited)
                    {
                        tiles.Add(tile);
                    }
                }

                //Check 2 Tiles right of StartTile

                if (currentTileX + 2 < SizeX - 1)
                {
                    Tile tile = mapTiles[currentTileX + 2, currentTileY];

                    if (!tile.isVisited)
                    {
                        tiles.Add(tile);
                    }
                }


                if (tiles.Count <= 0)
                {
                    if(chosenTiles.Count > 0)
                    {
                        chosenTiles.RemoveAt(chosenTiles.Count - 1);
                        currentTileX = chosenTiles[chosenTiles.Count - 1].TilePositionX;
                        currentTileY = chosenTiles[chosenTiles.Count - 1].TilePositionY;
                        continue;
                    }
                    else
                    {
                        FloorGenerated = true;
                        return;
                    }
                }
                int i = new Random().Next(tiles.Count);

                //Set chosen Tile as Floor Type
                Tile chosenTile = tiles[i];
                chosenTiles.Add(chosenTile);
                GenerateFloor(chosenTile.TilePositionX, chosenTile.TilePositionY);

                //Set Tile between StartTile and chosenTile as Floor Type
                if (chosenTile.TilePositionX < currentTileX)
                {
                    GenerateFloor(chosenTile.TilePositionX + 1, chosenTile.TilePositionY);
                }

                if (chosenTile.TilePositionX > currentTileX)
                {
                    GenerateFloor(chosenTile.TilePositionX - 1, chosenTile.TilePositionY);
                }

                if (chosenTile.TilePositionY < currentTileY)
                {
                    GenerateFloor(chosenTile.TilePositionX, chosenTile.TilePositionY + 1);
                }

                if (chosenTile.TilePositionY > currentTileY)
                {
                    GenerateFloor(chosenTile.TilePositionX, chosenTile.TilePositionY - 1);
                }

                currentTileX = chosenTiles[chosenTiles.Count -1].TilePositionX;
                currentTileY = chosenTiles[chosenTiles.Count - 1].TilePositionY;
            }

        }


        public void GenerateFloor(int col, int row)
        {
            Tile tile = new Tile();
            tile.TilePositionX = col;
            tile.TilePositionY = row;
            tile.Type = Tile.TileType.Floor;
            tile.isWall = false;
            tile.isVisited = true;
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

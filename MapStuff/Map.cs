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
                    if (playerViewRange[i].entity.isEnemy)
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
                if (playerViewRange[i].player == player)
                {
                    PicBox.BackgroundImage = ChangePlayerArrowDirection(player);
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


        //Procedural PathGeneration
        public void GeneratePaths()                     //Random Path/Floor Generation Method
        {
            int currentTileX = StartPosition.X;
            int currentTileY = StartPosition.Y;
            List<Tile> chosenTiles = new List<Tile>();

            while (!FloorGenerated)
            {
                List<Tile> validTiles = new List<Tile>();

                if (mapTiles[currentTileX, currentTileY].Type != Tile.TileType.Floor)
                {
                    GenerateFloor(currentTileX, currentTileY);
                }

                //Check 2 Tiles above StartTile and Keep Dungeon Border as Wall
                if (currentTileY - 2 > 0)
                {
                    Tile tile = mapTiles[currentTileX, currentTileY - 2];

                    if (!tile.isVisited)
                    {
                        validTiles.Add(tile);
                    }
                }

                //Check 2 Tiles beneath StartTile
                if (currentTileY + 2 < SizeY - 1)
                {
                    Tile tile = mapTiles[currentTileX, currentTileY + 2];

                    if (!tile.isVisited)
                    {
                        validTiles.Add(tile);
                    }
                }

                //Check 2 Tiles left of StartTile

                if (currentTileX - 2 > 0)
                {
                    Tile tile = mapTiles[currentTileX - 2, currentTileY];

                    if (!tile.isVisited)
                    {
                        validTiles.Add(tile);
                    }
                }

                //Check 2 Tiles right of StartTile

                if (currentTileX + 2 < SizeX - 1)
                {
                    Tile tile = mapTiles[currentTileX + 2, currentTileY];

                    if (!tile.isVisited)
                    {
                        validTiles.Add(tile);
                    }
                }

                //If no valid tile can be found, look at the last chosenTile that worked and try to find one there
                if (validTiles.Count <= 0)
                {
                    if (chosenTiles.Count > 0)
                    {
                        chosenTiles.RemoveAt(chosenTiles.Count - 1);
                        if (chosenTiles.Count == 0)
                        {
                            FloorGenerated = true;
                            return;
                        }
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
                int i = new Random().Next(validTiles.Count);

                //Choose a random tile from the valid tiles List, and add it to the chosenTiles list, then generate a Floor on this Tile
                Tile chosenTile = validTiles[i];
                chosenTile.isVisited = true;
                chosenTiles.Add(chosenTile);

                //Set Tile between StartTile and chosenTile as floor type
                Tile tileBetween = new();

                if (chosenTile.TilePositionX < currentTileX)
                {
                    tileBetween.TilePositionX = chosenTile.TilePositionX + 1;
                    tileBetween.TilePositionY = chosenTile.TilePositionY;
                }

                if (chosenTile.TilePositionX > currentTileX)
                {
                    tileBetween.TilePositionX = chosenTile.TilePositionX - 1;
                    tileBetween.TilePositionY = chosenTile.TilePositionY;
                }

                if (chosenTile.TilePositionY < currentTileY)
                {
                    tileBetween.TilePositionX = chosenTile.TilePositionX;
                    tileBetween.TilePositionY = chosenTile.TilePositionY + 1;
                }

                if (chosenTile.TilePositionY > currentTileY)
                {
                    tileBetween.TilePositionX = chosenTile.TilePositionX;
                    tileBetween.TilePositionY = chosenTile.TilePositionY - 1;
                }

                GenerateFloor(tileBetween.TilePositionX, tileBetween.TilePositionY);

                //Set chosenTile as currentTile so that it will be generated as Floor in next loop
                currentTileX = chosenTiles[chosenTiles.Count - 1].TilePositionX;
                currentTileY = chosenTiles[chosenTiles.Count - 1].TilePositionY;
            }
        }

        //Create a Tile of floor type
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


        //Get Area of the Map by entering Position & Range
        public List<Tile> CheckMapArea(int PositionX, int PositionY, int width, int height)
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = PositionY; i < PositionY + height; i++)
            {
                for (int j = PositionX; j < PositionX + width; j++)
                {
                    if (i < 0 || j < 0 || i > SizeX - 1 || j > SizeY - 1)
                    {
                        Debug.WriteLine("ERROR - Invalid Map Area Check");
                    }
                    else
                    {
                        tiles.Add(mapTiles[j, i]);
                    }
                }
            }
            return tiles;
        }

        public List<Tile> GetFloorTiles()
        {
            List<Tile> floortiles = new List<Tile>();
            foreach (Tile tile in mapTiles)
            {
                if (tile.Type == Tile.TileType.Floor)
                {
                    floortiles.Add(tile);
                }
            }
            return floortiles;
        }

        //Update the location of the entity
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


        //Spawn Enemies

        public void SpawnEnemies()
        {
            List<Tile> floortiles = GetFloorTiles();
            //Don't spawn Enemies near StartPoint of Player
            List<Tile> invalidTiles = CheckMapArea(0, 0, 3, 3);

            Random random = new Random();

            bool enemySpawnFinished = false;
            int maxEnemyNumber = 2;

            while (!enemySpawnFinished)
            {
                
                foreach (Tile t in invalidTiles)
                {
                    t.invalidFloorPosition = true;
                }

                if(invalidTiles.Count >= floortiles.Count)
                {
                    enemySpawnFinished = true;
                    break;
                }

                int rndNumber = random.Next(floortiles.Count);
                Tile randomTile = floortiles[rndNumber];

                if (!randomTile.invalidFloorPosition)
                {
                    rndNumber = random.Next(EnemyDatabase.EnemyCreators.Count);     //Generate random number based on Enemytypes
                    Entity newEnemy = EnemyDatabase.EnemyCreators[rndNumber](this, randomTile.TilePositionX, randomTile.TilePositionY);

                    //Add Tiles around Enemy to InvalidTiles List
                    List<Tile> newInvalidTiles = CheckMapArea(randomTile.TilePositionX - 1, randomTile.TilePositionY - 1, 3, 3);
                    
                    foreach(Tile t in newInvalidTiles)
                    {
                        if (!invalidTiles.Contains(t))
                        {
                            invalidTiles.Add(t);
                        }
                    }                    
                }
            }
        }

        public bool CheckIfInBounds(int Column, int Row)
        {
            if (Column > SizeX -1 || Column < 0 || Row > SizeY -1 || Row < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

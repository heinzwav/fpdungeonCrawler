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
        public int sizeX = 9;
        public int sizeY = 9;

        public Tile[,] mapTiles;
        public PictureBox[,] minimapIcons;
        public TableLayoutPanel minimap = new();

        private (int X, int Y) StartPosition = (1, 1);
        private (int X, int Y) EndPosition;
        private bool FloorGenerated;

        int playerViewRange = 6;
        public Tile[] playerSight = new Tile[6];       //Player can view the tile behind and the 4 tiles in front

        private bool FirstStartUp = true;


        public Map()
        {
            minimapIcons = new PictureBox[sizeX, sizeY];
        }

        
        //Generates a map of sizeX & sizeY purely of wall tiles
        //=> Generates floor tiles
        //=> Generates transition stairs to next level
        public void GenerateMap()
        {
            mapTiles = new Tile[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    Tile tile = new Tile();
                    tile.TilePositionX = i;
                    tile.TilePositionY = j;
                    tile.Type = Tile.TileType.Wall;
                    mapTiles[i, j] = tile;
                }
            }
            GeneratePaths();
            GenerateStairs();

        }

        
        //Minimap
        //Minimap is TableLayoutPanel with sizeX & sizeY
        //Minimap cells get icons from tile.Type of mapTiles
        public void GenerateMinimapUI()
        {
            minimap.AutoSize = true;
            minimap.ColumnCount = sizeX;
            minimap.RowCount = sizeY;
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
                minimapIcons[tile.TilePositionX, tile.TilePositionY] = PicBox;


                switch (tile.Type)
                {
                    case Tile.TileType.Wall:
                        PicBox.BackgroundImage = Properties.Resources.WallTile;
                        break;

                    case Tile.TileType.Floor:
                        PicBox.BackgroundImage = Properties.Resources.FloorTile;
                        break;
                    case Tile.TileType.Stairs:
                        PicBox.BackgroundImage = Properties.Resources.Stairs;
                        break;
                    default:
                        break;
                }
            }
        }


        public void GetPlayerSight(Player player)
        {
            for (int j = -1; j < playerViewRange -1; j++)
            {
                if (player.GetPositionInFront(player, j).Column < 0 || player.GetPositionInFront(player, j).Column >= sizeX - 1
                    || player.GetPositionInFront(player, j).Row < 0 || player.GetPositionInFront(player, j).Row >= sizeY - 1)
                {
                    playerSight[j + 1] = new Tile();
                }
                else
                {
                    playerSight[j + 1] = mapTiles[player.GetPositionInFront(player, j).Column, player.GetPositionInFront(player, j).Row];
                }
            }
        }


        public void UpdateMinimapUI(Player player)
        {
            if (!FirstStartUp)
            {
                ResetPlayerSightMinimap();
            }
            FirstStartUp = false;

            player.PositionInFront = player.GetPositionInFront(player, 1);
            player.PositionBehind = player.GetPositionBehind(player);

            Tile tile;
            GetPlayerSight(player);


            for (int i = 0; i < playerSight.Length; i++)
            {
                PictureBox PicBox = minimapIcons[playerSight[i].TilePositionX, playerSight[i].TilePositionY];

                if (playerSight[i].entity == null)
                {
                    switch (playerSight[i].Type)
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
                    if (playerSight[i].entity.isEnemy)
                    {
                        if (i < 5)
                        {
                            PicBox.BackgroundImage = playerSight[i].entity.mapIcon;
                        }
                        else
                        {
                            PicBox.BackgroundImage = Properties.Resources.FloorTile;
                        }
                    }
                }
                if (playerSight[i].player == player)
                {
                    PicBox.BackgroundImage = ChangePlayerIconDirection(player);
                }
            }
        }


        //Resets PlayerViewRange on Minimap
        //=> Minimap icons == mapTiles.tiles.Type
        public void ResetPlayerSightMinimap()
        {
            for (int i = 0; i < playerSight.Length; i++)
            {
                PictureBox PicBox = minimapIcons[playerSight[i].TilePositionX, playerSight[i].TilePositionY];

                switch (playerSight[i].Type)
                {
                    case Tile.TileType.Wall:
                        PicBox.BackgroundImage = Properties.Resources.WallTile;
                        break;

                    case Tile.TileType.Floor:
                        PicBox.BackgroundImage = Properties.Resources.FloorTile;
                        break;
                    
                    case Tile.TileType.Stairs:
                        PicBox.BackgroundImage = Properties.Resources.Stairs;
                        break;
                    default:
                        break;
                }
            }
        }


        //Changes rotation of player icon on Minimap
        public Image ChangePlayerIconDirection(Player player)
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



        public void GeneratePaths()                         //Procedural PathGeneration
        {
            int currentTileX = StartPosition.X;
            int currentTileY = StartPosition.Y;
            int distanceCounter = 0;
            int maxDistance = 0;

            Tile maxDistanceTile;

            List<Tile> chosenTiles = new List<Tile>();

            while (!FloorGenerated)
            {
                List<Tile> validTiles = new List<Tile>();

                if (mapTiles[currentTileX, currentTileY].Type != Tile.TileType.Floor)
                {
                    GenerateFloorTile(currentTileX, currentTileY);
                    distanceCounter++;
                    if (distanceCounter >= maxDistance)
                    {
                        maxDistance = distanceCounter;
                        EndPosition = (currentTileX, currentTileY);
                    }
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
                if (currentTileY + 2 < sizeY - 1)
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

                //Check 2 Tiles right of CurrentTile

                if (currentTileX + 2 < sizeX - 1)
                {
                    Tile tile = mapTiles[currentTileX + 2, currentTileY];

                    if (!tile.isVisited)
                    {
                        validTiles.Add(tile);
                    }


                }

                int[,] distance = new int[sizeX, sizeY];
                distance[StartPosition.X, StartPosition.Y] = 0;




                //If no valid tile can be found, look at the last chosenTile that worked and try to find one there
                if (validTiles.Count <= 0)
                {
                    chosenTiles.RemoveAt(chosenTiles.Count - 1);
                    if (chosenTiles.Count == 0)
                    {
                        FloorGenerated = true;
                        return;
                    }
                    currentTileX = chosenTiles[chosenTiles.Count - 1].TilePositionX;
                    currentTileY = chosenTiles[chosenTiles.Count - 1].TilePositionY;
                    distanceCounter--;

                    if (distanceCounter >= maxDistance)
                    {
                        maxDistance = distanceCounter;
                        EndPosition = (currentTileX, currentTileY);
                    }

                    continue;
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

                GenerateFloorTile(tileBetween.TilePositionX, tileBetween.TilePositionY);
                distanceCounter++;

                //Set chosenTile as currentTile so that it will be generated as Floor in next loop
                currentTileX = chosenTiles[chosenTiles.Count - 1].TilePositionX;
                currentTileY = chosenTiles[chosenTiles.Count - 1].TilePositionY;
            }
        }

        //Create a Tile of floor type
        public void GenerateFloorTile(int col, int row)
        {
            Tile tile = new Tile();
            tile.TilePositionX = col;
            tile.TilePositionY = row;
            tile.Type = Tile.TileType.Floor;
            tile.isVisited = true;
            mapTiles[col, row] = tile;
        }

        //Create Stairs to next Dungeon Level
        private void GenerateStairs()
        {
            Tile stairs = new();
            stairs.TilePositionX = EndPosition.X;
            stairs.TilePositionY = EndPosition.Y;
            stairs.Type = Tile.TileType.Stairs;
            mapTiles[EndPosition.X, EndPosition.Y] = stairs;
        }
        //Get Area of the Map by entering Position & Range
        public List<Tile> CheckMapArea(int PositionX, int PositionY, int width, int height)
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = PositionY; i < PositionY + height; i++)
            {
                for (int j = PositionX; j < PositionX + width; j++)
                {
                    if (i < 0 || j < 0 || i > sizeX - 1 || j > sizeY - 1)
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

                if (invalidTiles.Count >= floortiles.Count)
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

                    foreach (Tile t in newInvalidTiles)
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
            if (Column > sizeX - 1 || Column < 0 || Row > sizeY - 1 || Row < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        // Kevins Code
        //public Tile GetStairwayTile()
        //{
        //    int[,] distance = new int[SizeX, SizeY];
        //    for (int i = 0; i < SizeX; i++)
        //        for (int j = 0; j < SizeY; j++)
        //        {
        //            distance[i, j] = -1;
        //        }

        //    distance[StartPosition.X, StartPosition.Y] = 0;

        //    bool write = true;
        //    int number = 0;
        //    while (write)
        //    {
        //        write = false;
        //        for (int i = 0; i < SizeX; i++)
        //            for (int j = 0; j < SizeY; j++)
        //            {
        //                if (distance[i, j] == number)
        //                {
        //                    int neighbourX = i;
        //                    int neighbourY = j - 1;

        //                    if (CheckIfInBounds(neighbourX, neighbourY)
        //&& mapTiles[neighbourX, neighbourY].Type == Tile.TileType.Floor
        //&& distance[neighbourX, neighbourY] == -1)
        //                    {
        //                        distance[neighbourX, neighbourY] = number + 1;
        //                        write = true;
        //                    }

        //                    neighbourX = i;
        //                    neighbourY = j + 1;

        //                    if (CheckIfInBounds(neighbourX, neighbourY)
        //&& mapTiles[neighbourX, neighbourY].Type == Tile.TileType.Floor
        //&& distance[neighbourX, neighbourY] == -1)
        //                    {
        //                        distance[neighbourX, neighbourY] = number + 1;
        //                        write = true;
        //                    }

        //                    neighbourX = i + 1;
        //                    neighbourY = j;

        //                    if (CheckIfInBounds(neighbourX, neighbourY)
        //&& mapTiles[neighbourX, neighbourY].Type == Tile.TileType.Floor
        //&& distance[neighbourX, neighbourY] == -1)
        //                    {
        //                        distance[neighbourX, neighbourY] = number + 1;
        //                        write = true;
        //                    }

        //                    neighbourX = i - 1;
        //                    neighbourY = j;

        //                    if (CheckIfInBounds(neighbourX, neighbourY)
        //&& mapTiles[neighbourX, neighbourY].Type == Tile.TileType.Floor
        //&& distance[neighbourX, neighbourY] == -1)
        //                    {
        //                        distance[neighbourX, neighbourY] = number + 1;
        //                        write = true;
        //                    }

        //                }
        //            }
        //        number++;
        //    }

        //    int furthestX = StartPosition.X;
        //    int furthestY = StartPosition.Y;
        //    int biggestnumber = 0;

        //    for (int i = 0; i < SizeX; i++)
        //        for (int j = 0; j < SizeY; j++)
        //        {
        //            if (distance[i, j] > biggestnumber)
        //            {
        //                biggestnumber = distance[i, j];
        //                furthestX = i;
        //                furthestY = j;
        //            }
        //        }
        //    return mapTiles[furthestX, furthestY];
        //////}             

    }
}

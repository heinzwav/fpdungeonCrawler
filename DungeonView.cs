using fpdungeonCrawler.Map;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace fpdungeonCrawler
{
    public class DungeonView
    {
        public bool entityinSight;
        public Entity entityinFront;

        //Layer 1
        public static Layer layer1 = new();
        public Size layer1Size = new Size(240, 234);
        public Point layer1Location = new Point(280, 268);

        //Layer 2
        public static Layer layer2 = new();
        public Size layer2Size = new Size(480, 440);
        public Point layer2Location = new Point(175, 150);

        //Layer 3
        public static Layer layer3 = new();
        public Size layer3Size = new Size(820, 690);
        public Point layer3Location = new Point(0, 0);

        //Entity Layer
        public static Layer layerEntity = new();
        public Size layerEntitySize = new Size(430, 440);
        public Point layerEntityLocation = new Point(190, 150);

        //Player Layer
        public static Layer layerPlayer = new();
        public Size layerPlayerSize = new Size(800, 700);
        public Point layerPlayerLocation = new Point(0, 80);



        Layer[] layers = new Layer[3];
        Tile[] tilesLayer1 = new Tile[3];
        Tile[] tilesLayer2 = new Tile[3];
        Tile[] tilesLayer3 = new Tile[3];

        //Viewpoint Generation
        public void Initialize()
        {
            layer1 = layer1.InitializeLayer(layer1Size, layer1Location, Properties.Resources.Layer1_Straight);
            layer2 = layer2.InitializeLayer(layer2Size, layer2Location, Properties.Resources.Layer2_Straight);
            layer3 = layer3.InitializeLayer(layer3Size, layer3Location, Properties.Resources.Layer3_Straight);
            layerEntity = layerEntity.InitializeLayer(layerEntitySize, layerEntityLocation, Properties.Resources.Ork_Idle);
            layerPlayer = layerPlayer.InitializeLayer(layerPlayerSize, layerPlayerLocation, Properties.Resources.sword1_idle);
        }

        //Animations
        public DamageAnimation dmgAnimation = new();


        //Update Viewpoint
        private void CheckLayer1(Player player, fpdungeonCrawler.Map.Map map)
        {
            layer1.isVisible = true;
            GetPlayerView(player, map);
            for (int i = 0; i < 3; i++)
            {
                if (player.View[i].TilePositionX < 0 || player.View[i].TilePositionY < 0)
                {
                    tilesLayer1[i].isWall = true;
                    tilesLayer1[i].entity = null;
                }
                else
                {
                    tilesLayer1[i] = player.View[i];
                    if (tilesLayer1[i].Type == Tile.TileType.Wall)
                    {
                        tilesLayer1[i].isWall = true;
                    }
                    else
                    {
                        tilesLayer1[i].isWall = false;
                    }
                }
            }

            if (tilesLayer1[0].isWall == true && tilesLayer1[1].isWall == false && tilesLayer1[2].isWall == false)
            {
                layer1.img = Properties.Resources.Layer1_R;
            }
            else if (tilesLayer1[0].isWall == true && tilesLayer1[1].isWall == false && tilesLayer1[2].isWall == true)
            {
                layer1.img = Properties.Resources.Layer1_Straight;
            }
            else if (tilesLayer1[0].isWall == false && tilesLayer1[1].isWall == false && tilesLayer1[2].isWall == true)
            {
                layer1.img = Properties.Resources.Layer1_L;
            }
            else if (tilesLayer1[0].isWall == false && tilesLayer1[1].isWall == false && tilesLayer1[2].isWall == false)
            {
                layer1.img = Properties.Resources.Layer1_T;
            }
            else if (tilesLayer1[1].isWall == true)
            {
                layer1.img = Properties.Resources.Layer1_Wall;
            }
        }

        private void CheckLayer2(Player player, fpdungeonCrawler.Map.Map map)
        {
            layer1.isVisible = true;
            layer2.isVisible = true;
            GetPlayerView(player, map);
            for (int i = 3; i < 6; i++)
            {
                int j = i - 3;

                tilesLayer2[j] = player.View[i];
                if (tilesLayer2[j].Type == Tile.TileType.Wall)
                {
                    tilesLayer2[j].isWall = true;
                }
                else
                {
                    tilesLayer2[j].isWall = false;
                }
            }
            if (tilesLayer2[0].isWall == true && tilesLayer2[1].isWall == false && tilesLayer2[2].isWall == false)
            {
                layer2.img = Properties.Resources.Layer2_R;
            }
            else if (tilesLayer2[0].isWall == true && tilesLayer2[1].isWall == false && tilesLayer2[2].isWall == true)
            {
                layer2.img = Properties.Resources.Layer2_Straight;
            }
            else if (tilesLayer2[0].isWall == false && tilesLayer2[1].isWall == false && tilesLayer2[2].isWall == true)
            {
                layer2.img = Properties.Resources.Layer2_L;
            }
            else if (tilesLayer2[0].isWall == false && tilesLayer2[1].isWall == false && tilesLayer2[2].isWall == false)
            {
                layer2.img = Properties.Resources.Layer2_T;
            }
            else if (tilesLayer2[1].isWall == true)
            {
                layer2.img = Properties.Resources.Layer2_Wall;
                layer1.isVisible = false;
            }
        }

        private void CheckLayer3(Player player, fpdungeonCrawler.Map.Map map)
        {
            layer3.isVisible = true;
            GetPlayerView(player, map);
            for (int i = 6; i < 9; i++)
            {
                int j = i - 6;

                tilesLayer3[j] = player.View[i];

                if (tilesLayer3[j].Type == Tile.TileType.Wall)
                {
                    tilesLayer3[j].isWall = true;
                }
                else
                {
                    tilesLayer3[j].isWall = false;
                }
            }
            if (tilesLayer3[0].isWall == true && tilesLayer3[1].isWall == false && tilesLayer3[2].isWall == true)
            {
                layer3.img = Properties.Resources.Layer3_Straight;
            }
            else if (tilesLayer3[0].isWall == true && tilesLayer3[1].isWall == false && tilesLayer3[2].isWall == false)
            {
                layer3.img = Properties.Resources.Layer3_R;
            }
            else if (tilesLayer3[0].isWall == false && tilesLayer3[1].isWall == false && tilesLayer3[2].isWall == true)
            {
                layer3.img = Properties.Resources.Layer3_L;
            }
            else if (tilesLayer3[0].isWall == false && tilesLayer3[1].isWall == false && tilesLayer3[2].isWall == false)
            {
                layer3.img = Properties.Resources.Layer3_T;
            }
        }

        public void DrawEnvironment(Player player, fpdungeonCrawler.Map.Map map)
        {
            CheckLayer1(player, map);
            CheckLayer2(player, map);
            CheckLayer3(player, map);
            //Check3TilesInFront(player, map);
        }

        public void GetPlayerView(Player player, fpdungeonCrawler.Map.Map map)
        {
            if (player.orientation == Player.EntityOrientation.Up)
            {
                int k = 0;
                for (int i = player.CurrentPosition.Row - 2; i <= player.CurrentPosition.Row; i++)
                {
                    for (int j = player.CurrentPosition.Column - 1; j <= player.CurrentPosition.Column + 1; j++)
                    {
                        if (i < 0 || j < 0 || i > map.SizeX - 1 || j > map.SizeY - 1)
                        {
                            k++;
                        }
                        else
                        {
                            player.View[k] = map.mapTiles[j, i];
                            k++;
                        }
                    }
                }
            }

            else if (player.orientation == Player.EntityOrientation.Down)
            {
                int k = 0;
                for (int i = player.CurrentPosition.Row + 2; i >= player.CurrentPosition.Row; i--)
                {
                    for (int j = player.CurrentPosition.Column + 1; j >= player.CurrentPosition.Column - 1; j--)
                    {
                        if (i < 0 || j < 0 || i > 9 || j > 9)
                        {
                            k++;
                        }
                        else
                        {
                            player.View[k] = map.mapTiles[j, i];
                            k++;
                        }
                    }
                }
            }

            else if (player.orientation == Player.EntityOrientation.Left)
            {
                int k = 0;
                for (int i = player.CurrentPosition.Column - 2; i <= player.CurrentPosition.Column; i++)
                {
                    for (int j = player.CurrentPosition.Row + 1; j >= player.CurrentPosition.Row - 1; j--)
                    {
                        if (i < 0 || j < 0 || i > 9 || j > 9)
                        {
                            //entityinSight = false;
                            k++;
                        }
                        else
                        {
                            player.View[k] = map.mapTiles[i, j];
                            k++;
                        }
                    }
                }
            }


            else if (player.orientation == Player.EntityOrientation.Right)
            {
                int k = 0;
                for (int i = player.CurrentPosition.Column + 2; i >= player.CurrentPosition.Column; i--)
                {
                    for (int j = player.CurrentPosition.Row - 1; j <= player.CurrentPosition.Row + 1; j++)
                    {
                        if (i < 0 || j < 0 || i > 9 || j > 9)
                        {
                            k++;
                        }
                        else
                        {
                            player.View[k] = map.mapTiles[i, j];
                            k++;
                        }
                    }

                }
            }
        }
        public void PaintLayers(PaintEventArgs e, Player player, fpdungeonCrawler.Map.Map map)
        {
            entityinFront = GetAnyEnemyInFront(map, player);
            Entity entity = entityinFront;

            layers[0] = layer3;
            layers[1] = layer2;
            layers[2] = layer1;

            foreach (Layer layer in layers)
            {
                if (layer.isVisible)
                {
                    e.Graphics.DrawImage(layer.img, layer.rec);
                }
            }


            if (entityinSight)
            {
                layerEntity.img = entity.activeAnimation.GetCurrentFrameSprite(entity.activeAnimation.currentFrame);
                if (!entity.activeAnimation.isActive && entity.activeAnimation == entity.deathAnimation)
                {
                    layerEntity.img = entity.activeAnimation.frameSprites[entity.activeAnimation.maxFrames-1];
                }
                e.Graphics.DrawImage(layerEntity.img, layerEntity.rec);
            }
            else if (entityinSight == false)
            {
                layerEntity.rec.Size = new Size(0, 0);
                layerEntity.rec.Location = new Point(0, 0);
            }
            layerPlayer.img = player.attackAnimation.GetCurrentFrameSprite(player.attackAnimation.currentFrame);
            e.Graphics.DrawImage(layerPlayer.img, layerPlayer.rec);
            dmgAnimation.DoDamageAnimation(e.Graphics);
        }

        public void SetRecEntitySize(int distance)
        {
            switch (distance)
            {
                case 3:
                    layerEntity.rec.Size = new Size(120, 114);
                    layerEntity.rec.Location = new Point(335, 350);
                    break;
                case 2:
                    layerEntity.rec.Size = layer1.rec.Size;
                    layerEntity.rec.Location = layer1.rec.Location;
                    break;
                case 1:
                    layerEntity.rec.Size = layer2.rec.Size;
                    layerEntity.rec.Location = layer2.rec.Location;
                    break;
            }
        }

        //private void Check3TilesInFront(Player player, fpdungeonCrawler.Map.Map map)
        //{
        //    if (player.GetPositionInFront(player, 3).Column < 0 || player.GetPositionInFront(player, 3).Column > 9 ||
        //        player.GetPositionInFront(player, 3).Row < 0 || player.GetPositionInFront(player, 3).Row > 9)
        //    {
        //        return;
        //    }

        //    Tile tile = map.mapTiles[player.GetPositionInFront(player, 3).Column, player.GetPositionInFront(player, 3).Row];

        //    switch (tile.entity)
        //    {
        //        case null:
        //            break;
        //        case not null:
        //            SetRecEntitySize(3);
        //            entityinSight = true;
        //            entityinFront = tile.entity;
        //            break;
        //    }
        //}

        public Entity GetAnyEnemyInFront(fpdungeonCrawler.Map.Map map, Player player)
        {
            int viewDistance = 3;
            int closestEntityDistance = 0;
            Entity entity;

            for(int i = viewDistance; i >= 1; i--)
            {
                int X = player.GetPositionInFront(player, i).Column;
                int Y = player.GetPositionInFront(player, i).Row;

                if (map.CheckIfInBounds(X, Y))
                {
                    Tile tile = map.mapTiles[X, Y];
                    if (tile.entity != null)
                    {
                        closestEntityDistance = i;
                    }
                    if (tile.isWall || tile.Type == Tile.TileType.Wall)
                    {
                        closestEntityDistance = 0;
                    }
                }                
            }

            if (closestEntityDistance !> 0)
            {
                entity = map.mapTiles[player.GetPositionInFront(player, closestEntityDistance).Column, player.GetPositionInFront(player, closestEntityDistance).Row].entity;
                entityinSight = true;
                SetRecEntitySize(closestEntityDistance);
                return entity;
            }
            else
            {
                entity = null;
                entityinSight = false;
                return entity;
            }            
        }
    }
}

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


        private Dictionary<(int layer, int pattern), Image> layerImages = new()
        {
            { (1, 000), Properties.Resources.Layer1_T },
            { (1, 100), Properties.Resources.Layer1_L },
            { (1, 001), Properties.Resources.Layer1_R },
            { (1, 101), Properties.Resources.Layer1_Straight },
            { (1, 111), Properties.Resources.Layer1_Wall },

            { (2, 000), Properties.Resources.Layer2_T },
            { (2, 100), Properties.Resources.Layer2_L },
            { (2, 001), Properties.Resources.Layer2_R },
            { (2, 101), Properties.Resources.Layer2_Straight },
            { (2, 111), Properties.Resources.Layer2_Wall },

            { (3, 000), Properties.Resources.Layer3_T },
            { (3, 100), Properties.Resources.Layer3_L },
            { (3, 001), Properties.Resources.Layer3_R },
            { (3, 101), Properties.Resources.Layer3_Straight },
            { (3, 111), Properties.Resources.Layer3_Wall },
        };


        Layer[] layers = new Layer[3];
        Tile[] tilesLayer1 = new Tile[3];
        Tile[] tilesLayer2 = new Tile[3];
        Tile[] tilesLayer3 = new Tile[3];

        //Viewport Generation
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


        //Update Viewport || First Person View
        private void CheckLayer1(Player player, fpdungeonCrawler.Map.Map map)
        {
            GetPlayerView(player, map);
            for (int i = 0; i < 3; i++)
            {
                if (!map.CheckIfInBounds(player.View[i].TilePositionX, player.View[i].TilePositionY))
                {
                    i++;
                    continue;
                }
                else
                {
                    tilesLayer1[i] = player.View[i];
                }
            }
            bool leftWall = tilesLayer1[0].Type == Tile.TileType.Wall;
            bool frontWall = tilesLayer1[1].Type == Tile.TileType.Wall;
            bool rightWall = tilesLayer1[2].Type == Tile.TileType.Wall;

            if (frontWall)
            {
                layer1.img = layerImages[(1, 111)];
            }
            else if (leftWall && !frontWall && rightWall)
            {
                layer1.img = layerImages[(1, 101)];
            }
            else if (!leftWall && !frontWall && rightWall)
            {
                layer1.img = layerImages[(1, 001)];
            }
            else if (!leftWall && !frontWall && !rightWall)
            {
                layer1.img = layerImages[(1, 000)];
            }
            else if(leftWall && frontWall && !rightWall)
            {
                layer1.img = layerImages[(1, 110)];
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
            }
            bool leftWall = tilesLayer2[0].Type == Tile.TileType.Wall;
            bool frontWall = tilesLayer2[1].Type == Tile.TileType.Wall;
            bool rightWall = tilesLayer2[2].Type == Tile.TileType.Wall;

            if (leftWall && !frontWall && !rightWall)
            {
                layer2.img = layerImages[(2, 100)];
            }
            else if (leftWall && !frontWall && rightWall)
            {
                layer2.img = layerImages[(2, 101)];
            }
            else if (!leftWall && !frontWall && rightWall)
            {
                layer2.img = layerImages[(2, 001)];
            }
            else if (!leftWall && !frontWall && !rightWall)
            {
                layer2.img = layerImages[(2, 000)];
            }
            else if (frontWall)
            {
                layer2.img = layerImages[(2, 111)];
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
            }
            bool leftWall = tilesLayer3[0].Type == Tile.TileType.Wall;
            bool frontWall = tilesLayer3[1].Type == Tile.TileType.Wall;
            bool rightWall = tilesLayer3[2].Type == Tile.TileType.Wall;

            if (leftWall && !frontWall && rightWall)
            {
                layer3.img = layerImages[(3, 101)];
            }
            else if (leftWall && !frontWall && !rightWall)
            {
                layer3.img = layerImages[(3, 100)];
            }
            else if (!leftWall && ! frontWall && rightWall)
            {
                layer3.img = layerImages[(3, 001)];
            }
            else if (!leftWall && ! frontWall && !rightWall)
            {
                layer3.img = layerImages[(3, 000)];
            }
        }

        public void CheckViewportLayers(Player player, fpdungeonCrawler.Map.Map map)
        {
            CheckLayer1(player, map);
            CheckLayer2(player, map);
            CheckLayer3(player, map);
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
                        if (!map.CheckIfInBounds(i,j))
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
                        if (!map.CheckIfInBounds(i, j))
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
                        if (!map.CheckIfInBounds(i, j))
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


            else if (player.orientation == Player.EntityOrientation.Right)
            {
                int k = 0;
                for (int i = player.CurrentPosition.Column + 2; i >= player.CurrentPosition.Column; i--)
                {
                    for (int j = player.CurrentPosition.Row - 1; j <= player.CurrentPosition.Row + 1; j++)
                    {
                        if (!map.CheckIfInBounds(i, j))
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
                    layerEntity.img = entity.activeAnimation.frameSprites[entity.activeAnimation.maxFrames - 1];
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


        //Scale Viewport Layer for Entities
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

        public Entity GetAnyEnemyInFront(fpdungeonCrawler.Map.Map map, Player player)
        {
            int viewDistance = 3;
            int closestEntityDistance = 0;
            Entity entity;

            for (int i = viewDistance; i >= 1; i--)
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
                    if (tile.Type == Tile.TileType.Wall)
                    {
                        closestEntityDistance = 0;
                    }
                }
            }

            if (closestEntityDistance! > 0)
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

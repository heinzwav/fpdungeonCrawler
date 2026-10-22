using fpdungeonCrawler.Map;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static fpdungeonCrawler.Player;

namespace fpdungeonCrawler
{
    public class Entity
    {
        public enum EntityOrientation { Left, Up, Right, Down }
        public EntityOrientation orientation = EntityOrientation.Up;
        public TableLayoutPanelCellPosition currentPos;
        public int healthpoints { get; set; }
        public int damage { get; set; }

        public bool isEnemy { get; set; }

        public (int Column, int Row) CurrentPosition { get; set; }

        public (int Column, int Row) StartPosition { get; set; }

        public (int Column, int Row) PositionInFront { get; set; }

        public (int Column, int Row) PositionBehind { get; set; }

        public string name { get; set; }
        public Animation idleAnimation { get; set; }
        public Animation attackAnimation { get; set; }

        public Image mapIcon { get; set; }


        public void DoDamage(Entity actor, fpdungeonCrawler.Map.Map map, Audiomanager audiomanager)
        {
            actor.GetPositionInFront(actor, 1);
            Tile tile = map.mapTiles[actor.PositionInFront.Column, actor.PositionInFront.Row];
            if (tile?.entity != null)
            {
                Entity entity;
                entity = tile.entity;
                entity.healthpoints -= actor.damage;
                actor.attackAnimation.StartAnimation();

                audiomanager.PlayAudio(Properties.Resources.weaponWhoosh);
                

                if (entity.healthpoints <= 0)
                {
                    audiomanager.PlayAudio(Properties.Resources.SchattenFlieht);
                    tile.entity = null;
                }
            }
        }

        public void TakeDamage(int damage)
        {
            healthpoints -= damage;
        }


        public (int Column, int Row) GetPositionInFront(Entity entity, int distance)
        {
            switch (entity.orientation)
            {
                case EntityOrientation.Up:
                    entity.PositionBehind = (entity.CurrentPosition.Column, entity.CurrentPosition.Row - distance);
                    break;

                case EntityOrientation.Down:
                    entity.PositionBehind = (entity.CurrentPosition.Column, entity.CurrentPosition.Row + distance);
                    break;

                case EntityOrientation.Left:
                    entity.PositionBehind = (entity.CurrentPosition.Column - distance, entity.CurrentPosition.Row);
                    break;

                case EntityOrientation.Right:
                    entity.PositionBehind = (entity.CurrentPosition.Column + distance, entity.CurrentPosition.Row);
                    break;
            }
            return entity.PositionBehind;
        }

        public (int Column, int Row) GetPositionBehind(Entity entity)
        {
            switch (entity.orientation)
            {
                case EntityOrientation.Up:
                    entity.PositionBehind = (entity.CurrentPosition.Column, entity.CurrentPosition.Row + 1);
                    break;

                case EntityOrientation.Down:
                    entity.PositionBehind = (entity.CurrentPosition.Column, entity.CurrentPosition.Row - 1);
                    break;

                case EntityOrientation.Left:
                    entity.PositionBehind = (entity.CurrentPosition.Column + 1, entity.CurrentPosition.Row);
                    break;

                case EntityOrientation.Right:
                    entity.PositionBehind = (entity.CurrentPosition.Column - 1, entity.CurrentPosition.Row);
                    break;
            }
            return entity.PositionBehind;
        }


        //public void Attack(Entity entity)
        //{
        //    entity.PositionInFront = entity.GetPositionInFront(entity, 1);
        //    if (map.mapTiles[entity.PositionInFront.Column, entity.PositionInFront.Row].entity is null)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        audiomanager.PlayAudio(Properties.Resources.weaponWhoosh);
        //        entity.DoDamage(entity, map, audiomanager);
        //        dungeonView.dmgAnimation.StartAnimation();
        //        dungeonView.swordHit.StartAnimation();
        //    }
        //}
    
    }

}

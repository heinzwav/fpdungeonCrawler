using fpdungeonCrawler.Map;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace fpdungeonCrawler
{
    public class Entity
    {
        public TableLayoutPanelCellPosition currentPos;
        public int healthpoints { get; set; }
        public int damage { get; set; }

        public bool isEnemy { get; set; }

        public (int Column, int Row) CurrentPosition { get; set; }

        public (int Column, int Row) StartPosition { get; set; }

        public string name { get; set; }
        public Animation idleAnimation { get; set; }
        public Animation attackAnimation { get; set; }

        public Image mapIcon { get; set; }

        public void TakeDamage(int damage)
        {
            healthpoints -= damage;
        }

        public void DoDamage(int damage, Entity entity)
        {
            entity.healthpoints -= damage;
        }

        public void Attack(Entity entity)
        

        {
            entity.PositionInFront = player.GetPositionInFront(player, 1);
            if (map.mapTiles[player.PositionInFront.Column, player.PositionInFront.Row].entity is null)
            {
                return;
            }
            else
            {
                audiomanager.PlayAudio(Properties.Resources.weaponWhoosh);
                player.DoDamage(player, map, audiomanager);
                dungeonView.dmgAnimation.StartAnimation();
                dungeonView.swordHit.StartAnimation();
            }
        }
    
    }

}

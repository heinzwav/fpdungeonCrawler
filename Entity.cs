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

        public (int Column, int Row) CurrentPosition { get; set; }

        public (int Column, int Row) StartPosition { get; set; }

        public void TakeDamage(int damage)
        {
            healthpoints -= damage;
        }

        public void DoDamage(int damage, Entity entity)
        {
            entity.healthpoints -= damage;
        }


        //public void Move(Enum direction)
        //{
        //    if (direction = direction.Up)
        //        Entity.CurrentPosition.Column, player.CurrentPosition.Row + 1
        //}
    }

}

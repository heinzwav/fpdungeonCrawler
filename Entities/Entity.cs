using DungeonCrawlerMitHandy.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawlerMitHandy.Entities
{
    public class Entity
    {
        public string Name { get; }
        public int MaxHealth { get; }
        public int CurrentHealth { get; private set; }
        public int AttackDamage { get; private set; }
        public bool IsEnemy { get; }
        public Direction Facing { get; private set; }
        public GridPos StartPosition { get; }
        public bool IsDead => CurrentHealth == 0;

        public Entity(string name, int maxHealth, int attackDamage, bool isEnemy, 
            Direction facing, GridPos startPosition)
        {
            Name = name;
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
            AttackDamage = attackDamage;
            IsEnemy = isEnemy;
            Facing = facing;
            StartPosition = startPosition;
        }

        public void TakeDamage(int damage)
        {
            // Minus Damage würde die Entity heilen
            if (damage < 0) throw new ArgumentOutOfRangeException(nameof(damage));

            CurrentHealth = Math.Max(CurrentHealth - damage, 0);
        }

        public void TurnLeft()
        {
            Facing = Facing.TurnLeft();
        }

        public void TurnRight()
        {
            Facing = Facing.TurnRight();
        }
    }
}

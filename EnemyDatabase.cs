using fpdungeonCrawler.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler.Map
{
    public static class EnemyDatabase
    {
        public static List<Func<Map, int, int, Entity>> EnemyCreators = new()
        {
            CreateSuccubus,
            CreateOrc
        };
        //SUCCUBUS//
        public static Entity CreateSuccubus(Map map, int PositionX, int PositionY)
        {
            Entity succubus = new Entity()
            {
                CurrentPosition = (PositionX,PositionY),
                StartPosition = (PositionX,PositionY),
                name = "Succubus",
                healthpoints = 300,
                baseDamage = 4,
                isEnemy = true,
                mapIcon = Properties.Resources.Succubus_Idle1,
                idleAnimation = AnimationDatabase.succubusIdle,
                attackAnimation = AnimationDatabase.succubusAttack,
                deathAnimation = AnimationDatabase.succubusDeath,
                activeAnimation = AnimationDatabase.succubusIdle
            };
            map.UpdateEntityLocation(succubus);
            return succubus;
        }

        //ORC//
        public static Entity CreateOrc(Map map, int PositionX, int PositionY)
        {
            //Animations/DeathAnimation/Suc
            Entity orc = new Entity()
            {
                CurrentPosition = (PositionX, PositionY),
                StartPosition = (PositionX, PositionY),
                name = "orc",
                healthpoints = 300,
                baseDamage = 2,
                isEnemy = true,
                mapIcon = Properties.Resources.OrkIdle1,
                idleAnimation = AnimationDatabase.orcIdle,
                attackAnimation = AnimationDatabase.orcAttack,
                deathAnimation = AnimationDatabase.orcDeath,
                activeAnimation = AnimationDatabase.orcIdle
            };
            map.UpdateEntityLocation(orc);
            return orc;
        }
    }
}

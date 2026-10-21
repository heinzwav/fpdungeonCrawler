using fpdungeonCrawler.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public static class EnemyDatabase
    {
        //SUCCUBUS//
        public static Entity CreateSuccubus(fpdungeonCrawler.Map.Map map)
        {
            Entity succubus = new Entity()
            {
                CurrentPosition = (4,5),
                StartPosition = (4,5),
                name = "Succubus",
                healthpoints = 300,
                damage = 30,
                isEnemy = true,
                mapIcon = Properties.Resources.Succubus_Idle1,
                idleAnimation = new Animation()
                {
                    frameSprites = new Image[3]
                    {
                        Properties.Resources.Succubus_Idle1,
                        Properties.Resources.Succubus_Idle2,
                        Properties.Resources.Succubus_Idle3
                    },

                    interval = 16,
                    isActive = true,
                    maxFrames = 3,


                },
            };
            map.UpdateEntityLocation(succubus);
            return succubus;
        }

        //ORC//
        public static Entity CreateOrc(fpdungeonCrawler.Map.Map map)
        {
            Entity orc = new Entity()
            {
                CurrentPosition = (1, 4),
                StartPosition = (1, 4),
                name = "orc",
                healthpoints = 300,
                damage = 30,
                isEnemy = true,
                mapIcon = Properties.Resources.OrkIdle1,
                idleAnimation = new Animation()
                
                {
                    frameSprites = new Image[2]
                    {
                        Properties.Resources.OrkIdle1,
                        Properties.Resources.OrkIdle2
                    },

                    interval = 16,
                    isActive = true,
                    maxFrames = 2,
                },
            };
            map.UpdateEntityLocation(orc);
            return orc;
        }
    }
}

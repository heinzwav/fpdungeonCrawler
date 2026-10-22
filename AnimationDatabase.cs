using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public static class AnimationDatabase
    {

        //Player Animations
        public static Animation swordhit = new()
        {
            frameSprites = new Image[5]
                {
                    Properties.Resources.sword1_idle,
                    Properties.Resources.sword2_windup,
                    Properties.Resources.sword3_swing,
                    Properties.Resources.sword4_follow,
                    Properties.Resources.sword5_recovery,
                },
            maxFrames = 5,
            interval = 8,
            isActive = false
        };




        //Orc Animations
        public static Animation orcIdle = new()
        {
            frameSprites = new Image[2]
                    {
                        Properties.Resources.OrkIdle1,
                        Properties.Resources.OrkIdle2
                    },

            interval = 16,
            isActive = true,
            maxFrames = 2,
        };

        //Succubus Animations
        public static Animation idleSuccubus = new Animation()
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
        };
    }
}

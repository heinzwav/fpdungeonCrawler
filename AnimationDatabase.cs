using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
            interval = 1,
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
            loop = true,
            maxFrames = 2,
        };

        public static Animation orcAttack = new()
        {
            frameSprites = new Image[5]
            {
                        Properties.Resources.orc_1_idle,
                        Properties.Resources.orc_2_windup,
                        Properties.Resources.orc_3_swing,
                        Properties.Resources.orc_4_followthrough,
                        Properties.Resources.orc_5_recovery
            },

            interval = 1,
            isActive = false,
            maxFrames = 5,
        };

        public static Animation orcDeath = new()
        {
            frameSprites = new Image[6]
    {
                        Properties.Resources.orc_death_1_hit,
                        Properties.Resources.orc_death_2_stagger,
                        Properties.Resources.orc_death_3_fall,
                        Properties.Resources.orc_death_4_falling_back,
                        Properties.Resources.orc_death_5_airborne,
                        Properties.Resources.orc_death_6_lying
    },

            interval = 3,
            isActive = false,
            maxFrames = 6,
        };

        //Succubus Animations
        public static Animation succubusIdle = new Animation()
        {
            frameSprites = new Image[4]
            {
                Properties.Resources.Succubus_Idle1,
                Properties.Resources.Succubus_Idle2,
                Properties.Resources.Succubus_Idle3,
                Properties.Resources.Succubus_idle4
            },

            interval = 4,
            isActive = true,
            loop = true,
            maxFrames = 4,
        };

        public static Animation succubusAttack = new Animation()
        {
            frameSprites = new Image[4]
    {
                Properties.Resources.Succubus_attack1,
                Properties.Resources.Succubus_attack2,
                Properties.Resources.Succubus_attack3,
                Properties.Resources.Succubus_attack4
    },

            interval = 2,
            isActive = true,
            maxFrames = 4,
        };

        public static Animation succubusDeath = new Animation()
        {
            frameSprites = new Image[7]
{
                Properties.Resources.Succubus_death1,
                Properties.Resources.Succubus_death2,
                Properties.Resources.Succubus_death3,
                Properties.Resources.Succubus_death4,
                Properties.Resources.Succubus_death5,
                Properties.Resources.Succubus_death6,
                Properties.Resources.Succubus_death7,
},

            interval = 2,
            isActive = true,
            maxFrames = 7,
        };
    }
}

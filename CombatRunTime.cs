using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace fpdungeonCrawler
{
    public  class CombatRunTime
    {
        int currentTick = 0;
        public int CoolDowninMilliseconds = 2000;
        public int Interval = 45; //Wie viele Frames
        int maxRadius = 360;
        



        public void CombatTick(Form1 form1)
        {
            if(currentTick > Interval)
            {
                //PlayerHit

                if (form1.player.StateHit)
                {
                    form1.player.DoDamage(form1.player, form1.map, form1.audiomanager);
                }
                EnemyAttackCheck(form1);
                currentTick = 0;
                form1.player.StateHit = false;
            }
            else
            {
                currentTick++;
            }
        }


        public void CombatTickIndicator(PaintEventArgs e, Layer layer)
        {
            Brush brush = new SolidBrush(Color.White);
            e.Graphics.FillPie(brush, layer.rec, 0f, maxRadius/Interval * currentTick);
        }


        public void EnemyAttackCheck(Form1 form1)
        {
            if(form1.map.mapTiles[form1.player.PositionInFront.Column, form1.player.PositionInFront.Row].entity != null)
            {
                Entity entity = form1.map.mapTiles[form1.player.PositionInFront.Column, form1.player.PositionInFront.Row].entity!;
                if(entity.healthpoints > 0)
                {
                    entity.activeAnimation = entity.attackAnimation;
                    entity.activeAnimation.StartAnimation();
                    form1.player.healthpoints -= entity.baseDamage;
                }
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace fpdungeonCrawler
{
    public  class CombatRunTime
    {
        int currentTick = 0;
        public int CoolDowninMilliseconds = 2000;
        public int Interval = 45; //Wie viele Frames
        int currentRadius = 0;
        int maxRadius = 360;
        



        public void CombatTick(Form1 form1)
        {
            if(currentTick > Interval)
            {
                //PlayerHit

                if (form1.player.StateHit)
                {
                    form1.PlayerAttack();
                }

                currentTick = 0;
                
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
    }
}

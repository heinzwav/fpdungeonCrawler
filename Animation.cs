using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public class Animation
    {
        public System.Windows.Forms.Timer timer = new();
        int interval = 33;
        public int currentFrame = 0;
        public int maxFrames;
        public bool isActive;



        public System.Windows.Forms.Timer InitializeTimer()
        {
            timer.Interval = interval;
            timer.Enabled = true;
            timer.Start();
            return timer;
        }


        public void StartAnimation()
        {
            isActive = true;
            currentFrame = 0;
        }

        public void Update()
        {
            if (!isActive)
            { return; }

            currentFrame++;

            if(currentFrame >= maxFrames)
            {
                isActive = false;
            }
        }
    }
}

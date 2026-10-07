using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public class Animation
    {
        public int interval;
        public int currentTick = 0;
        public int currentFrame = 0;
        public int maxFrames;
        public bool isActive;
        public Image[] frameSprites;


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

        public void Loop()
        {
            if (!isActive)
            { return; }

            currentTick++;

            if(currentTick < interval)
            { return; }

            if (currentTick == interval * (currentFrame+1))
            {
                currentFrame++;
            }

            if (currentFrame >= maxFrames)
            {
                currentFrame = 0; 
                currentTick = 0;
            }
        }

        public Image GetCurrentFrameSprite(int currentframe)
        {
            return frameSprites[currentFrame];            
        }
    }
}

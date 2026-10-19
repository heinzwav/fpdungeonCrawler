using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public class DungeonView
    {
        //Layer 1
        public Layer layer1 = new();
        public Size layer1Size = new Size(240, 234);
        public Point layer1Location = new Point(280, 268);

        //Layer 2
        public Layer layer2 = new();
        public Size layer2Size = new Size(480, 440);
        public Point layer2Location = new Point(175, 150);

        //Layer 3
        public Layer layer3 = new();
        public Size layer3Size = new Size(820, 690);
        public Point layer3Location = new Point(0, 0);

        //Entity Layer
        public Layer layerEntity = new();
        public Size layerEntitySize = new Size(408, 406);
        public Point layerEntityLocation = new Point(190, 150);
    }
}

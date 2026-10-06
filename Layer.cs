using System;
using System.Collections.Generic;
using System.Text;

namespace fpdungeonCrawler
{
    public class Layer
    {
        public bool isVisible = true;
        public Rectangle rec;

        public Rectangle CreateRec(Size size, Point point)
        {
            Rectangle rec = new();
            rec.Size = new Size(224, 234);
            rec.Location = new Point(288, 268);
            return rec;
        }

        public Layer InitializeLayer(Size size, Point point)
        {
            Layer layer = new Layer();
            layer.rec = CreateRec(size, point);
            return layer;
        }
    }


}

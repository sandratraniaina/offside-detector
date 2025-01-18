using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace offside_detector.Models
{

    public class Goal
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Goal() { }

        public Goal(int positionX, int positionY, int width, int height)
        {
            PositionX = positionX;
            PositionY = positionY;
            Width = width;
            Height = height;
        }

        public bool IsBallInGoal(Point ballPosition)
        {
            return ballPosition.X >= PositionX &&
                   ballPosition.X <= PositionX + Width &&
                   ballPosition.Y >= PositionY &&
                   ballPosition.Y <= PositionY + Height;
        }
    }
}
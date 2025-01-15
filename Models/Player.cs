using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace offside_checker.Models
{
    public class Player
    {
        private Point point;
        private Double radius;
        private bool isOffside;

        public double Radius { get => radius; set => radius = value; }
        public Point Point { get => point; set => point = value; }
        public bool IsOffside { get => isOffside; set => isOffside = value; }
    }
}

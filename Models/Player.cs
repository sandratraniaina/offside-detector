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
        private PlayerStatus playerStatus = PlayerStatus.NEUTRAL;

        public double Radius { get => radius; set => radius = value; }
        public Point Point { get => point; set => point = value; }
        public PlayerStatus PlayerStatus { get => playerStatus; set => playerStatus = value; }
    }

    public enum PlayerStatus
    {
        OFFSIDE, NORMAL, NEUTRAL
    }
}

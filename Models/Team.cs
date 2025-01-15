﻿using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace offside_checker.Models
{
    public class Team
    {
        private List<Player> _players;
        private Bgr minColor;
        private Bgr maxColor;
        private Player goalKeeper;
        private Player playerWithBall;

        public List<Player> Players { get => _players; set => _players = value; }
        public Player GoalKeeper { get => goalKeeper; set => goalKeeper = value; }
        public Player PlayerWithBall { get => playerWithBall; set => playerWithBall = value; }
        public Bgr MinColor { get => minColor; set => minColor = value; }
        public Bgr MaxColor { get => maxColor; set => maxColor = value; }
    }
}

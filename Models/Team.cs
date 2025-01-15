using Emgu.CV.Structure;
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
        private Bgr color;
        private Player goalKeeper;
        private Player playerWithBall;

        public List<Player> Players { get => _players; set => _players = value; }
        public Bgr Color { get => color; set => color = value; }
        public Player GoalKeeper { get => goalKeeper; set => goalKeeper = value; }
        public Player PlayerWithBall { get => playerWithBall; set => playerWithBall = value; }
    }
}

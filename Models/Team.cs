using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace offside_detector.Models
{
    public class Team
    {
        private List<Player> _players;
        private Hsv minColor;
        private Hsv maxColor;
        private Player goalKeeper;
        private Player playerWithBall;
        private Player lastDefender;
        private Goal goal;
        private AttackDirection direction;
        public bool IsAttackRight;

        public List<Player> Players { get => _players; set => _players = value; }
        public Player GoalKeeper { get => goalKeeper; set => goalKeeper = value; }
        public Player PlayerWithBall { get => playerWithBall; set => playerWithBall = value; }
        public Hsv MinColor { get => minColor; set => minColor = value; }
        public Hsv MaxColor { get => maxColor; set => maxColor = value; }
        public Player LastDefender { get => lastDefender; set => lastDefender = value; }
        public AttackDirection Direction { get => direction; set => direction = value; }
    }

    public enum AttackDirection
    {
        Left, Right
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace offside_checker.Models
{
    public class Team
    {
        public string Color { get; set; }  // "red" or "blue"
        public Point GoalPosition { get; set; }
        public bool HasBall { get; set; }
        public bool IsAttackingRight { get; set; }
        public Player Goalkeeper { get; set; }
    }
}

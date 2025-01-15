using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace offside_checker.Models
{
    public class Player
    {
        public Point Position { get; set; }
        public string Team { get; set; }  // "red" or "blue"
        public string Status { get; set; } // "HJ" for offside, "M" for between ball and last defense
        public bool IsGoalkeeper { get; set; }
    }
}

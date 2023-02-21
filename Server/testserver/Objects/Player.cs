using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testserver.Models
{
    public class Player
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Hp { get; set; }

        public int Id { get; set; }

     //   public string Direction { get; set; }

     //   public bool IsDead { get; set; } = false;

        
        /*
        private Dictionary<string, string> directionsRight = new Dictionary<string, string>
        {
            ["up"] = "right",
            ["right"] = "down",
            ["down"] = "left",
            ["left"] = "up"
        };

        private Dictionary<string, string> directionsLeft = new Dictionary<string, string>
        {
            ["up"] = "left",
            ["left"] = "down",
            ["down"] = "right",
            ["right"] = "up"
        };
        public void RotateLeft()
        {
            this.Direction = this.directionsLeft[this.Direction];
        }

        public void RotateRight()
        {
            this.Direction = this.directionsRight[this.Direction];
        }
        */
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testserver.Objects;

namespace testserver.Models
{
    public class Player
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Hp { get; set; }

        public int Id { get; set; }

        public int Points { get; set; } = 0;

        public int Wood { get; set; } = 0;
        public int Stone { get; set; } = 0;


        public List<Item> playertools = new List<Item>();

        public bool IsAlive { get; set; } = true;

    }
}


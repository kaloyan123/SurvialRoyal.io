using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testserver.Objects
{
    public class ImobileEntity
    {
        public int Id { get; set; }

        public int CreatorId { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Heigth { get; set; }

        public double Hp { get; set; }

        public string Type { get; set; }

        public bool IsAlive { get; set; } = true;
    }
}

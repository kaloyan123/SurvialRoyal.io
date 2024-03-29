﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testserver.Objects
{
    public class MobileEntity
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Heigth { get; set; }

        public double Hp { get; set; }

        public int Id { get; set; }

        public string Type { get; set; }

        public double DirectionX { get; set; } = 0;
        public double DirectionY { get; set; } = 0;
        public double Angle { get; set; } = 0;

        public bool IsAlive { get; set; } = true;

        public string AgrivatedBy { get; set; } = "";
        public int AgrivatedById { get; set; } = -1;

        public int EntityTiks { get; set; } = 0;
    }
}

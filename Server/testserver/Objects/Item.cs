using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testserver.Objects
{
    public class Item
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public int Tier { get; set; }
        //1 -- wood
        //2 -- stone
        //3 -- iron?
        public string Kind { get; set; }

        public int Copies { get; set; } = 1;
    }
}

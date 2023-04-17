using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testserver.Data
{
    public class PlayerAccount
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public int TimesWon { get; set; }

        public int TimesDied { get; set; }

        public int HighestScore { get; set; }
    }
}

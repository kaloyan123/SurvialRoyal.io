using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testserver.Models;

namespace testserver.Objects
{
    public class Map
    {
        // public bool IsDone { get; private set; } = false;

        // public string Winner { get; private set; } = null;

        public int tiks { get; set; } = 0;

        public List<Player> players { get; set; } = new List<Player>();

            public void CreatePlayer(int x, int y, int id)
        {
            Player player = new Player() { X = x, Y = y, Id = id };
            players.Add(player);
        }

        public void MovePlayer(int x,int y,int id)
        {
            players.ForEach(player =>
            {
                if (player.Id == id)
                {
                    player.X = x;
                    player.Y = y;
                }
            });
        }


        public void Update()
        {
            tiks++;
           // Console.WriteLine("working");
        }
    }
}

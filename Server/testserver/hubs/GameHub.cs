using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using testserver.Models;
using testserver.Objects;
using testserver.Services;

namespace testserver.hubs
{
    public class GameHub : Hub
    {
        static int playernumber = -1 ;

        CreateLoop loopCraete;

        public GameHub(CreateLoop loopCraete)
        {
            this.loopCraete = loopCraete;
        }

        public async Task UpdatePlayers(int x, int y, int id)
        {
            this.loopCraete.curMap?.MovePlayer(x, y, id);

          //  int? tiks = this.loopCraete.curMap?.tiks;

           // var obj = this.loopCraete.curMap?.players.FirstOrDefault(x => x.Id == id);

            await Clients.All.SendAsync("drawCharacters", x, y, id);
        }

        public async Task InitiatePlayers(int x,int y)
        {
            playernumber++;

            if (playernumber == 0)
            {
                this.loopCraete.Start();
            }

            this.loopCraete.curMap?.CreatePlayer(x, y, playernumber);

            await Clients.All.SendAsync("startCharacters", playernumber);
        }
        public async Task PlayerAttack(int x, int y, int id)
        {
            this.loopCraete.curMap?.CreatePlayer(x, y, id);

            await Clients.All.SendAsync("startCharacters", id);
        }

    }
}
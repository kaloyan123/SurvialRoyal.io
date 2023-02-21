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
        static int playernumber = -1;

        CreateLoop loopCraete;

        public GameHub(CreateLoop loopCraete)
        {
            this.loopCraete = loopCraete;
        }

        public async Task UpdatePlayers(int x, int y, int id)
        {
            this.loopCraete.curMap?.MovePlayer(x, y, id);

            //  int? tiks = this.loopCraete.curMap?.tiks;

            var obj = this.loopCraete.curMap?.players.FirstOrDefault(x => x.Id == id);

            double hp = 0;
            this.loopCraete.curMap?.players.ForEach(player =>
            {
                hp = player.Hp;
            });

            await Clients.All.SendAsync("drawCharacters", x, y, obj.Hp, id);
        }

        public async Task InitiatePlayers(int x, int y, int health)
        {
            playernumber++;

            if (playernumber == 0)
            {
                this.loopCraete.Start();
                this.loopCraete.curMap?.StartOfGame();
            }

            this.loopCraete.curMap?.CreatePlayer(x, y, health, playernumber);

            await Clients.All.SendAsync("startCharacters", playernumber);
        }
        public async Task PlayerAttack(int x, int y, int id)
        {
            this.loopCraete.curMap?.Attack(x, y, id);

            await Clients.All.SendAsync("atak", id);
        }

        public async Task GetImobileObj()
        {
            List<double> Xes = new List<double>();
            this.loopCraete.curMap?.imobileObjs.ForEach(imobileobj =>
            {
                double x = imobileobj.X;
                Xes.Add(x);
            });

            List<double> Yes = new List<double>();
            this.loopCraete.curMap?.imobileObjs.ForEach(imobileobj =>
            {
                double y = imobileobj.Y;
                Yes.Add(y);
            });

            List<double> Ids = new List<double>();
            this.loopCraete.curMap?.imobileObjs.ForEach(imobileobj =>
            {
                double id = imobileobj.Id;
                Ids.Add(id);
            });

            List<string> Types = new List<string>();
            this.loopCraete.curMap?.imobileObjs.ForEach(imobileobj =>
            {
                string type = imobileobj.Type;
                Types.Add(type);
            });

            await Clients.All.SendAsync("drawObjects", Xes, Yes, Ids, Types);
        }

        public async Task GetMobileEntity()
        {
            List<double> EntityX = new List<double>();
            this.loopCraete.curMap?.mobileEntities.ForEach(mobileEntity =>
            {
                double x = mobileEntity.X;
                EntityX.Add(x);
                // Console.WriteLine(x);
            });

            List<double> EntityY = new List<double>();
            this.loopCraete.curMap?.mobileEntities.ForEach(mobileEntity =>
            {
                double y = mobileEntity.Y;
                EntityY.Add(y);
            });

            List<double> EntityHp = new List<double>();
            this.loopCraete.curMap?.mobileEntities.ForEach(mobileEntity =>
            {
                double hp = mobileEntity.Hp;
                EntityHp.Add(hp);
            });

            List<double> EntityId = new List<double>();
            this.loopCraete.curMap?.mobileEntities.ForEach(mobileEntity =>
            {
                double id = mobileEntity.Id;
                EntityId.Add(id);
            });

            List<string> EntityTypes = new List<string>();
            this.loopCraete.curMap?.mobileEntities.ForEach(mobileEntity =>
            {
                string type = mobileEntity.Type;
                EntityTypes.Add(type);
            });
            /*
            foreach (var i in EntityX)
            {
                Console.WriteLine(i);
            }
            */
            //Console.WriteLine(EntityX);

            await Clients.All.SendAsync("drawEnteties", EntityX, EntityY, EntityHp, EntityId, EntityTypes);
        }

    }
}
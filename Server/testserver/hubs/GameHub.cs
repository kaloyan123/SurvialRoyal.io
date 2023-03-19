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

        public async Task UpdatePlayers(int x, int y, int id, double angle)
        {
            this.loopCraete.curMap?.MovePlayer(x, y, id);

            var obj = this.loopCraete.curMap?.players.FirstOrDefault(x => x.Id == id);

            List<double> points = new List<double>();
            this.loopCraete.curMap?.players.ForEach(player =>
            {
                double id = player.Points;
                points.Add(id);
            });

            await Clients.All.SendAsync("drawCharacters", x, y, obj.Hp, points, obj.Wood, obj.Stone, id, angle);
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
        public async Task PlayerAttack(int x, int y, int id, double angle)
        {
            this.loopCraete.curMap?.Attack(x, y, "player", id, angle);

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

            List<double> EntityAngles = new List<double>();
            this.loopCraete.curMap?.mobileEntities.ForEach(mobileEntity =>
            {
                double agle = mobileEntity.Angle;
                EntityAngles.Add(agle);
            });

            await Clients.All.SendAsync("drawEnteties", EntityX, EntityY, EntityHp, EntityId, EntityTypes, EntityAngles);
        }

    }
}
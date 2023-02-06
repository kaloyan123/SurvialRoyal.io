﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
        Player player = new Player();

        private IServiceProvider serviceProvider;

        Map curMap;

        Loop loop;

        public GameHub(IServiceProvider serviceProvider)
        {
            // Key - Socket Id (client), Value - Game Session
            /*
            this.sessions = new ConcurrentDictionary<string, GameSession>();
            this.loops = new ConcurrentDictionary<string, GameLoopService>();
            this.loopCancellationTokens = new ConcurrentDictionary<string, CancellationToken>();
            this.Waiting = new List<string>();
            this.PlayerUsernames = new ConcurrentDictionary<string, string>();
            this.OtherPlayer = new ConcurrentDictionary<string, string>();
            this.PlayerColor = new ConcurrentDictionary<string, string>();
            */
           // this.map = new Map ();

            this.serviceProvider = serviceProvider;
        }

        public async Task UpdatePlayers(int x, int y, int id)
        {
            //this.map?.MovePlayer(x, y, id);
           // int tik = curMap.tiks;

            await Clients.All.SendAsync("drawCharacters", x, y, id);
        }

        public async Task InitiatePlayers(int x,int y)
        {
            playernumber++;
            if (playernumber == 0)
            {
                Createloop();
            }

           // this.map?.MovePlayer(x, y, playernumber);

            await Clients.All.SendAsync("startCharacters", playernumber);
        }

        public void Createloop()
        {
            Map map = new Map();

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            Loop gameLoop = this.serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Loop>();

            // Loop gameLoop = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<Loop>();

            loop = gameLoop;
            this.curMap = map;

            // gameLoop.map = new Objects.Map();
            gameLoop.map = map;
            gameLoop.StartAsync(token).Wait();
        }


    }
}
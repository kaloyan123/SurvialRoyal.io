using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testserver.Data;
using testserver.Services;

namespace testserver.hubs
{
    public class IndexHub : Hub
    {
        private ApplicationDbContext dbContext;

        //private UserService userservice; , UserService userservice

        public IndexHub(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
          //  this.userservice = userservice;
        }
        
        public List<PlayerAccount> GetAllByName(string name)
        {
            return dbContext.Players
                .Where(p => p.Name == name)
                .ToList();

        }
        public PlayerAccount GetAllById(int id)
        {
            return dbContext.Players
                .FirstOrDefault(p => p.Id == id);

        }

        public async Task LogSmt()
        {

            await Clients.All.SendAsync("logtest");
        }


        public async Task Register(string name, string password)
        {
            PlayerAccount player = new PlayerAccount();
            player.Name = name;
            player.Password = password;
            player.TimesWon = 0;
            player.TimesDied = 0;
            player.HighestScore = 0;

            dbContext.Players.Add(player);
            dbContext.SaveChanges();


            await Clients.All.SendAsync("ReciveLogins", player.Id, player.Name);
        }

        public async Task Login(string name, string passowrd)
        {
            List<PlayerAccount> playerNames = GetAllByName(name);

            PlayerAccount loginPlayer = playerNames.FirstOrDefault(p => p.Password == passowrd);


            await Clients.All.SendAsync("ReciveLogins", loginPlayer.Id, loginPlayer.Name);
        }

        public async Task Account(int id)
        {
            PlayerAccount playerId = GetAllById(id);
            PlayerAccount playerName = GetAllById(id);
            PlayerAccount playerDied = GetAllById(id);
            PlayerAccount playerScore = GetAllById(id);


            await Clients.All.SendAsync("ReceiveValues", playerId.Id, playerName.Name, playerDied.TimesDied, playerScore.HighestScore);
        }
    }
}
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;  
namespace testserver.hubs
{
    public class GameHub : Hub
    {
        static int playernumber = -1 ;
        public async Task UpdatePlayers(int x, int y, int id)
        {
            await Clients.All.SendAsync("drawCharacters", x, y, id);
        }

        public async Task InitiatePlayers(int x, int y)
        {
            playernumber++;
            await Clients.All.SendAsync("startCharacters", x, y, playernumber);
            
            
        }
    }
}
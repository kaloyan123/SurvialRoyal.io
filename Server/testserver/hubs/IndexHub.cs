using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testserver.hubs
{
    public class IndexHub : Hub
    {
        public async Task LogSmt()
        {
            
            await Clients.All.SendAsync("logtest");
        }
    }
}

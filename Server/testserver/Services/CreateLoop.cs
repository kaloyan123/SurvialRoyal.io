using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using testserver.Objects;

namespace testserver.Services
{
    public class CreateLoop
    {
        private IServiceProvider serviceProvider;
        private Loop loop;
        public Map curMap;

        public CreateLoop(IServiceProvider serviceProvider)
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
            this.serviceProvider = serviceProvider;
        }



        public void Start()
        {
            Map map = new Map();

            CancellationTokenSource tokenSource = new CancellationTokenSource();
             CancellationToken token = tokenSource.Token;
            Loop gameLoop = this.serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Loop>();

            loop = gameLoop ;
            this.curMap = map;

            gameLoop.map = map;
            loop.map = curMap;

            gameLoop.StartAsync(token).Wait();
        }
    }
}

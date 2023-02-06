using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using testserver.Objects;

namespace testserver.Services
{
    public class Loop : ILoop
    {
        public Map map { get; set; }

        private Timer timer;

        private void Step(object state)
        {
            this.map?.Update();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.timer = new Timer(Step, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(17));

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.timer?.Dispose();
        }

        
    }
}

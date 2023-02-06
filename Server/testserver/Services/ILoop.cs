using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testserver.Services
{
    public interface ILoop : IHostedService, IDisposable
    {

    }
}

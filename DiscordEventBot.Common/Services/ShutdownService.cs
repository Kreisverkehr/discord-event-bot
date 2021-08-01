using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Services
{
    public class ShutdownService
    {
        public Func<CancellationToken, Task> Shutdown { get; set; }
    }
}

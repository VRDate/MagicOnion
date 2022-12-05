using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagicOnion;

namespace PerformanceTest.Shared
{
    public interface IPerfTestHub : IStreamingHub<IPerfTestHub, IPerfTestHubReceiver>
    {
        Task<int> CallMethodAsync(string arg1, int arg2);
        Task<(int StatusCode, byte[] Data)> CallMethodLargePayloadAsync(string arg1, int arg2, byte[] arg3);
    }

    public interface IPerfTestHubReceiver
    {}
}

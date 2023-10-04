using MagicOnion.Client.SourceGenerator.Tests.Verifiers;

namespace MagicOnion.Client.SourceGenerator.Tests;

public class GenerateRawStreamingTest
{
    [Fact]
    public async Task StreamingResult()
    {
        var source = """
        using System;
        using MessagePack;
        using MagicOnion;
        using System.Threading.Tasks;
        
        namespace TempProject
        {
            public interface IMyService : IService<IMyService>
            {
                Task<ClientStreamingResult<string, string>> ClientStreamingAsync();
                Task<ServerStreamingResult<string>> ServerStreamingAsync();
                Task<DuplexStreamingResult<string, string>> DuplexStreamingAsync();
            }
        }
        """;

        await MagicOnionSourceGeneratorVerifier.RunAsync(source);
    }
}

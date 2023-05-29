// See https://aka.ms/new-console-template for more information
using Grpc.Net.ClientFactory;
using ProtoBuf.Grpc.ClientFactory;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;

namespace GrpcGreeterClient;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string url = "https://localhost:7057";
        var httpHandler = new HttpClientHandler
        {
            // allow certificates
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        //using var channel = GrpcChannel.ForAddress(url,  new GrpcChannelOptions { HttpHandler = httpHandler });
        //var client = channel.CreateGrpcService<IGreeterService>();

        //var reply = await client.SayHelloAsync(
        //    new HelloRequest { Name = "GreeterClient" });

        //Console.WriteLine($"Greeting: {reply.Message}");
        //Console.WriteLine("Press any key to exit...");
        //Console.ReadKey();

        var services = new ServiceCollection();
        services.AddCodeFirstGrpcClient<IGreeterService>(o =>
        {
            o.Address = new Uri(url);
            
            o.ChannelOptionsActions.Add((opt) =>
            {
                opt.HttpHandler = httpHandler;
            });
        });

        var serviceProvider = services.BuildServiceProvider();

        var clientFactory = serviceProvider.GetRequiredService<GrpcClientFactory>();
        var client = clientFactory.CreateClient<IGreeterService>(nameof(IGreeterService));

        var name = client.GetType().FullName;
        Console.WriteLine("ProtoBuf.Grpc.Internal.Proxies.ClientBase." + name);

        var reply = await client.SayHelloAsync(
            new HelloRequest { Name = "GreeterClient" });

        Console.WriteLine($"Greeting: {reply.Message}");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
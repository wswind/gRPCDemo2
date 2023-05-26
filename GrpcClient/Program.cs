// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using Shared.Contracts;

namespace GrpcGreeterClient;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var httpHandler = new HttpClientHandler();  
        // Return `true` to allow certificates that are untrusted/invalid  
        httpHandler.ServerCertificateCustomValidationCallback =   
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;  

        using var channel = GrpcChannel.ForAddress("https://localhost:7057",  new GrpcChannelOptions { HttpHandler = httpHandler });
        var client = channel.CreateGrpcService<IGreeterService>();

        var reply = await client.SayHelloAsync(
            new HelloRequest { Name = "GreeterClient" });

        Console.WriteLine($"Greeting: {reply.Message}");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
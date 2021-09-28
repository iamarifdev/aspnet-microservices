// using System.Text;
// using System;
// using System.Text.Json;
// using Microsoft.Extensions.Configuration;
// using PlatformService.Dtos;
// using RabbitMQ.Client;

// namespace PlatformService.AsyncDataServices
// {
//   public class MessageBusClientOld : IMessageBusClient
//   {
//     private readonly IConfiguration _configuration;
//     private readonly IConnection _connection;
//     private readonly IModel _channel;

//     public MessageBusClientOld(IConfiguration configuration)
//     {
//       _configuration = configuration;
//       var factory = new ConnectionFactory
//       {
//         UserName = "arif",
//         Password = "nokia523324",
//         HostName = _configuration["RabbitMQHost"],
//         Port = int.Parse(_configuration["RabbitMQPort"])
//       };

//       try
//       {
//         _connection = factory.CreateConnection();
//         _channel = _connection.CreateModel();

//         _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
//         _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

//         Console.WriteLine("Connected to MessageBus");
//       }
//       catch (Exception ex)
//       {
//         Console.WriteLine($"--> Could not connect to message Bus: {ex.Message}");
//       }
//     }

//     public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
//     {
//       var message = JsonSerializer.Serialize(platformPublishDto);
//       if (_connection.IsOpen)
//       {
//         Console.WriteLine("RabbitMQ connection open, sending message...");
//         SendMessage(message);
//       }
//       else
//       {
//         Console.WriteLine("RabbitMQ connection closed, not sending!");
//       } 
//     }

//     private void SendMessage(string message)
//     {
//       var body = Encoding.UTF8.GetBytes(message);

//       _channel.BasicPublish(
//         exchange: "trigger",
//         routingKey: "",
//         basicProperties: null,
//         body: body
//       );

//       Console.WriteLine($"--> We have sent {message}");
//     }

//     public void Dispose()
//     {
//       System.Console.WriteLine("MessageBus Disposed");
//       if (_channel.IsOpen)
//       {
//         _channel.Close();
//         _connection.Close();
//       }
//     }

//     private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
//     {
//       Console.WriteLine("--> RabbitMQ Connection Shutdown");
//     }
//   }
// }
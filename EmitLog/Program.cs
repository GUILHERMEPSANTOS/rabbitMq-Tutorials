using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("logs", ExchangeType.Fanout);

var message = GetMessage(args);
var body = Encoding.ASCII.GetBytes(message);

channel.BasicPublish("logs", string.Empty, basicProperties: null, body: body);

Console.WriteLine($"Sent [x] {message}");
Console.WriteLine($"Press [Enter] to exit");

Console.ReadLine();

static string GetMessage(string[] args)
{
    return (args.Length > 0) ? string.Join(" ", args) : "info: Hello World!";
}
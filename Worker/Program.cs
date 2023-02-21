using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "hello",
                      durable: true,
                      exclusive: false,
                      autoDelete: false,
                      arguments: null);

    channel.BasicQos(prefetchCount:1,prefetchSize: 0, global: false);

    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var dots = message.Split('.').Length - 1;

        Thread.Sleep(dots * 1000);
        Console.WriteLine("[x] Received {0}");
        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    };

    channel.BasicConsume(queue: "hello",
                         autoAck: false,
                         consumer: consumer);

    Console.WriteLine("\n Press [enter] to exit");
    Console.ReadLine();
}
using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using RabbitMQ.Client.Events;

namespace Bus
{
    public class MessageBus : IMessageBus
    {
        private readonly string _queueName;
        private readonly string _exchangeName;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBus(string queueName, string exchangeName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _queueName = queueName;
            _exchangeName = exchangeName;

            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: string.Empty);
        }


        public void Publish<T>(T message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.ASCII.GetBytes(json);

            _channel.BasicPublish(exchange: _exchangeName, routingKey: string.Empty, body: body);
        }

        public void Consume<T>(Action<T> action)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));
                action(message);
            };

            _channel.BasicConsume(_queueName, true, consumer);
        }
    }
}
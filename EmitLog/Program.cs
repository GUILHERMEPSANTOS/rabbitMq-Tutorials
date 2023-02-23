using DomainObjects;
using Bus;

var log = new Log("Vish deu um erro!");

var messageBus = new MessageBus("queue_log", "exchange_log");

messageBus.Publish<Log>(log);

Console.WriteLine("Press [Enter]");
Console.ReadLine();


using DomainObjects;
using Bus;

var messageBus = new MessageBus("queue_log", "exchange_log");

messageBus.Consume<Log>(Console.WriteLine);

Console.WriteLine("Press [Enter]");
Console.ReadLine();





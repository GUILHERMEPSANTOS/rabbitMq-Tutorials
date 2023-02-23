namespace Bus
{
    public interface IMessageBus
    {
        void Consume<T>(Action<T> action);
        void Publish<T>(T message);
    }
}
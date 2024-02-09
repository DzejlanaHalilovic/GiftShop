namespace Micro.Sinhro.Gift.Persistance
{
    public interface IMessageBroker
    {
        void Publish<T>(T message);
    }
}

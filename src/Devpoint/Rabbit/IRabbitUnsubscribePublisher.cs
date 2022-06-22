namespace Rabbit;

public interface IRabbitUnsubscribePublisher
{
    void SendMessage(object obj);
    void SendMessage(string message);
}
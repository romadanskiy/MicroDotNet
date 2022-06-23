namespace Background.Rabbit;

public interface IRabbitPublisher
{
    void SendMessage(object obj);
    void SendMessage(string message);
}
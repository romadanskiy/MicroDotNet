namespace Services.Payments.Rabbit;

public interface IRabbitSubscribePublisher
{
    void SendMessage(object obj);
    void SendMessage(string message);
}
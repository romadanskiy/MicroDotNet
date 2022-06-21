using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Rabbit;

public class RabbitPublisher : IRabbitPublisher
{
    public void SendMessage(object obj)
    {
        var message = JsonSerializer.Serialize(obj);
        SendMessage(message);
    }

    public void SendMessage(string message)
    {
        // Не забудьте вынести значения "localhost" и "MyQueue"
        // в файл конфигурации
        var factory = new ConnectionFactory { HostName = "devpoint.rabbit", Port = 5672 };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare("devpoint.queue", false, false, false, null);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("", "devpoint.queue", null, body);
    }
}
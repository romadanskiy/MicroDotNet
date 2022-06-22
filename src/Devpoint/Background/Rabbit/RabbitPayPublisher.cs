using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Background.Rabbit;

public class RabbitPayPublisher : IRabbitPublisher
{
    private readonly string _host = Environment.GetEnvironmentVariable("RABBIT_HOST")!;
    private readonly int _port = int.Parse(Environment.GetEnvironmentVariable("RABBIT_PORT")!);
    private readonly string _queue = Environment.GetEnvironmentVariable("RABBIT_QUEUE_PAY")!;
    
    public void SendMessage(object obj)
    {
        var message = JsonSerializer.Serialize(obj);
        SendMessage(message);
    }

    public void SendMessage(string message)
    {
        var factory = new ConnectionFactory {HostName = _host, Port = _port};
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(_queue, false, false, false, null);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("", _queue, null, body);
    }
}
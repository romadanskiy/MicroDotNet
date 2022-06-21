using System.Text;
using System.Text.Json;
using Background.Jobs.PaySubscription;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Background.Rabbit;

public class RabbitConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly PaySubscriptionJob _paySubscriptionJob;

    public RabbitConsumer(PaySubscriptionJob paySubscriptionJob)
    {
        _paySubscriptionJob = paySubscriptionJob;

        // Не забудьте вынести значения "localhost" и "MyQueue"
        // в файл конфигурации
        var factory = new ConnectionFactory { HostName = "devpoint.rabbit", Port = 5672 };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare("devpoint.queue", false, false, false, null);
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());

            var dto = JsonSerializer.Deserialize<PaySubscriptionDto>(content);
            AddSubscription(dto!);

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume("devpoint.queue", false, consumer);

        return Task.CompletedTask;
    }
    
    private void AddSubscription(PaySubscriptionDto dto)
    {
        _paySubscriptionJob.AddSubscription(dto);
    }
    
    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        GC.SuppressFinalize(this);
    }
}
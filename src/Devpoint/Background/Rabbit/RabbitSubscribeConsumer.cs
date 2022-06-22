using System.Text;
using System.Text.Json;
using Background.Jobs.PaySubscription;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Background.Rabbit;

public class RabbitSubscribeConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly PaySubscriptionJob _paySubscriptionJob;

    private readonly string _host = Environment.GetEnvironmentVariable("RABBIT_HOST")!;
    private readonly int _port = int.Parse(Environment.GetEnvironmentVariable("RABBIT_PORT")!);
    private readonly string _queue = Environment.GetEnvironmentVariable("RABBIT_QUEUE_SUBSCRIBE")!;

    public RabbitSubscribeConsumer(PaySubscriptionJob paySubscriptionJob)
    {
        _paySubscriptionJob = paySubscriptionJob;

        var factory = new ConnectionFactory {HostName = _host, Port = _port};
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(_queue, false, false, false, null);
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());

            var record = JsonSerializer.Deserialize<SubscriptionRecord>(content)!;
            _paySubscriptionJob.AddSubscription(record);

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(_queue, false, consumer);

        return Task.CompletedTask;
    }
    
    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        GC.SuppressFinalize(this);
    }
}
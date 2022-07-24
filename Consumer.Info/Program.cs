using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };

var connection = factory.CreateConnection();

using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare("directs_logs", ExchangeType.Direct);
    var queueName = channel.QueueDeclare().QueueName;

    channel.QueueBind(queueName, "directs_logs", "info");

    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (sender, args) =>
    {
        var body = args.Body;
        var message = Encoding.UTF8.GetString(body.ToArray());
        Console.WriteLine($"Получено сообщение:{message}");
    };
    channel.BasicConsume(queueName, true, consumer);
    Console.WriteLine($"подписались на очередь {queueName}");
    Console.ReadLine();
}
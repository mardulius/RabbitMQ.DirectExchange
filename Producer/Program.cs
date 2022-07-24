// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


Task.Run(CreateTask(12000, "error"));
Task.Run(CreateTask(8000, "info"));
Task.Run(CreateTask(15000, "warning"));


Console.ReadLine();



static Func<Task> CreateTask(int timeToSleepTo, string routingKey)
{
    return () =>
    {

        var counter = 0;

        do
        {
            int timeToSleep = new Random().Next(1000, timeToSleepTo);
            Thread.Sleep(timeToSleep);

            var factory = new ConnectionFactory() { HostName = "localhost" };

            var connection = factory.CreateConnection();

            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("directs_logs", ExchangeType.Direct);
                string message = $"здороува мужики,{routingKey} произошла ошибка! сообщение[{counter}]";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(
                    exchange: "directs_logs",
                    routingKey: routingKey,
                    basicProperties: null,
                    body: body);

                Console.WriteLine($"Сообщение [{counter++}] типа:{routingKey} - отправлено");
            }
        }
        while (true);

    };

    
}
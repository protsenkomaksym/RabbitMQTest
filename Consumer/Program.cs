using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory connectionFactory = new ConnectionFactory();
connectionFactory.HostName = "localhost";

IConnection connection = connectionFactory.CreateConnection();
IModel model = connection.CreateModel();

model.QueueDeclare(queue: "testQueue",
                 durable: false,
                 exclusive: false,
                 autoDelete: false,
                 arguments: null);

// Read queue body
var consumer = new EventingBasicConsumer(model);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
};

model.BasicConsume(queue: "testQueue",
                     autoAck: true,
                     consumer: consumer);

Console.ReadKey();
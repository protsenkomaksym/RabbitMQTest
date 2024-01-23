using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

bool sender = false;
bool receiver = true;

if (sender)
{
    ConnectionFactory connectionFactory = new ConnectionFactory();
    connectionFactory.HostName = "localhost";

    IConnection connection = connectionFactory.CreateConnection();
    IModel model = connection.CreateModel();

    string exchange = "testE";
    string routingKey = "testK";
    string queue = "testQ";

    // Create excchange
    model.ExchangeDeclare(exchange, ExchangeType.Direct);

    // Create queue
    model.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

    // Bind queue-key
    model.QueueBind(queue, exchange, routingKey, arguments: null);

    // Messege body
    string msg = "Hello";
    model.BasicPublish(exchange, routingKey, basicProperties: null, Encoding.UTF8.GetBytes(msg));

    // Close connection
    model.Close();
    connection.Close();
}

// receiver
if (receiver)
{
    ConnectionFactory connectionFactory = new ConnectionFactory();
    connectionFactory.HostName = "localhost";

    IConnection connection = connectionFactory.CreateConnection();
    IModel model = connection.CreateModel();

    string exchange = "testE";
    string routingKey = "testK";
    string queue = "testQ";

    model.QueueDeclare(queue: "testQ",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
    // Create excchange
    ///model.ExchangeDeclare(exchange, ExchangeType.Direct);

    // Create queue
    //model.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

    // Bind queue-key
    //model.QueueBind(queue, exchange, routingKey, arguments: null);

    // Read queue body
    var consumer = new EventingBasicConsumer(model);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($" [x] Received {message}");
    };

    model.BasicConsume(queue: "testQ",
                         autoAck: true,
                         consumer: consumer);

    // Close connection
    //model.Close();
    //connection.Close();
}

Console.ReadKey();
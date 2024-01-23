using RabbitMQ.Client;
using System.Text;

ConnectionFactory connectionFactory = new ConnectionFactory();
connectionFactory.HostName = "localhost";

IConnection connection = connectionFactory.CreateConnection();
IModel model = connection.CreateModel();

string exchange = "testExchange";
string routingKey = "testKey";
string queue = "testQueue";

// Create exchange
model.ExchangeDeclare(exchange, ExchangeType.Direct);

// Create queue
model.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

// Bind rounting queue-key
model.QueueBind(queue, exchange, routingKey, arguments: null);

// Send messages to the queue
for (int i = 0; i <= 5; i++)
{
    string msg = $"Msg #{i}";
    model.BasicPublish(exchange, routingKey, basicProperties: null, Encoding.UTF8.GetBytes(msg));
    Console.WriteLine(msg);

    // Wait 2sec
    Thread.Sleep(2000);
}

// Close connection
model.Close();
connection.Close();

Console.ReadKey();
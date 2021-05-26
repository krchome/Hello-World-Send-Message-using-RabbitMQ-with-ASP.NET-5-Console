using System;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Send
{ 
        class Send
        {
            public static void Main()
            {
                IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfiguration configuration = configurationBuilder.Build();

            var factory = new ConnectionFactory()
            {
                HostName = configuration.GetSection("AppConfig").GetSection("SmtpOptions").GetSection("Host").Value,
                UserName = configuration.GetSection("AppConfig").GetSection("SmtpOptions").GetSection("Username").Value,
                Password = configuration.GetSection("AppConfig").GetSection("SmtpOptions").GetSection("Password").Value,
                // VirtualHost = configuration.GetSection("AppConfig").GetSection("SmtpOptions").GetSection("RabbitMQHost").Value,
                Port = Convert.ToInt32(configuration.GetSection("AppConfig").GetSection("SmtpOptions").GetSection("Port").Value)
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    string message = "Hello World Program From RabbitMQ Site!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();


        }
        }
    }

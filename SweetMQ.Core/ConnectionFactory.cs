using System;
using RabbitMQ.Client;

namespace SweetMQ.Core
{
    public class ConnectionFactory : IDisposable
    {
        public ConnectionFactory(
            string hostName = "localhost",
            string password = "guest",
            int port = 5672,
            string userName = "guest",
            string virtualHost = "/"
        )
        {
            var factory = new RabbitMQ.Client.ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                VirtualHost = virtualHost,
                UserName = userName,
                Password = password
            };

            Connection = factory.CreateConnection();


            AppDomain.CurrentDomain.ProcessExit += (sender, args) => Dispose();
            Console.CancelKeyPress += (sender, args) =>
            {
                Dispose();
                args.Cancel = true;
            };
        }

        public IConnection Connection { get; }

        public void Dispose()
        {
            Connection?.Close();
            Connection?.Dispose();
        }
    }
}
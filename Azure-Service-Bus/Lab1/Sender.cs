using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Azure.Messaging.ServiceBus;

namespace Lab1
{
    public class Sender
    {
        const string connectionString = "Primary-connection-string";
        const string queueName = "basic-queue";

        public static async Task SendMessage()
        {
            ServiceBusClient client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(queueName);

            string messageBody = "Hello, Azure Service Bus!";
            ServiceBusMessage message = new ServiceBusMessage(messageBody);

            await sender.SendMessageAsync(message);

            Console.WriteLine($"Sent a message: {messageBody}");
        }
    }
}
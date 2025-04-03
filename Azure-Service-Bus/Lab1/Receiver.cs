using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Azure.Messaging.ServiceBus;

namespace Lab1
{
    public class Receiver
    {
        const string connectionString = "Primary-connection-string";
        const string queueName = "basic-queue";

        public static async Task ReceiveMessage()
        {
            ServiceBusClient client = new ServiceBusClient(connectionString);
            ServiceBusProcessor processor = client.CreateProcessor(queueName);

            processor.ProcessMessageAsync += async args =>
            {
                string body = args.Message.Body.ToString();
                Console.WriteLine($"Received a message: {body}");
                await args.CompleteMessageAsync(args.Message);
            };

            processor.ProcessErrorAsync += args =>
            {
                Console.WriteLine($"Error: {args.Exception.Message}");
                return Task.CompletedTask;
            };

            await processor.StartProcessingAsync();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            await processor.StopProcessingAsync();
        }
    }
}
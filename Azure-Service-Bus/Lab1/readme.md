# Basic Lab - Azure Service Bus

## Objectives

- Learn how to create and manage an Azure Service Bus Queue
- Send and receive messages from a queue using a C# application

## Lab

### Step 1: Step up the Azure Env

- Sign in to Azure Portal
- Search for "Service Bus" and Create
- Fill in the details:
	- ![[Pasted image 20250403184101.png]]
- ![[Pasted image 20250403184235.png]]

### Step 2: Create a Queue

- Go to the resource and create Queue
- ![[Pasted image 20250403184340.png]]
- ![[Pasted image 20250403184432.png]]

### Step 3: Write a C# App to Send and Receive Msg

#### Set up the Connection:

- Go to Shared Access Policies > RootManageSharedAccessKey
	- ![[Pasted image 20250403184558.png]]
- Copy the value of Primary Connection String

#### Install the SDK

- Use VS Code and create a console app project
```console
dotnet new console -n Lab1
cd Lab1
```

- Install Azure Service Bus SDK
```console
dotnet add package Azure.Messaging.ServiceBus
```

- Write code for app:
- Sender.cs
```c#
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
```

- Receiver.cs:
```c#
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
```

- Program.cs
```c#
using System;
using System.Threading.Tasks;

using Lab1;

class Program{
   static async Task Main(string[] args){
      if(args.Length==0){
         Console.WriteLine("Please enter the parameter: send or receive");
         return;
      }
      if(args[0] == "send"){
         await Sender.SendMessage();
      } else if (args[0] == "receive"){
         await Receiver.ReceiveMessage();
      } else {
         Console.WriteLine("Parameter is not valid. Use send or receive");
      }
   }
}
```

### Step 4: Run the app

- Run the sender with command:
```cli
dotnet run -- send
```
- ASB will set the msg to queue:
	- ![[Pasted image 20250403201050.png]]

- Run the receiver with command:
```cli
dotnet run --receive
```
- ASB will push msg out of queue to the receiver:
	- ![[Pasted image 20250403201257.png]]

- Here is the result on cli
![[Pasted image 20250403201207.png]]


# References
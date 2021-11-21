using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace L06
{
    class Program
    {
        // azure storage connection json
        const string ServiceBusConnectionString = "";
        const string QueueName = "coada";
        static IQueueClient queueClient;

        public static async Task Main(string[] args)
        {   
            //start api
            CreateHostBuilder(args).Build().Run();

            const int numberOfMessages = 10;
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            await SendMessagesAsync(numberOfMessages);
            Console.ReadKey();
            await queueClient.CloseAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    string messageBody = $"Hello world: {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    Console.WriteLine($"Sending message: {messageBody}");

                    await queueClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}

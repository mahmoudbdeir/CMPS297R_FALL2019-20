using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Queue; // Namespace for Queue storage types
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CMPS297R.AzureQueue
{
    class Program
    {
        static CloudStorageAccount storageAccount;
        static CloudQueueClient queueClient;
        static CloudQueue queue;
        static void WriteObject()
        {
            do
            {
                Console.WriteLine("Enter firstname lastname");
                var name = Console.ReadLine();
                var a = name.Split(' ');
                var firstname = a[0];
                var lastname = a[1];

                Person p = new Person() { FirstName = firstname, LastName = lastname };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                CloudQueueMessage message = new CloudQueueMessage(json);
                queue.AddMessageAsync(message);
            } while (true);
        }
        static async void ReadObject()
        {
            do
            {

                CloudQueueMessage peekedMessage = await queue.PeekMessageAsync();
                if (peekedMessage == null)
                {
                    Console.WriteLine("\tSleepig for 3");
                    Thread.Sleep(3000);
                    continue;
                }
                Console.WriteLine(peekedMessage.AsString);
                var message = await queue.GetMessageAsync();
                while (message != null)
                {
                    Person person = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(message.AsString);
                    Console.WriteLine($"{person.FirstName},{person.LastName}");
                    //message.SetMessageContent("Updated contents.");
                    //queue.UpdateMessage(message,
                    //    TimeSpan.FromSeconds(60.0),  // Make it invisible for another 60 seconds.
                    //    MessageUpdateFields.Content | MessageUpdateFields.Visibility);

                    // Process the message in less than 30 seconds, and then delete the message
                    await queue.DeleteMessageAsync(message);
                    message = await queue.GetMessageAsync();
                }
            } while (true);
        }
        static async Task<string> Read()
        {
            do
            {
                CloudQueueMessage peekedMessage = await queue.PeekMessageAsync();
                if (peekedMessage == null)
                {
                    Console.WriteLine("\tSleepig for 3");
                    Thread.Sleep(3000);
                    continue;
                }
                Console.WriteLine(peekedMessage.AsString);
                var message = await queue.GetMessageAsync();
                while (message != null)
                {
                    //message.SetMessageContent("Updated contents.");
                    //queue.UpdateMessage(message,
                    //    TimeSpan.FromSeconds(60.0),  // Make it invisible for another 60 seconds.
                    //    MessageUpdateFields.Content | MessageUpdateFields.Visibility);

                    // Process the message in less than 30 seconds, and then delete the message
                    await queue.DeleteMessageAsync(message);
                    message = await queue.GetMessageAsync();
                }
            } while (true);
        }
        static async Task<string> Write()
        {
            do
            {
                Console.WriteLine("Type a message to queue: (stop to stop)");
                var m = Console.ReadLine();
                if (m.ToLower() == "stop")
                    return string.Empty;
                CloudQueueMessage message = new CloudQueueMessage(m);
                await queue.AddMessageAsync(message);
                Thread.Sleep(5000);
            } while (true);
        }
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {

                return;
            }

            storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("CloudStorageConnectionString"));
            queueClient = storageAccount.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference("confirmreading");
            queue.CreateIfNotExistsAsync().Wait();

            switch (args[0].ToLower())
            {
                case "/r":
                    Read().Wait();
                    break;
                case "/w":
                    Write().Wait();
                    break;
                case "/wo":
                    Console.WriteLine("Write Object");
                    WriteObject();
                    break;
                case "/ro":
                    Console.WriteLine("Read Object");
                    ReadObject();
                    break;
            }
        }
    }
}
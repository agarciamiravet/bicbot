namespace BIC.Bot.RestService.ServiceBus
{
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Text;
    using System.Threading.Tasks;

    public class BicServiceBus
    {
        const string ServiceBusConnectionString = "[KEY]";
        const string QueueName = "taskqueue";
        const string QueuePersianaName = "taskVentana";
        const int MessageTimeToLifeInMinutes = 5;

        public async Task SendMessageToHome(int actionToPerform)
        {
            byte[] array = Encoding.UTF8.GetBytes(actionToPerform.ToString());
            var newBusMessage = new Message(array)
            {
                TimeToLive = new TimeSpan(0, MessageTimeToLifeInMinutes, 0)
            };

            var queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            await queueClient.SendAsync(newBusMessage);
            await queueClient.CloseAsync();
        }

        public async Task SendMessageBlindToHome(int actionToPerform)
        {
            byte[] array = Encoding.UTF8.GetBytes(actionToPerform.ToString());
            var newBusMessage = new Message(array)
            {
                TimeToLive = new TimeSpan(0, MessageTimeToLifeInMinutes, 0)
            };

            var queueClient = new QueueClient(ServiceBusConnectionString, QueuePersianaName);

            await queueClient.SendAsync(newBusMessage);
            await queueClient.CloseAsync();
        }
    }
}

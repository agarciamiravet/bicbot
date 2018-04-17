namespace BIC.Bot.GlobalHandler
{
    using BIC.Bot.Constants;
    using BIC.Bot.Dialogs;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using Microsoft.Bot.Builder.Internals.Fibers;
    using Microsoft.Bot.Builder.Scorables.Internals;
    using Microsoft.Bot.Connector;
    using System.Threading;
    using System.Threading.Tasks;


    public class ScorableBot : ScorableBase<IActivity, string, double>
    {
        private IDialogTask _task;

        public ScorableBot(IDialogTask task)
        {
            SetField.NotNull(out _task, nameof(task),
            task);
        }

        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        protected override double GetScore(IActivity item, string state)
        {
            return 1.0;
        }

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            var message = item as IMessageActivity;

            if (message != null)
            {
                string messageToSend = string.Empty;

                if (message.Text.ToLower() == "ayuda" || message.Text.ToLower() == "help")
                {
                    if(item.ChannelId == Channel.Skype)
                    {
                        messageToSend = "Para conversar conmigo puedes hacerlo escribiendome o directamente llamarme.";
                        messageToSend += " \n\n\n\n" + "Escribeme frases de la siguiente manera" ;
                        messageToSend += " \n\n\n\n" + "-Enciende la luz de la habitación";
                        messageToSend += " \n\n\n\n" + "-Apagar luz de la cocina";
                        messageToSend += " \n\n\n\n" + "-Sube la persiana del comedor";
                    }

                    if (item.ChannelId == Channel.Sms)
                    {
                        messageToSend = "Enviame mensajes como";
                        messageToSend += " \n\n" + "encender luz cocina";
                        messageToSend += " \n\n" + "apagar luz comedor";
                        messageToSend += " \n\n" + "subir persiana habitacion";
                    }


                    var commonResponsesDialog = new HelpDialog(messageToSend);
                    var interruption = commonResponsesDialog.Void<object, IMessageActivity>();
                    this._task.Call(interruption, null);
                    await this._task.PollAsync(token);
                }
            }
        }
        protected override bool HasScore(IActivity item, string state)
        {
            return state != null;
        }


        protected override async Task<string> PrepareAsync(IActivity item, CancellationToken token)
        {
            var message = item.AsMessageActivity();
            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                var msg = message.Text.
                ToLowerInvariant();
                if (msg.ToLower() == "help" || msg.ToLower() == "ayuda")
                {
                    return message.Text;
                }
            }
            return null;
        }
    }
}
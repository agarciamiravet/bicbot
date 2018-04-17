using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BIC.Bot.Dialogs
{
    [Serializable]
    public class HelpDialog : IDialog<object>
    {
        private readonly string _messageToSend;
        public HelpDialog(string message)
        {
            _messageToSend = message;
        }

        public ResumeAfter<object> MessageReceived { get; private set; }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(_messageToSend);
            context.Done<object>(null);
        }
    }
}
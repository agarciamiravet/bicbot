namespace BIC.Bot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using BIC.Bot.Common;
    using BIC.Bot.Services;
    using BIC.Bot.Services.Dtos;
    using Microsoft.Bot.Builder.Calling;
    using Microsoft.Bot.Builder.Calling.Events;
    using Microsoft.Bot.Builder.Calling.ObjectModel.Contracts;
    using Microsoft.Bot.Builder.Calling.ObjectModel.Misc;
    using Newtonsoft.Json;

    public class IVRBot : IDisposable, ICallingBot
    {
        // DTMF keys required for each of option, will be used for parsing results of recognize
        private const string Support = "1";

        // Response messages depending on user selection
        //private const string WelcomeMessage = "Hello, you have successfully contacted the Emergency Services Bot.";
        private const string WelcomeMessage = "Bienvenido to BIC Bot. Your home bot";
        //private const string MainMenuPromptMessage = "If you have a life threatening medical emergency please contact the emergency services or go to your nearest hospital. For non-life threatening situations please press 1.";
        private const string MainMenuPromptMessage = "Please press 1 or say yes for handling your home.";
        //private const string NoConsultantsMessage = "Whilst we wait to connect you, please leave your name and a description of your problem. You can press the hash key when finished. We will call you as soon as possible.";
        private const string NoConsultantsMessage = "Please say your command.";
        private const string EndingMessage = "Thank you for leaving the command, goodbye";
        private const string OptionMenuNotSupportedMessage = "The option you entered is not supported. Please try again.";

        private readonly Dictionary<string, CallState> callStateMap = new Dictionary<string, CallState>();

        private readonly MicrosoftCognitiveSpeechService speechService = new MicrosoftCognitiveSpeechService();

        public IVRBot(ICallingBotService callingBotService)
        {
            if (callingBotService == null)
            {
                throw new ArgumentNullException(nameof(callingBotService));
            }

            this.CallingBotService = callingBotService;

            this.CallingBotService.OnIncomingCallReceived += this.OnIncomingCallReceived;
            this.CallingBotService.OnPlayPromptCompleted += this.OnPlayPromptCompleted;
            this.CallingBotService.OnRecordCompleted += this.OnRecordCompleted;
            this.CallingBotService.OnRecognizeCompleted += this.OnRecognizeCompleted;
            this.CallingBotService.OnHangupCompleted += OnHangupCompleted;
        }

        public ICallingBotService CallingBotService { get; }

        public void Dispose()
        {
            if (this.CallingBotService != null)
            {
                this.CallingBotService.OnIncomingCallReceived -= this.OnIncomingCallReceived;
                this.CallingBotService.OnPlayPromptCompleted -= this.OnPlayPromptCompleted;
                this.CallingBotService.OnRecordCompleted -= this.OnRecordCompleted;
                this.CallingBotService.OnRecognizeCompleted -= this.OnRecognizeCompleted;
                this.CallingBotService.OnHangupCompleted -= OnHangupCompleted;
            }
        }

        private static Task OnHangupCompleted(HangupOutcomeEvent hangupOutcomeEvent)
        {
            hangupOutcomeEvent.ResultingWorkflow = null;
            return Task.FromResult(true);
        }

        private static void SetupInitialMenu(Workflow workflow)
        {
            workflow.Actions = new List<ActionBase> { GetInitialMenu() };
        }

        private static void SetupInitialMenuWithErrorMessage(Workflow workflow)
        {
            workflow.Actions = new List<ActionBase>
            {
                GetPromptForText(OptionMenuNotSupportedMessage),
                GetInitialMenu()
            };
        }

        private static ActionBase GetInitialMenu()
        {
            return CreateIvrOptions(MainMenuPromptMessage, 1, false);
        }

        private static void ProcessMainMenuSelection(RecognizeOutcomeEvent outcome, CallState callStateForClient)
        {
            if (outcome.RecognizeOutcome.Outcome != Outcome.Success)
            {
                if (outcome.RecognizeOutcome.FailureReason == RecognitionCompletionReason.IncorrectDtmf.ToString())
                {
                    SetupInitialMenuWithErrorMessage(outcome.ResultingWorkflow);
                }
                else
                {
                    SetupInitialMenu(outcome.ResultingWorkflow);
                }

                return;
            }

            switch (outcome.RecognizeOutcome.ChoiceOutcome.ChoiceName)
            {
                case Support:
                    callStateForClient.ChosenMenuOption = Support;
                    SetupRecording(outcome.ResultingWorkflow);
                    break;
                default:
                    SetupInitialMenu(outcome.ResultingWorkflow);
                    break;
            }
        }

        private static Recognize CreateIvrOptions(string textToBeRead, int numberOfOptions, bool includeBack)
        {
            if (numberOfOptions > 9)
            {
                throw new Exception("too many options specified");
            }

            var choices = new List<RecognitionOption>();

            for (int i = 1; i <= numberOfOptions; i++)
            {
                choices.Add(new RecognitionOption { Name = Convert.ToString(i), DtmfVariation = (char)('0' + i), SpeechVariation = new List<string>() { "yes", "one" } });
            }

            if (includeBack)
            {
                choices.Add(new RecognitionOption { Name = "#", DtmfVariation = '#' });
            }

            var recognize = new Recognize
            {
                OperationId = Guid.NewGuid().ToString(),
                PlayPrompt = GetPromptForText(textToBeRead),
                BargeInAllowed = true,
                Choices = choices
            };

            return recognize;
        }

        private static void SetupRecording(Workflow workflow)
        {
            var id = Guid.NewGuid().ToString();

            var prompt = GetPromptForText(NoConsultantsMessage);
            var record = new Record
            {
                OperationId = id,
                PlayPrompt = prompt,
                MaxDurationInSeconds = 10,
                InitialSilenceTimeoutInSeconds = 5,
                MaxSilenceTimeoutInSeconds = 0,
                PlayBeep = true,
                RecordingFormat = RecordingFormat.Wav,
                //StopTones = new List<char> { '#' }
            };
            workflow.Actions = new List<ActionBase> { record };
        }

        private static PlayPrompt GetPromptForText(string text)
        {
            var prompt = new Prompt { Value = text, Voice = VoiceGender.Male, Culture = Culture.Unknown };
            return new PlayPrompt { OperationId = Guid.NewGuid().ToString(), Prompts = new List<Prompt> { prompt } };
        }

        private Task OnIncomingCallReceived(IncomingCallEvent incomingCallEvent)
        {
            this.callStateMap[incomingCallEvent.IncomingCall.Id] = new CallState(incomingCallEvent.IncomingCall.Participants);

            incomingCallEvent.ResultingWorkflow.Actions = new List<ActionBase>
                {
                    new Answer { OperationId = Guid.NewGuid().ToString() },
                    GetPromptForText(WelcomeMessage)
                };

            return Task.FromResult(true);
        }

        private Task OnPlayPromptCompleted(PlayPromptOutcomeEvent playPromptOutcomeEvent)
        {
            var callState = this.callStateMap[playPromptOutcomeEvent.ConversationResult.Id];
            SetupInitialMenu(playPromptOutcomeEvent.ResultingWorkflow);

            return Task.FromResult(true);
        }

        private async Task OnRecordCompleted(RecordOutcomeEvent recordOutcomeEvent)
        {
            recordOutcomeEvent.ResultingWorkflow.Actions = new List<ActionBase>
                {
                    GetPromptForText(EndingMessage),
                    new Hangup { OperationId = Guid.NewGuid().ToString() }
                };

            // Convert the audio to text
            if (recordOutcomeEvent.RecordOutcome.Outcome == Outcome.Success)
            {
                var record = await recordOutcomeEvent.RecordedContent;
                string text = await this.GetTextFromAudioAsync(record);

                var callState = this.callStateMap[recordOutcomeEvent.ConversationResult.Id];

                await this.SendSTTResultToUser(text, callState.Participants);
            }

            recordOutcomeEvent.ResultingWorkflow.Links = null;
            this.callStateMap.Remove(recordOutcomeEvent.ConversationResult.Id);
        }

        private async Task SendSTTResultToUser(string text, IEnumerable<Participant> participants)
        {
            var to = participants.Single(x => x.Originator);
            var from = participants.First(x => !x.Originator);

            var jsonSpeech = JsonConvert.DeserializeObject<BingSpeechResponse>(text);

            var g = await this.takeAction(jsonSpeech.DisplayText);
            await AgentListener.Resume(to.Identity, to.DisplayName, from.Identity, from.DisplayName, to.Identity, g);
        }

        private async Task<string> takeAction(string text)
        {
            var result = string.Empty;
            var luisService = new LuisService();
            var resultado = await luisService.MakeRequest(text);

            // encender 

            if (resultado == "encendercocina")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.lightOnKitchenAction);
                return  "Luz de la cocina encendida";
            }

            if (resultado == "encenderbaño")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.lightOnBathRoomAction);
                return "Luz del baño encendida";
            }

            if (resultado == "encenderrecibidor")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.lightOnEntranceHallAction);
                return "Luz del recibidor encendida";
            }

            if (resultado == "encenderhabitacion")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.lightOnBedroomAction);
                return "Luz de la habitación encendida";
            }

            //apagar

            if (resultado == "apagarcocina")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.lightOffKitchenAction);
                return "Luz de la cocina apagada";
            }

            if (resultado == "apagarbaño")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.lightOffBathRoomAction);
                return "Luz del baño apagada";
            }

            if (resultado == "apagarrecibidor")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.lightOffEntranceHallAction);
                return "Luz del recibidor apagada";
            }

            if (resultado == "apagarhabitacion")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.lightOffBedroomAction);
                return "Luz de la habitación apagada";
            }

            if (resultado == "apagartodo")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.lightOffAllAction);
                return "Apagadas todas las luces de la casa";
            }

            // bajar persiana

            if (resultado == "bajarpersianacocina")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.blindDownKitchenAction);
                return "Bajada la persiana de la cocina";
            }

            if (resultado == "bajarpersianahabitacion")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.blindDownBedroomAction);
                return "Bajada la persiana de la habitación";
            }

            if (resultado == "bajarpersianabuzon")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.blindDownMailboxAction);
                return "Bajada la persiana del buzón";
            }


            // subir persiana

            if (resultado == "subirpersianacocina")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.blindUpKitchenAction);
                return "Subida la persiana de la cocina";
            }

            if (resultado == "subirpersianahabitacion")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.blindUpBedroomAction);
                return "Subida la persiana de la habitación";
            }

            if (resultado == "subirpersianabuzon")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.blindDownMailboxAction);
                return "Subida la persiana del buzón";
            }

            if(resultado == "carteropaquete")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.mailUserRequestAction);
                return "Por favor deposita el paquete en el buzón";
            }

            if(resultado == "encendertodo")
            {
                HomeRestService homeRestService = new HomeRestService();
                await homeRestService.ExecuteAction(HomeRestService.lightOnAllAction);
                return  "Encendidas todas las luces de la casa";
            }


            return resultado;
        }


        /// <summary>
        /// Gets text from an audio stream.
        /// </summary>
        /// <param name="audiostream"></param>
        /// <returns>Transcribed text. </returns>
        private async Task<string> GetTextFromAudioAsync(Stream audiostream)
        {
            try
            {
                var text = await this.speechService.GetTextFromAudioAsync(audiostream);
                Debug.WriteLine(text);
                return text;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        private Task OnRecognizeCompleted(RecognizeOutcomeEvent recognizeOutcomeEvent)
        {
            var callState = this.callStateMap[recognizeOutcomeEvent.ConversationResult.Id];

            ProcessMainMenuSelection(recognizeOutcomeEvent, callState);

            return Task.FromResult(true);
        }

        private class CallState
        {
            public CallState(IEnumerable<Participant> participants)
            {
                this.Participants = participants;
            }

            public string ChosenMenuOption { get; set; }

            public IEnumerable<Participant> Participants { get; }
        }
    }
}
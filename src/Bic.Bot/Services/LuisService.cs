namespace BIC.Bot.Services
{
    using BIC.Bot.Services.Dtos;
    using Newtonsoft.Json;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;

    public class LuisService
    {
        public async Task<string> MakeRequest(string textToSend)
        {
            string result = string.Empty;
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // This app ID is for a public sample app that recognizes requests to turn on and turn off lights
            var luisAppId = "[KEY]";
            var subscriptionKey = "[KEY]";

            // The request header contains your subscription key
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            var sendToSend = textToSend;

            sendToSend = textToSend.Replace("ó", "o").Replace(".", "");

            queryString["q"] = sendToSend;
           
            // These optional request parameters are set to their default values
            queryString["timezoneOffset"] = "0";
            queryString["verbose"] = "false";
            queryString["spellCheck"] = "true";
            queryString["bing-spell-check-subscription-key"] = "[KEY]";
            queryString["staging"] = "false";

            var uri = "https://westeurope.api.cognitive.microsoft.com/luis/v2.0/apps/" + luisAppId + "?" + queryString;
            
            var response = await client.GetAsync(uri);

            var strResponseContent = await response.Content.ReadAsStringAsync();


            var json = JsonConvert.DeserializeObject<LuisResponse>(strResponseContent);

            if (json.topScoringIntent != null && json.topScoringIntent.intent == "EncenderElementoCasa")
            {
                var entities = json.entities.Select(l => l.entity).ToList();

                if (entities.Contains("luz"))
                {
                    if (entities.Contains("cocina"))
                    {
                        return "encendercocina";
                    }

                    if (entities.Contains("baño"))
                    {
                        return "encenderbaño";
                    }

                    if (entities.Contains("habitacion"))
                    {
                        return "encenderhabitacion";
                    }

                    if (entities.Contains("recibidor"))
                    {
                        return "encenderrecibidor";
                    }
                }
            }

            if (json.topScoringIntent != null && json.topScoringIntent.intent == "ApagarElementoCasa")
            {
                var entities = json.entities.Select(l => l.entity).ToList();

                if (entities.Contains("verbena") && entities.Contains("paloma"))
                {
                    return "apagartodo";
                }

                if (entities.Contains("luz"))
                {
                    if (entities.Contains("cocina"))
                    {
                        return "apagarcocina";
                    }

                    if (entities.Contains("baño"))
                    {
                        return "apagarbaño";
                    }

                    if (entities.Contains("habitacion"))
                    {
                        return "apagarhabitacion";
                    }

                    if (entities.Contains("recibidor"))
                    {
                        return "apagarrecibidor";
                    }
                }
            }

            if (json.topScoringIntent != null && json.topScoringIntent.intent == "BajarPersiana")
            {
                var entities = json.entities.Select(l => l.entity).ToList();

                if (entities.Contains("persiana"))
                {
                    if (entities.Contains("cocina"))
                    {
                        return "bajarpersianacocina";
                    }

                    if (entities.Contains("habitación") || entities.Contains("habitacion"))
                    {
                        return "bajarpersianahabitacion";
                    }

                    if (entities.Contains("buzon") || entities.Contains("buzón"))
                    {
                        return "bajarpersianabuzon";
                    }
                }
            }

            if (json.topScoringIntent != null && json.topScoringIntent.intent == "SubirPersiana")
            {
                var entities = json.entities.Select(l => l.entity).ToList();

                if (entities.Contains("persiana"))
                {
                    if (entities.Contains("cocina"))
                    {
                        return "subirpersianacocina";
                    }

                    if (entities.Contains("habitación") || entities.Contains("habitacion"))
                    {
                        return "subirpersianahabitacion";
                    }

                    if (entities.Contains("buzon") || entities.Contains("buzón"))
                    {
                        return "subirpersianabuzon";
                    }
                }
            }


            if (json.topScoringIntent != null && json.topScoringIntent.intent == "CarteroPaquete")
            {
                return "carteropaquete";
            }

            if (json.topScoringIntent != null && json.topScoringIntent.intent == "ApagarDicho")
            {
                return "encendertodo";
            }
            return result;
        }

    }
}
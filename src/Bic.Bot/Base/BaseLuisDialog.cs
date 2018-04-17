using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BIC.Bot.Base
{
    [Serializable]
    public class BaseLuisDialog<T> : LuisDialog<T>
    {
        public BaseLuisDialog() : base(GetNewService())
        {

        }

        private static ILuisService[] GetNewService()
        {
            var modelId = ConfigurationManager.AppSettings.Get("LuisModelId");
            var subscriptionKey = ConfigurationManager.AppSettings.Get("LuisSubscriptionKey");
            var domain = ConfigurationManager.AppSettings.Get("LuisDomain"); 
            var luisModel = new LuisModelAttribute(modelId, subscriptionKey, LuisApiVersion.V2,domain);
            return new ILuisService[] { new LuisService(luisModel) };
        }
    }
}
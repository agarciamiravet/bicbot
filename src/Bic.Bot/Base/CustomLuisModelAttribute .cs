namespace BIC.Bot.Base
{
    using Microsoft.Bot.Builder.Luis;
    using System;
    using System.Configuration;


    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true)]
    public class CustomLuisModelAttribute : LuisModelAttribute, ILuisModel
    {
        public CustomLuisModelAttribute() : base(
            GetModelId(),
            GetSubscriptionKey(),
            LuisApiVersion.V2,
            GetDomain())
        { }

        private static string GetModelId()
        {
            return ConfigurationManager.AppSettings.Get("LuisModelId");
        }

        private static string GetSubscriptionKey()
        {
            return ConfigurationManager.AppSettings.Get("LuisSubscriptionKey");
        }

        private static string GetDomain()
        {
            return ConfigurationManager.AppSettings.Get("LuisDomain");
        }
    }
}
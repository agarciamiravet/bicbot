using Autofac;
using System.Web.Http;
using System.Configuration;
using System.Reflection;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using BIC.Bot.GlobalHandler;
using Microsoft.Bot.Builder.Scorables;

namespace BIC.Bot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var store = new TableBotDataStore(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            Conversation.UpdateContainer(
                builder =>
                {
                    builder.RegisterModule(new DialogModule());
                   
                    builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));

                    // Using Azure Table Storage
                    //var store = new TableBotDataStore(ConfigurationManager.AppSettings["AzureWebJobsStorage"]); // requires Microsoft.BotBuilder.Azure Nuget package 

                    // To use CosmosDb or InMemory storage instead of the default table storage, uncomment the corresponding line below
                    // var store = new DocumentDbBotDataStore("cosmos db uri", "cosmos db key"); // requires Microsoft.BotBuilder.Azure Nuget package 
                    //var store = new InMemoryDataStore(); // volatile in-memory store

                    //builder.Register(c => store)
                    //    .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                    //    .AsSelf()
                    //    .SingleInstance();

                    builder.RegisterType<ScorableBot>().As<IScorable<IActivity, double>>().InstancePerLifetimeScope();

                    builder.Register(c => store)
                         .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                         .AsSelf()
                         .SingleInstance();

                    builder.Register(c => new CachingBotDataStore(store,
                               CachingBotDataStoreConsistencyPolicy
                               .ETagBasedConsistency))
                               .As<IBotDataStore<BotData>>()
                               .AsSelf()
                               .InstancePerLifetimeScope();

                });
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}

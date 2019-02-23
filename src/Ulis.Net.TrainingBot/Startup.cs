using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ulis.Net.Library;
using Ulis.Net.Recognizer;

namespace Ulis.Net.TrainingBot
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseBotFramework();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            var botConfig = BotConfiguration.Load(Configuration["BotFilePath"]);

            var service = botConfig.Services.FirstOrDefault(s => s.Type == "generic" && s.Name == "microsoft-translator");
            if (!(service is GenericService microsoftTranslatorService))
            {
                throw new InvalidOperationException($"The .bot file does not contain a Microsoft Translator service.");
            }

            service = botConfig.Services.FirstOrDefault(s => s.Type == "luis" && s.Name == "luis");
            if (!(service is LuisService luisService))
            {
                throw new InvalidOperationException($"The .bot file does not contain a LUIS service.");
            }

            services.AddSingleton(sp => new UlisRecognizer(
                luisService,
                new MicrosoftTranslatorClient(
                    sp.GetRequiredService<IHttpClientFactory>().CreateClient(),
                    microsoftTranslatorService.Configuration["key"])));

            services.AddBot<TrainingBot>(options =>
            {
                service = botConfig.Services.FirstOrDefault(s => s.Type == "endpoint" && s.Name == "development");
                if (!(service is EndpointService endpointService))
                {
                    throw new InvalidOperationException($"The .bot file does not contain a development endpoint.");
                }

                options.CredentialProvider = new SimpleCredentialProvider(endpointService.AppId, endpointService.AppPassword);

                options.OnTurnError = async (context, exception) =>
                {
                    await context.SendActivityAsync("Sorry, it looks like something went wrong.");
                };
            });
        }
    }
}
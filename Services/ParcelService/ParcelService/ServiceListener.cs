using ParcelService.Services.LandBank;
using ParcelService.Services.LBApplicationStatus;
using ParcelService.Services.LBBookmarks;
using ParcelService.Services.Parcels;
using ServiceStack;
using ServiceStack.Text;
using SharedCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService
{
    internal class ServiceListener
    {
        internal void StartListening()
        {
            var listeningOn = $"http://*:{Helper.PortNumber}/";
            Licensing.RegisterLicense(Helper.ServiceStackLicense);
            var appHost = new AppHost()
                .Init()
                .Start(listeningOn);

            Logger.Info($"Serice Hub listening on {listeningOn}");
            //http://localhost:9866//diagnostics
        }

        internal class AppHost : AppSelfHostBase
        {
            public AppHost() : base("Parcel Service", typeof(AppHost).Assembly) { }
            public override void Configure(Funq.Container container)
            {
                JsConfig.DateHandler = DateHandler.ISO8601;
                JsConfig<DateTime>.SerializeFn = dt => dt.ToString("O"); 

                SetConfig(new HostConfig { UseCamelCase = false });

                container.RegisterAutoWiredTypes(
                                   Assembly.GetExecutingAssembly().GetTypes()
                                       .Where(t => t.IsSubclassOf(typeof(Service))).ToArray()
                               );

                container.Register<ILandBankDataProvider>(c => new LandBankDataProvider());
                container.Register<IApplicationStatusDataProvider>(c => new ApplicationStatusDataProvider());
                container.Register<IBookmarksDataProvider>(c => new BookmarksDataProvider());
                container.Register<IParcelsDataProvider>(c => new ParcelsDataProvider());

                Plugins.Add(new CorsFeature(
                   allowedOrigins: "*",
                   allowedMethods: "GET,POST,PUT,DELETE,OPTIONS,HEAD",
                   allowedHeaders: "Content-Type, Authorization, Accept, X-Requested-With",
                   allowCredentials: false
                   ));

                //this.SetConfig(new HostConfig
                //{
                //    EnableFeatures = Feature.All.Remove(Feature.Metadata)
                //});
            }
        }
    }
}

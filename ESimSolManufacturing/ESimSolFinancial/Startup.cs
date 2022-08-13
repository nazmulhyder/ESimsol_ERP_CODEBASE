using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.SignalR;
using ESimSolFinancial.Hubs;

[assembly: OwinStartup(typeof(ESimSolFinancial.Startup))]
namespace ESimSolFinancial
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            var idProvider = new ESimSolFinancial.Hubs.ProgressHub.CustomUserIdProvider();

            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

            app.MapSignalR("/signalr", new HubConfiguration());
        }
    }
}
using BankingService;
using CommonServiceLocator;
using System.Web.Http;
using System.Web.Services.Description;
using Unity;
using Unity.ServiceLocation;

namespace WebApp
{
    /// <summary>
    /// Konfiguration für den IoC Container Unity
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Registers the components.
        /// </summary>
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            UnityServiceLocator locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
            container.RegisterInstance(typeof(HttpConfiguration), GlobalConfiguration.Configuration);
            RegisterTypes(container);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityResolver(container);
        }

        private static void RegisterTypes(IUnityContainer container)
        {  
            container.RegisterType<Service>();          
            container.RegisterType<AccountService>();
        }
    }
}
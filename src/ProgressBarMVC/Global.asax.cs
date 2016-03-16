using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;
using SetStartupProjects;

public class MvcApplication : HttpApplication
{
    IBus _bus;

    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        routes.MapRoute(
            "Default", // Route name
            "{controller}/{action}/{id}", // URL with parameters
            new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            } // Parameter defaults
            );
    }

    public override void Dispose()
    {
        if (_bus != null)
        {
            _bus.Dispose();
        }
        base.Dispose();
    }

    protected void Application_Start()
    {
        ContainerBuilder builder = new ContainerBuilder();

        // Register your MVC controllers.
        builder.RegisterControllers(typeof(MvcApplication).Assembly);

        // Set the dependency resolver to be Autofac.
        IContainer container = builder.Build();

        DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Mvc.WebApplication");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        busConfiguration.RegisterComponents(c => c.ConfigureComponent<StatusStoreClient>(DependencyLifecycle.InstancePerCall));

        IStartableBus startableBus = Bus.Create(busConfiguration);
        _bus = startableBus.Start();

        AreaRegistration.RegisterAllAreas();
        RegisterRoutes(RouteTable.Routes);
    }
}
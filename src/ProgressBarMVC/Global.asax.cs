using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;

public class MvcApplication : HttpApplication
{
    IEndpointInstance _endpoint;

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
        _endpoint?.Stop().GetAwaiter().GetResult();
        base.Dispose();
    }

    protected void Application_Start()
    {
        ContainerBuilder builder = new ContainerBuilder();

        // Register your MVC controllers.
        builder.RegisterControllers(typeof(MvcApplication).Assembly);

        var busConfiguration = new EndpointConfiguration("Samples.Mvc.WebApplication");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UseContainer<AutofacBuilder>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        _endpoint = Endpoint.Start(busConfiguration).GetAwaiter().GetResult();

        builder.Register(c => _endpoint).As<IMessageSession>();
        builder.Register(c => new StatusStoreClient()).As<IStatusStoreClient>();

        var container = builder.Build();

        DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        AreaRegistration.RegisterAllAreas();
        RegisterRoutes(RouteTable.Routes);
    }
}
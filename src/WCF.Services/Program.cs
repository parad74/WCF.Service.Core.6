using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCF.Description;
using WCF.Services.Behavior;
using WCF.Services.Logging;
using WCF.Services;
using Microsoft.EntityFrameworkCore;

public class Program
{
	public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel((context, options) =>
        {
            options.AllowSynchronousIO = true;
        });

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        //builder.Services.AddScoped<EchoService>();
        builder.Services.AddScoped<BookService>();
        builder.Services.AddSingleton<LogMessageBehavior>();
        builder.Services.AddSingleton<LogMessageInspector>();

        // Add WSDL support
        builder.Services.AddServiceModelServices().AddServiceModelMetadata();
        builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();
		builder.Services.AddDbContext<CatalogSqliteDBContext>(options =>
			   options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

		var app = builder.Build();

		CreateDbIfNotExists(app);

		app.UseMiddleware<LogHeadersMiddleware>();

        app.UseServiceModel(builder =>
        {
			builder
			.AddService<BookService>(serviceOptions =>
			{
				serviceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true;
			})
			.AddServiceEndpoint<BookService, IBookService>(
			new BasicHttpBinding(BasicHttpSecurityMode.None) { MaxReceivedMessageSize = 1000000000 },
			"/BookService", endpointOptions =>
			{
				endpointOptions.EndpointBehaviors.Add(app.Services.GetRequiredService<LogMessageBehavior>());
				
			});
		});

        var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
        serviceMetadataBehavior.HttpGetEnabled = true;
        serviceMetadataBehavior.HttpsGetEnabled = true;

		serviceMetadataBehavior.HttpGetUrl = new Uri("http://localhost:5051/metadata");
		serviceMetadataBehavior.HttpsGetUrl = new Uri("https://localhost:7051/metadata");

		app.Run();
    }

	private static void CreateDbIfNotExists(WebApplication host)
	{
		using (var scope = host.Services.CreateScope())
		{
			var services = scope.ServiceProvider;

			try
			{
				var context = services.GetRequiredService<CatalogSqliteDBContext>();
				DbInitializer.Initialize(context, host.Environment);
			}
			catch (Exception ex)
			{
				var logger = services.GetRequiredService<ILogger<Program>>();
				logger.LogError(ex, "An error occurred creating the DB.");
			}
		}
	}
}
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SharpLab2.Components;
using SharpLab2.Data;
using SharpLab2.HubClients;
using SharpLab2.Hubs;
using SharpLab2.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseStaticWebAssets();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddSignalR(options =>
    {
        options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    })
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<ITrainService, TrainService>();
builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddScoped<ICarriageTypeService, CarriageTypeService>();

builder.Services.AddScoped<ITicketHubClient, TicketHubClient>();
builder.Services.AddScoped<IPassengerHubClient, PassengerHubClient>();
builder.Services.AddScoped<ITrainHubClient, TrainHubClient>();
builder.Services.AddScoped<IDestinationHubClient, DestinationHubClient>();
builder.Services.AddScoped<ICarriageTypeHubClient, CarriageTypeHubClient>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var initializer = new DatabaseInitializer(context);
    initializer.Initialize();
}

app.UseRouting();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.MapHub<TicketsHub>("/hubs/tickets");
app.MapHub<PassengersHub>("/hubs/passengers");
app.MapHub<TrainsHub>("/hubs/trains");
app.MapHub<DestinationsHub>("/hubs/destinations");
app.MapHub<CarriageTypesHub>("/hubs/carriagetypes");

app.Run();
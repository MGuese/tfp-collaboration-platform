using tfp_collab_userspace_domain.Service;
using tfp_collab_userspace_domain.UseCase;
using tfp_collab_userspace_storage_database;
using tfp_collab_userspace_storage_database.Initialize;
using tfp_collab_userspace_storage_database.Repositories;

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    // Add other configuration sources as needed
    .Build();

var builder = WebApplication.CreateBuilder(); // This might still be available for building the app

// Füge den Logging-Dienst hinzu
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    // Optional: Konfiguration aus appsettings.json laden (Abschnitt "Logging")
    loggingBuilder.AddConfiguration(builder.Configuration.GetSection("Logging"));
    // Optional: Setze das minimale LogLevel direkt im Code
    loggingBuilder.SetMinimumLevel(LogLevel.Information);
});

// Database und DbConnection
builder.Services.AddScoped<IDbConnectionFactory>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new PostgresDbConnectionFactory(connectionString);
});
builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();
builder.Services.AddScoped<IDatabaseService, DapperService>();
builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

// Use Cases
builder.Services.AddScoped<ICreateGalleryUseCase, CreateGalleryUseCase>();
builder.Services.AddScoped<IGetAllGalleriesUseCase, GetAllGalleriesUseCase>();

// Add services to the container.
builder.Services.AddControllers(); // For Web API or MVC
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting(); // Enables endpoint routing

// Deine Endpunkte
app.MapGet("/", (ILogger<Program> logger) =>
{
     logger.LogInformation("Root-Endpunkt wurde aufgerufen.");
     return "Hallo von deiner Minimal API mit Logger!";
});

app.MapControllers(); // Maps controller actions to routes

// Datenbank Schema erstellen wenn notwendig.
using (var scope = app.Services.CreateScope())
{
    var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    databaseInitializer.InitializeDatabaseAsync().Wait(); // Oder await im async Kontext
}

app.Run();
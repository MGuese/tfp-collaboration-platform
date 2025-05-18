using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using tfp_collab_userspace_api.Authorization;
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

// Konditionale Registrierung des ICurrentUserContext
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<ICurrentUserContext, DevelopmentCurrentUserContext>();
    Console.WriteLine("Using DevelopmentCurrentUserContext for OwnerId.");

    // Für den Entwicklungsmodus: Registrieren Sie ein Dev-Authentifizierungsschema
    // Setzen Sie dies als DefaultAuthenticateScheme und DefaultChallengeScheme
    builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "DevScheme"; // Wichtig!
            options.DefaultChallengeScheme = "DevScheme";    // Wichtig!
        })
        .AddScheme<AuthenticationSchemeOptions, DevelopmentAuthHandler>("DevScheme", options => { });
}
else
{
    builder.Services.AddSingleton<ICurrentUserContext, CurrentUserContext>();
    Console.WriteLine("Using Production CurrentUserContext for OwnerId.");
    
    // 1. Authentifizierung hinzufügen
    builder.Services.AddAuthentication(options =>
    {
        // Hier definieren Sie das Standard-Schema.
        // Für JWT Bearer Tokens ist dies oft "Bearer"
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => // Beispiel: JWT Bearer Authentifizierung konfigurieren
    {
        // Dies ist der wichtigste Teil für JWTs.
        // Sie müssen angeben, wie Ihr Token validiert werden soll.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Validiert den Herausgeber des Tokens
            ValidateAudience = true, // Validiert den Empfänger des Tokens
            ValidateLifetime = true, // Validiert die Gültigkeitsdauer des Tokens
            ValidateIssuerSigningKey = true, // Validiert die Signatur des Tokens

            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Aus Konfiguration lesen
            ValidAudience = builder.Configuration["Jwt:Audience"], // Aus Konfiguration lesen
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Ihr geheimer Schlüssel
        };
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting(); // Enables endpoint routing
app.MapControllers(); // Maps controller actions to routes

if (!app.Environment.IsDevelopment()) // Nur im NICHT-Entwicklungsmodus Authentifizierung anwenden
{
    app.UseAuthentication();
}

app.UseAuthorization();  // Wichtig für den Produktions-CurrentUserContext

// Datenbank Schema erstellen wenn notwendig.
using (var scope = app.Services.CreateScope())
{
    var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    databaseInitializer.InitializeDatabaseAsync().Wait(); // Oder await im async Kontext
}

app.Run();
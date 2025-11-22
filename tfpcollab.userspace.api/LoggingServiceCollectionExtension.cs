using tfp_collab_userspace_api.Authorization;
using tfp_collab_userspace_domain.Service;
using tfp_collab_userspace_domain.UseCase;
using tfp_collab_userspace_storage_database;
using tfp_collab_userspace_storage_database.Initialize;
using tfp_collab_userspace_storage_database.Repositories;

namespace tfp_collab_userspace_api;

public static class LoggingServiceCollectionExtensions
{
    public static IServiceCollection AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(loggingBuilder =>
        {
            // Fügt den Konsolen-Provider hinzu
            loggingBuilder.AddConsole(); 
            
            // Lädt die Konfiguration aus dem Abschnitt "Logging" 
            // der appsettings.json
            loggingBuilder.AddConfiguration(configuration.GetSection("Logging")); 
            
            // Setzt das minimale LogLevel direkt im Code
            // Dies kann durch die Konfiguration überschrieben werden
            loggingBuilder.SetMinimumLevel(LogLevel.Information); 
        });

        return services;
    }
    
    public static IServiceCollection AddDAL(this IServiceCollection services)
    {
        services.AddScoped<IDbConnectionFactory>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException("No connection string found.");
                return new PostgresDbConnectionFactory(connectionString);
            });
        //builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

        return services;
    }
    
    public static IServiceCollection AddBL(this IServiceCollection services)
    {
        services.AddScoped<ICreateGalleryUseCase, CreateGalleryUseCase>();
        return services;
    }

    public static IServiceCollection AddAuthorization(this IServiceCollection services, bool development)
    {
        //services.AddAuthorization();
        
        if (development)
        {
            services.AddSingleton<ICurrentUserContext, DevelopmentCurrentUserContext>();
            Console.WriteLine("Using DevelopmentCurrentUserContext for OwnerId.");
            
            /*
            // Für den Entwicklungsmodus: Registrieren Sie ein Dev-Authentifizierungsschema
            // Setzen Sie dies als DefaultAuthenticateScheme und DefaultChallengeScheme
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "DevScheme"; // Wichtig!
                    options.DefaultChallengeScheme = "DevScheme";    // Wichtig!
                })
                .AddScheme<AuthenticationSchemeOptions, DevelopmentAuthHandler>("DevScheme", options => { });
                */
        }
        else
        {
            services.AddSingleton<ICurrentUserContext, CurrentUserContext>();
            Console.WriteLine("Using Production CurrentUserContext for OwnerId.");
            /*
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
            */
        }
        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(_ =>
        {
            /*
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
            */
        });
        return services;
    }
}
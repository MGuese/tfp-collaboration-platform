using System.Data;
using Microsoft.Data.SqlClient;
using tfp_collab_userspace_domain.Service;
using tfp_collab_userspace_storage_database;

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    // Add other configuration sources as needed
    .Build();

var services = new ServiceCollection();

// Register Image Repository Service (assuming you've configured your IDbConnection)
services.AddScoped<IDatabaseService, DapperService>();
services.AddScoped<IDbConnection>(sp => new SqlConnection(configurationBuilder.GetConnectionString("DefaultConnection"))); // Example connection

var builder = WebApplication.CreateBuilder(); // This might still be available for building the app

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

app.UseHttpsRedirection();
app.UseRouting(); // Enables endpoint routing
app.UseAuthorization(); // Enables authorization
app.MapControllers(); // Maps controller actions to routes
app.Run();
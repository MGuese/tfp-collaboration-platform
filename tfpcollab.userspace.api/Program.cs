using tfpcollab.userspace.api;
using ConfigurationExtensions = tfpcollab.userspace.api.ConfigurationExtensions;

var configuration = ConfigurationExtensions.BuildConfiguration();
var builder = WebApplication.CreateBuilder();
builder.Configuration.AddConfiguration(configuration);
builder.Services.AddCustomLogging(configuration);
builder.Services.AddDAL();
builder.Services.AddBL();
builder.Services.AddAuthorization(builder.Environment.IsDevelopment());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();
await app.InitializeDatabaseAsync();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting(); // Enables endpoint routing
// AUTHENTIFIZIERUNG und AUTORISIERUNG Middleware
// diese müssen nach UseRouting() und vor MapControllers() stehen
//app.UseAuthentication(); 
//app.UseAuthorization();
app.MapControllers(); // Maps controller actions to routes
app.Run();
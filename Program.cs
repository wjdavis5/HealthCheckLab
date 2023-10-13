using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//dotnet add package AspNetCore.HealthChecks.SqlServer --version 7.0.0

builder.Services.AddHealthChecks()
    .AddSqlServer(
        options: new HealthChecks.SqlServer.SqlServerHealthCheckOptions()
        {
            ConnectionString = "Server=localhost,1433;Database=master;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True;Encrypt=false",
            CommandText = "SELECT 1"
        },
        name: "SqlServer",
        failureStatus: HealthStatus.Unhealthy,
        tags: new string[] { "db", "sql", "sqlserver" },
        timeout: TimeSpan.FromSeconds(5)
        );

var app = builder.Build();
app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

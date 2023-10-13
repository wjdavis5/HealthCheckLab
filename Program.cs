using Azure.Storage.Blobs;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//dotnet add package AspNetCore.HealthChecks.SqlServer --version 7.0.0

builder.Services.AddBackgroundHealthChecks(builder.Configuration)
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
        )
    .AddAzureBlobStorage(
        connectionString: "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;",
        containerName: "test",
        name: "AzureStorage")
    .Services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Delay = TimeSpan.FromSeconds(2);
        });

var app = builder.Build();
app.MapBackgroundHealthChecks("/health");

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

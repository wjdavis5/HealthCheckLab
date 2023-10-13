using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.VisualBasic;

namespace HealthCheckLab.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    //docker run -it -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 -d mcr.microsoft.com/mssql/server

    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Health")]
    public async Task<IActionResult> Get()
    {
        var isDbConnected = await CheckDbConnection();
        return !isDbConnected
            ? new ObjectResult(new { Status = "Unhealthy" }) { StatusCode = 500 }
            : (IActionResult)new OkObjectResult(new { Status = "Healthy" });
    }

    private async Task<bool> CheckDbConnection()
    {
        var connectionString = "Server=localhost,1433;Database=master;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=true";
        using var connection = new SqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            await using var cmd = new SqlCommand("SELECT 1", connection);
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (SqlException)
        {
            return false;
        }
    }

}

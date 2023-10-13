using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckLab.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{

    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Health")]
    public async Task<IActionResult> Get()
    {
       return new OkObjectResult(new { Status = "Healthy" });
    }
}

using Microsoft.Extensions.Diagnostics.HealthChecks;

public class SimpleHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            HealthCheckResult.Healthy("A healthy result."));
    }
}
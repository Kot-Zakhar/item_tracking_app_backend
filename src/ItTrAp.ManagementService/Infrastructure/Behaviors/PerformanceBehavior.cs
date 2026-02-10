using System.Diagnostics;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private readonly int _warnMs;

    public PerformanceBehavior(
        ILogger<PerformanceBehavior<TRequest, TResponse>> logger,
        int warnMs = 500)
    {
        _logger = logger;
        _warnMs = warnMs;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var reqName = typeof(TRequest).Name;
        _logger.LogDebug("Handling {RequestName}", reqName);

        var sw = Stopwatch.StartNew();
        try
        {
            return await next();
        }
        catch
        {
            throw;
        }
        finally
        {
            sw.Stop();
            var elapsedMs = sw.ElapsedMilliseconds;

            if (elapsedMs >= _warnMs)
            {
                _logger.LogWarning(
                    "Slow MediatR handler {RequestName} took {ElapsedMs} ms. {@Request}",
                    reqName, elapsedMs, request);
            }
            else
            {
                _logger.LogInformation(
                    "Handled {RequestName} in {ElapsedMs} ms.",
                    reqName, elapsedMs);
            }
        }
    }
}
using FluentResults;
using Microsoft.Extensions.Logging;

namespace tfp_collab_userspace_storage_database;

public static class LoggingExtensions
{
    public static void LogFluentResultErrors<T, S>(this ILogger<T> logger, Result<S> result, string operationDescription)
    {
        if (!result.IsSuccess)
        {
            logger.LogWarning($"Die Operation '{operationDescription}' war nicht erfolgreich.");

            if (result.Errors.Any())
            {
                logger.LogWarning($"Fehler für '{operationDescription}':");
                foreach (var error in result.Errors)
                {
                    logger.LogWarning($"- {error.Message}");
                    if (error.Metadata.Any())
                    {
                        logger.LogDebug($"  Metadaten: {string.Join(", ", error.Metadata.Select(kv => $"{kv.Key}={kv.Value}"))}");
                    }
                }
            }

            if (result.Reasons.Any())
            {
                logger.LogDebug($"Zusätzliche Gründe für '{operationDescription}':");
                foreach (var reason in result.Reasons)
                {
                    logger.LogDebug($"- {reason.Message}");
                    if (reason.Metadata.Any())
                    {
                        logger.LogDebug($"  Metadaten: {string.Join(", ", reason.Metadata.Select(kv => $"{kv.Key}={kv.Value}"))}");
                    }
                }
            }
        }
        else
        {
            logger.LogDebug($"Die Operation '{operationDescription}' war erfolgreich.");
        }
    }
}
using FluentResults;
using Microsoft.Extensions.Logging;

namespace tfpcollab.userspace.infrastructure.database;

public static class LoggingExtensions
{
    public static void LogFluentResultErrors<T, TS>(this ILogger<T> logger, Result<TS> result, string operationDescription)
    {
        if (!result.IsSuccess)
        {
            logger.LogWarning("Die Operation '{OperationDescription}' war nicht erfolgreich.", operationDescription);

            if (result.Errors.Count != 0)
            {
                logger.LogWarning("Fehler für '{OperationDescription}':", operationDescription);
                foreach (var error in result.Errors)
                {
                    logger.LogWarning("- {ErrorMessage}", error.Message);
                    if (error.Metadata.Count != 0)
                    {
                        logger.LogDebug("  Metadaten: {Join}", string.Join(", ", error.Metadata.Select(kv => $"{kv.Key}={kv.Value}")));
                    }
                }
            }

            if (result.Reasons.Count == 0) return;
            {
                logger.LogDebug("Zusätzliche Gründe für '{OperationDescription}':", operationDescription);
                foreach (var reason in result.Reasons)
                {
                    logger.LogDebug("- {ReasonMessage}", reason.Message);
                    if (reason.Metadata.Count != 0)
                    {
                        logger.LogDebug("  Metadaten: {Join}", string.Join(", ", reason.Metadata.Select(kv => $"{kv.Key}={kv.Value}")));
                    }
                }
            }
        }
        else
        {
            logger.LogDebug("Die Operation '{OperationDescription}' war erfolgreich.", operationDescription);
        }
    }
}
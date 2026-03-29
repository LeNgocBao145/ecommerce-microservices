using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Mappings;
using OrderApi.Application.Services;
using Polly;
using Polly.Retry;
using Serilog;

namespace OrderApi.Application
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Register Http client
            services.AddHttpClient<IOrderService, OrderService>(options =>
            {
                options.BaseAddress = new Uri(config["ApiGateway:BaseAddress"]!);
                options.Timeout = TimeSpan.FromSeconds(1);
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODA2MTA1NjAwIiwiaWF0IjoiMTc3NDYxNjkwNCIsImFjY291bnRfaWQiOiIwMTlkMmY2ODY2Y2U3NzViYmU0Nzg5MjEzYTQzMjJlYSIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa21xcGoweDBndGhhN3MxZWM3OW1xcnJhIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.JCd1OV2YSb7w7Ge7luFG_saXM3-d4IvArLQyjXtKRt6aLezgOUir24jWPspwvtJ9mn1mtEpCnfYm8mUUFQ1SIrH8yrBCQQOqROgDSW8Bh0coFIDDm3e4frPzD_gpxdYGJuAjFJD60P6Ulb1NG8DV8sGE1zzz49ufzd3zm5lHaIToIomDtKyZf032oUW1UIA7ESis53bBVLXC4cYXvdvDjP2HIiz2FD4TWx9oFpVifWIP54p0iDzM2oeFNymQV4KvcFDBWkicoHW7YdCSYov5B8LLriV4HReY9SpxkNIREfHgmkIdWNFFw1VAfR60sozUF1GqDxWm9Zgyz7Nxx47V2g";
                cfg.AddProfile<MappingProfile>();
            });

            var retryStrategy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
                UseJitter = true,
                BackoffType = DelayBackoffType.Constant,
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                OnRetry = args =>
                {
                    string message = $"Retrying due to: {args.Outcome.Exception?.Message ?? args.Outcome.Result?.ToString()}. Retry attempt: {args.AttemptNumber}";
                    Log.Debug(message);
                    return ValueTask.CompletedTask;
                }
            };

            services.AddResiliencePipeline("my-retry-pipeline", builder => builder.AddRetry(retryStrategy));

            return services;
        }
    }
}

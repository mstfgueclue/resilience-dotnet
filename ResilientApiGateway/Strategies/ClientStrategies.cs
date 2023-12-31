using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Timeout;

namespace ResilientApiGateway.Strategies;

public class ClientStrategies
{
    private RetryStrategyOptions<HttpResponseMessage> RetryStrategy { get; }
    private CircuitBreakerStrategyOptions<HttpResponseMessage> CircuitBreakerStrategy { get; }
    private FallbackStrategyOptions<HttpResponseMessage> FallbackStrategy { get; }
    private FallbackStrategyOptions<HttpResponseMessage> FallbackCircuitStrategy { get; }
    private TimeoutStrategyOptions TimeoutStrategy { get; }

    // Reactive Strategies
    public ResiliencePipeline<HttpResponseMessage> WaitAndRetry { get; set; }
    public ResiliencePipeline<HttpResponseMessage> CircuitBreaker { get; set; }
    public ResiliencePipeline<HttpResponseMessage> Fallback { get; set; }

    // Proactive Strategies
    public ResiliencePipeline<HttpResponseMessage> Timeout { get; set; }

    // Mixed Strategies
    public ResiliencePipeline<HttpResponseMessage> All { get; set; }
    public ResiliencePipeline<HttpResponseMessage> CircuitBreakerCombo { get; set; }


    public ClientStrategies(HttpClient client )
    {
        RetryStrategy = new RetryStrategyOptions<HttpResponseMessage>
        {
            Delay = TimeSpan.FromSeconds(1),
            MaxRetryAttempts = 3,
            BackoffType = DelayBackoffType.Constant,
            ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                .Handle<Exception>()
                .HandleResult(static result => !result.IsSuccessStatusCode),
            OnRetry = static args =>
            {
                Console.WriteLine($"Retrying {args.AttemptNumber+1} of 3");
                Console.WriteLine($"Delay is set to {args.RetryDelay} second");
                return default;
            }
        };

        TimeoutStrategy = new TimeoutStrategyOptions
        {
            Timeout = TimeSpan.FromSeconds(1.5),
            OnTimeout = static args =>
            {
                Console.WriteLine($"Timeout after {args.Timeout.TotalSeconds} seconds.");
                return default;
            }
        };

        CircuitBreakerStrategy = new CircuitBreakerStrategyOptions<HttpResponseMessage>
        {
            SamplingDuration = TimeSpan.FromSeconds(10),
            FailureRatio = 0.2,
            MinimumThroughput = 3,
            ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                .Handle<Exception>()
                .HandleResult(static result => !result.IsSuccessStatusCode),
            OnOpened = static args =>
            {
                Console.WriteLine($"Circuit opened. Delay: {args.BreakDuration} seconds.");
                return default;
            },
        };

        FallbackStrategy = new FallbackStrategyOptions<HttpResponseMessage>
        {
            ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                .HandleResult(static result => !result.IsSuccessStatusCode),
            FallbackAction = async args =>
            {
                var fallBackMessage = args.Outcome.Result != null
                    ? $"Fallback after {(int)args.Outcome.Result.StatusCode}: {args.Outcome.Result.ReasonPhrase}"
                    : $"Fallback after {args.Outcome.Exception?.Message}";
                Console.WriteLine(fallBackMessage);

                var response = await client.GetAsync("api/cars/2");
                return Outcome.FromResult(response);
            }
        };

        FallbackCircuitStrategy = new FallbackStrategyOptions<HttpResponseMessage>
        {
            ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                .Handle<BrokenCircuitException>(), // notice that this strategy only handles BrokenCircuitException
            FallbackAction = async args =>
            {
                var fallBackMessage = args.Outcome.Result != null
                    ? $"Fallback after {(int)args.Outcome.Result.StatusCode}: {args.Outcome.Result.ReasonPhrase}"
                    : $"Fallback after {args.Outcome.Exception?.Message}";
                Console.WriteLine(fallBackMessage);

                var response = await client.GetAsync("api/cars/2");
                return Outcome.FromResult(response);
            }
        };

        // See: https://www.pollydocs.org/strategies/retry.html
        WaitAndRetry = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(RetryStrategy)
            .Build();

        // See: https://www.pollydocs.org/strategies/timeout.html
        Timeout = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddTimeout(TimeoutStrategy)
            .Build();

        // See: https://www.pollydocs.org/strategies/circuit-breaker.html
        CircuitBreaker = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddCircuitBreaker(CircuitBreakerStrategy)
            .Build();

        // See: https://www.pollydocs.org/strategies/fallback.html
        Fallback = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddFallback(FallbackStrategy)
            .Build();

        All = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(RetryStrategy)
            .AddCircuitBreaker(CircuitBreakerStrategy)
            .AddTimeout(TimeoutStrategy)
            .AddFallback(FallbackStrategy)
            .Build();

        // The strategies are executed in the order they are added to the pipeline
        // e.g. 1. Retry, 2. Fallback, 3. Circuit Breaker
        // if 2. Circuit Breaker and 3. Fallback then it would not catch the BrokenCircuitException
        CircuitBreakerCombo = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(RetryStrategy)
            .AddFallback(FallbackCircuitStrategy)
            .AddCircuitBreaker(CircuitBreakerStrategy)
            .Build();
    }
}
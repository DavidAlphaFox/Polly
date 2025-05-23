﻿#nullable enable
namespace Polly.Caching;

/// <summary>
/// Provides a strongly-typed wrapper over a non-generic CacheProviderAsync.
/// </summary>
/// <typeparam name="TCacheFormat">The type of the objects in the cache.</typeparam>
internal sealed class AsyncGenericCacheProvider<TCacheFormat> : IAsyncCacheProvider<TCacheFormat>
{
    private readonly IAsyncCacheProvider _wrappedCacheProvider;

    internal AsyncGenericCacheProvider(IAsyncCacheProvider nonGenericCacheProvider) =>
        _wrappedCacheProvider = nonGenericCacheProvider ?? throw new ArgumentNullException(nameof(nonGenericCacheProvider));

    async Task<(bool, TCacheFormat?)> IAsyncCacheProvider<TCacheFormat>.TryGetAsync(string key, CancellationToken cancellationToken, bool continueOnCapturedContext)
    {
        (bool cacheHit, object? cached) = await _wrappedCacheProvider.TryGetAsync(key, cancellationToken, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);

        TCacheFormat? result = default;

        if (cacheHit)
        {
            result = (TCacheFormat?)cached;
        }

        return (cacheHit, result);
    }

    Task IAsyncCacheProvider<TCacheFormat>.PutAsync(string key, TCacheFormat? value, Ttl ttl, CancellationToken cancellationToken, bool continueOnCapturedContext) =>
        _wrappedCacheProvider.PutAsync(key, value, ttl, cancellationToken, continueOnCapturedContext);
}

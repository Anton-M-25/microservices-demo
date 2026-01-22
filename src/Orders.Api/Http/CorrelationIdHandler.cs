using System.Net.Http.Headers;

namespace Orders.Api.Http;

public sealed class CorrelationIdHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string HeaderName = "X-Correlation-Id";

    public CorrelationIdHandler(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var ctx = _httpContextAccessor.HttpContext;
        if (ctx is not null && ctx.Response.Headers.TryGetValue(HeaderName, out var corr))
        {
            request.Headers.TryAddWithoutValidation(HeaderName, corr.ToString());
        }

        return base.SendAsync(request, cancellationToken);
    }
}
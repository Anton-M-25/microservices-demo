using System.Net;
using System.Net.Http.Json;
using Orders.Api.Models;

namespace Orders.Api.Clients;

public sealed class CatalogApiClient
{
    private readonly HttpClient _http;

    public CatalogApiClient(HttpClient http) => _http = http;

    public async Task<ProductDto?> GetProductAsync(Guid id, CancellationToken ct = default)
    {
        using var resp = await _http.GetAsync($"/products/{id}", ct);

        if (resp.StatusCode == HttpStatusCode.NotFound)
            return null;

        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<ProductDto>(cancellationToken: ct);
    }
}

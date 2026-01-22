namespace Orders.Api.Options;

public sealed class CatalogApiOptions
{
    public const string SectionName = "CatalogApi";
    public string BaseUrl { get; init; } = default!;
}
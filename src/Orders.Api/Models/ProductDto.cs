namespace Orders.Api.Models;

public sealed record ProductDto(Guid Id, string Name, decimal Price);
namespace Orders.Api.Models;

public sealed record OrderDto(Guid Id, Guid ProductId, int Quantity, decimal UnitPrice);
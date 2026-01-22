namespace Orders.Api.Models;

public sealed record CreateOrderRequest(Guid ProductId, int Quantity);
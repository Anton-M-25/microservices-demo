using Microsoft.AspNetCore.Mvc;
using Orders.Api.Clients;
using Orders.Api.Models;

namespace Orders.Api.Controllers;

[ApiController]
[Route("orders")]
public sealed class OrdersController : ControllerBase
{
    private static readonly Dictionary<Guid, OrderDto> Orders = new();

    private readonly CatalogApiClient _catalog;

    public OrdersController(CatalogApiClient catalog) => _catalog = catalog;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest req, CancellationToken ct)
    {
        if (req.Quantity <= 0)
            return BadRequest("Quantity must be > 0");

        var product = await _catalog.GetProductAsync(req.ProductId, ct);
        if (product is null)
            return BadRequest("Invalid ProductId (not found in Catalog)");

        var order = new OrderDto(Guid.NewGuid(), product.Id, req.Quantity, product.Price);
        Orders[order.Id] = order;

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
        => Orders.TryGetValue(id, out var order) ? Ok(order) : NotFound();
}

using Catalog.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> Products =
    [
        new(Guid.NewGuid(), "Keyboard", 49.99m),
        new(Guid.NewGuid(), "Mouse", 19.99m)
    ];

    [HttpGet]
    public IActionResult GetAll() => Ok(Products);

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);
        return product is null ? NotFound() : Ok(product);
    }
}
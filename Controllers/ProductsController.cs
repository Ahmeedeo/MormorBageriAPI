using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MormorBageriAPI.Data;
using MormorBageriAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound("Produkten hittades inte.");

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductPrice(int id, [FromBody] decimal newPrice)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound("Produkten hittades inte.");

        product.Price = newPrice;
        await _context.SaveChangesAsync();

        return Ok(product);
    }
}
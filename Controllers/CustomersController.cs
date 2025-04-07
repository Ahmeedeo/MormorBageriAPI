using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MormorBageriAPI.Data;
using MormorBageriAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _context.Customers
            .Include(c => c.Address)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.ContactPerson,
                Address = new
                {
                    c.Address.Street,
                    c.Address.ZipCode,
                    c.Address.City
                }
            }).ToListAsync();

        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerWithOrders(int id)
    {
        var customer = await _context.Customers
            .Include(c => c.Address)
            .Include(c => c.Orders)
                .ThenInclude(o => o.OrderRows)
                    .ThenInclude(or => or.Product)
            .Where(c => c.Id == id)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.ContactPerson,
                Address = new
                {
                    c.Address.Street,
                    c.Address.ZipCode,
                    c.Address.City
                },
                Orders = c.Orders.Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    Products = o.OrderRows.Select(or => new
                    {
                        or.Product.Name,
                        or.Product.Price,
                        or.Quantity
                    })
                })
            }).FirstOrDefaultAsync();

        if (customer == null)
            return NotFound("Kund hittades inte.");

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
    {
        if (customer.Address == null)
            return BadRequest("Adress krävs.");

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCustomerWithOrders), new { id = customer.Id }, customer);
    }
}
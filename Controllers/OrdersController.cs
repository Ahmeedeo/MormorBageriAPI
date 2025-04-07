using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MormorBageriAPI.Data;
using MormorBageriAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] int? orderNumber, [FromQuery] DateTime? orderDate)
    {
        var query = _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderRows)
                .ThenInclude(or => or.Product)
            .AsQueryable();

        if (orderNumber.HasValue)
            query = query.Where(o => o.Id == orderNumber);

        if (orderDate.HasValue)
            query = query.Where(o => o.OrderDate.Date == orderDate.Value.Date);

        var orders = await query.Select(o => new
        {
            o.Id,
            o.OrderDate,
            CustomerName = o.Customer.Name,
            Products = o.OrderRows.Select(or => new
            {
                or.Product.Name,
                or.Product.Price,
                or.Quantity
            })
        }).ToListAsync();

        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
    {
        var customer = await _context.Customers.FindAsync(orderDto.CustomerId);
        if (customer == null)
            return NotFound("Kund hittades inte.");

        var order = new Order
        {
            CustomerId = orderDto.CustomerId,
            OrderDate = orderDto.OrderDate,
            OrderRows = new List<OrderRow>()
        };

        foreach (var item in orderDto.Products)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null)
                return NotFound($"Produkt med ID {item.ProductId} hittades inte.");

            order.OrderRows.Add(new OrderRow
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrders), new { orderNumber = order.Id }, order);
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MormorBageriAPI.Data;
using MormorBageriAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly AppDbContext _context;

    public SuppliersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSuppliers()
    {
        var suppliers = await _context.Suppliers
            .Include(s => s.RawMaterialSuppliers)
                .ThenInclude(rms => rms.RawMaterial)
            .Select(s => new
            {
                s.Id,
                s.Name,
                RawMaterials = s.RawMaterialSuppliers.Select(rms => new
                {
                    rms.RawMaterial.Name,
                    rms.Price
                })
            }).ToListAsync();

        return Ok(suppliers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplier(int id)
    {
        var supplier = await _context.Suppliers
            .Include(s => s.RawMaterialSuppliers)
                .ThenInclude(rms => rms.RawMaterial)
            .Where(s => s.Id == id)
            .Select(s => new
            {
                s.Id,
                s.Name,
                RawMaterials = s.RawMaterialSuppliers.Select(rms => new
                {
                    rms.RawMaterial.Name,
                    rms.Price
                })
            }).FirstOrDefaultAsync();

        if (supplier == null)
            return NotFound("Leverantör hittades inte.");

        return Ok(supplier);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSupplier([FromBody] string name)
    {
        var supplier = new Supplier { Name = name };
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSupplier), new { id = supplier.Id }, supplier);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSupplierName(int id, [FromBody] string newName)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null)
            return NotFound("Leverantör hittades inte.");

        supplier.Name = newName;
        await _context.SaveChangesAsync();

        return Ok(supplier);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null)
            return NotFound("Leverantör hittades inte.");

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

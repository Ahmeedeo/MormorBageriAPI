using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MormorBageriAPI.Data;
using MormorBageriAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class RawMaterialsController : ControllerBase
{
    private readonly AppDbContext _context;

    public RawMaterialsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRawMaterials()
    {
        var rawMaterials = await _context.RawMaterials
            .Include(r => r.RawMaterialSuppliers)
                .ThenInclude(rms => rms.Supplier)
            .Select(r => new
            {
                r.Id,
                r.Name,
                Suppliers = r.RawMaterialSuppliers.Select(rms => new
                {
                    rms.Supplier.Name,
                    rms.Price
                })
            }).ToListAsync();

        return Ok(rawMaterials);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchRawMaterial([FromQuery] string name)
    {
        var result = await _context.RawMaterials
            .Where(r => r.Name.Contains(name))
            .Include(r => r.RawMaterialSuppliers)
                .ThenInclude(rms => rms.Supplier)
            .Select(r => new
            {
                r.Id,
                r.Name,
                Suppliers = r.RawMaterialSuppliers.Select(rms => new
                {
                    rms.Supplier.Name,
                    rms.Price
                })
            }).ToListAsync();

        if (!result.Any())
            return NotFound("Ingen matchande råvara hittades.");

        return Ok(result);
    }
}
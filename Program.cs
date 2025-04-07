using Microsoft.EntityFrameworkCore;
using MormorBageriAPI.Data;
using MormorBageriAPI.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=bageri.db"));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Skapa databasen och seed-data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.RawMaterials.Any())
    {
        var mjolk = new RawMaterial { Name = "Mjölk" };
        var socker = new RawMaterial { Name = "Socker" };

        var leverantor1 = new Supplier { Name = "AhmedFOOD" };
        var leverantor2 = new Supplier { Name = "Rosengård C" };

        db.RawMaterials.AddRange(mjolk, socker);
        db.Suppliers.AddRange(leverantor1, leverantor2);
        db.SaveChanges();

        db.RawMaterialSuppliers.AddRange(
            new RawMaterialSupplier { RawMaterialId = mjolk.Id, SupplierId = leverantor1.Id, Price = 12.5m },
            new RawMaterialSupplier { RawMaterialId = mjolk.Id, SupplierId = leverantor2.Id, Price = 13m },
            new RawMaterialSupplier { RawMaterialId = socker.Id, SupplierId = leverantor2.Id, Price = 8.9m }
        );
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

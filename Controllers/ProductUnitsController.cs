using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsServer.DataBase;
using ProductsServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductUnitsController : ControllerBase
    {
        private readonly ProductsDBContext _context;

        public ProductUnitsController(ProductsDBContext context)
        {
            _context = context;
        }

        private ProductsDBContext Context => _context;

        [HttpGet]
        public async Task<List<ProductUnit>> Get()
        {
            using ProductsDBContext context = Context;
            return await context.Units.ToListAsync();
            //return await ManageDataDB.Instance.GetProductUnitsAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductUnit unit)
        {
            using ProductsDBContext context = Context;
            if (!await context.Units.AnyAsync(x => x.Id.Equals(unit.Id)))
            {
                context.Entry(unit).State = EntityState.Added;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Already have unit");
            }
            //await ManageDataDB.Instance.InsertProductUnitAsync(unit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(uint id, ProductUnit unit)
        {
            using ProductsDBContext context = Context;
            if (await context.Units.AnyAsync(x => x.Id.Equals(id)))
            {
                unit.Id = id;
                context.Entry(unit).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
            //await ManageDataDB.Instance.InsertProductUnitAsync(unit);
        }
    }
}

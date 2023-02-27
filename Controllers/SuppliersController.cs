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
    public class SuppliersController : ControllerBase
    {
        private readonly ProductsDBContext _context;

        public SuppliersController(ProductsDBContext context)
        {
            _context = context;
        }

        private ProductsDBContext Context => _context;

        [HttpGet]
        public async Task<List<Supplier>> Get()
        {
            using ProductsDBContext context = Context;
            return await context.Suppliers.ToListAsync();
            //return await ManageDataDB.Instance.GetSuppliersAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Supplier supplier)
        {
            using ProductsDBContext context = Context;
            if (!await context.Suppliers.AnyAsync(x => x.Id.Equals(supplier.Id)))
            {
                context.Entry(supplier).State = EntityState.Added;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Already have supplier");
            }
            //await ManageDataDB.Instance.InsertSupplierAsync(supplier);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(uint id, Supplier supplier)
        {
            using ProductsDBContext context = Context;
            if (await context.Suppliers.AnyAsync(x => x.Id.Equals(id)))
            {
                supplier.Id = id;
                context.Entry(supplier).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
            //await ManageDataDB.Instance.UpdateSupplierAsync(id, supplier);
        }
    }
}

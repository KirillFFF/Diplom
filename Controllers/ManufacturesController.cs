using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsServer.DataBase;
using ProductsServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturesController : ControllerBase
    {
        private readonly ProductsDBContext _context;

        public ManufacturesController(ProductsDBContext context)
        {
            _context = context;
        }

        private ProductsDBContext Context => _context;

        [HttpGet]
        public async Task<List<Manufacture>> Get()
        {
            using ProductsDBContext context = Context;
            return await context.Manufactures.OrderBy(x => x.Id).Select(x => new Manufacture { Id = x.Id, Name = x.Name, Country = x.Country }).ToListAsync();
            //return await ManageDataDB.Instance.GetManufacturesAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Manufacture manufacture)
        {
            using ProductsDBContext context = Context;
            if (!await context.Manufactures.AnyAsync(x => x.Id.Equals(manufacture.Id)))
            {
                context.Entry(manufacture).State = EntityState.Added;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Already have manufacture");
            }
            //await ManageDataDB.Instance.InsertManufactureAsync(manufacture);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(uint id, Manufacture manufacture)
        {
            using ProductsDBContext context = Context;
            Manufacture entity = await context.Manufactures.FindAsync(id);
            if (entity != null)
            {
                entity.Id = id;
                entity.Name = manufacture.Name;
                entity.Country = manufacture.Country;
                context.Entry(entity).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }

            //await ManageDataDB.Instance.UpdateManufactureAsync(id, manufacture);
        }
    }
}

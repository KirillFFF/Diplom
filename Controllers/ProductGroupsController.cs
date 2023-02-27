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
    public class ProductGroupsController : ControllerBase
    {
        private readonly ProductsDBContext _context;

        public ProductGroupsController(ProductsDBContext context)
        {
            _context = context;
        }

        private ProductsDBContext Context => _context;

        [HttpGet]
        public async Task<List<ProductGroup>> Get()
        {
            using ProductsDBContext context = Context;
            return await context.Groups.ToListAsync();
            //return await ManageDataDB.Instance.GetProductGroupsAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductGroup group)
        {
            using ProductsDBContext context = Context;
            if (!await context.Groups.AnyAsync(x => x.Id.Equals(group.Id)))
            {
                context.Entry(group).State = EntityState.Added;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Already have group");
            }
            //await ManageDataDB.Instance.InsertProductGroupAsync(group);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(uint id, ProductGroup group)
        {
            using ProductsDBContext context = Context;
            if (await context.Groups.AnyAsync(x => x.Id.Equals(id)))
            {
                group.Id = id;
                context.Entry(group).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}

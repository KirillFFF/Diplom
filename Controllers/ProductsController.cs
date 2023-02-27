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
    public class ProductsController : ControllerBase
    {
        private readonly ProductsDBContext _context;

        public ProductsController(ProductsDBContext context)
        {
            _context = context;
        }

        private ProductsDBContext Context => _context;

        [HttpGet]
        public async Task<List<Product>> GetAll()
        {
            using ProductsDBContext context = Context;
            return await context.Products.Select(x => new Product
            {
                IdName = x.IdName,
                Name = x.Name,
                Group = x.Group,
                Unit = x.Unit,
                Manufacture = new Manufacture
                {
                    Id = x.Manufacture.Id,
                    Name = x.Manufacture.Name,
                    Country = x.Manufacture.Country
                }
            }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Product> GetById(string id)
        {
            using ProductsDBContext context = Context;
            return await context.Products.Where(x => x.IdName.Equals(id)).Select(s => new Product
            {
                IdName = s.IdName,
                Name = s.Name,
                Group = s.Group,
                Unit = s.Unit,
                Manufacture = new Manufacture
                {
                    Id = s.Manufacture.Id,
                    Name = s.Manufacture.Name,
                    Country = s.Manufacture.Country
                }
            }).FirstOrDefaultAsync();
            //return await ManageDataDB.Instance.GetProductAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            using ProductsDBContext context = Context;
            if (!await context.Products.AnyAsync(x => x.IdName.Equals(product.IdName)))
            {
                context.Entry(product).State = EntityState.Added;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Already have product");
            }
            //await ManageDataDB.Instance.InsertProductAsync(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Product product)
        {
            using ProductsDBContext context = Context;
            Product entity = await context.Products.FindAsync(id);
            if (entity != null)
            {
                entity.IdName = id;
                entity.Name = product.Name;
                entity.Group = product.Group;
                entity.Unit = product.Unit;
                entity.Manufacture = product.Manufacture;
                context.Entry(entity).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
            //await ManageDataDB.Instance.UpdateProductAsync(id, product);
        }
    }
}

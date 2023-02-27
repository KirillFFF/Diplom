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
    public class NoteProductsController : ControllerBase
    {
        private readonly ProductsDBContext _context;

        public NoteProductsController(ProductsDBContext context)
        {
            _context = context;
        }

        private ProductsDBContext Context => _context;

        [HttpGet]
        public async Task<List<NoteProduct>> Get()
        {
            using ProductsDBContext context = Context;
            return await context.NoteProducts.OrderBy(x => x.Id).Select(x => new NoteProduct
            {
                Id = x.Id,
                Supplier = x.Supplier,
                Employee = new Employee
                {
                    Id = x.Employee.Id,
                    Name = x.Employee.Name,
                    Phone = x.Employee.Phone,
                    Address = x.Employee.Address,
                    Position = x.Employee.Position
                },
                Date = x.Date
            }).ToListAsync();
            //return await ManageDataDB.Instance.GetNoteProductsAsync();
        }

        [HttpGet("{id}")]
        public async Task<NoteProduct> GetById(uint id)
        {
            using ProductsDBContext context = Context;
            return await context.NoteProducts.Where(x => x.Id.Equals(id)).Select(x => new NoteProduct
            {
                Id = x.Id,
                Supplier = x.Supplier,
                Employee = new Employee
                {
                    Id = x.Employee.Id,
                    Name = x.Employee.Name,
                    Phone = x.Employee.Phone,
                    Address = x.Employee.Address,
                    Position = x.Employee.Position
                },
                Date = x.Date
            }).FirstOrDefaultAsync();
            //return await ManageDataDB.Instance.GetNoteProductAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post(NoteProduct note)
        {
            using ProductsDBContext context = Context;
            if (!await context.NoteProducts.AnyAsync(x => x.Id.Equals(note.Id)))
            {
                context.Entry(note).State = EntityState.Added;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Already have note");
            }
            //await ManageDataDB.Instance.InsertNoteProductAsync(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(uint id, NoteProduct note)
        {
            using ProductsDBContext context = Context;
            NoteProduct entity = await context.NoteProducts.FindAsync(id);
            if (entity != null)
            {
                entity.Id = id;
                entity.Supplier = note.Supplier;
                entity.Employee = note.Employee;
                entity.Date = note.Date;
                context.Entry(entity).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
            //await ManageDataDB.Instance.UpdateNoteProductAsync(id, product);
        }
    }
}

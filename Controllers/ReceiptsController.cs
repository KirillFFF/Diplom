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
    public class ReceiptsController : ControllerBase
    {
        private readonly ProductsDBContext _context;

        public ReceiptsController(ProductsDBContext context)
        {
            _context = context;
        }

        private ProductsDBContext Context => _context;

        [HttpGet]
        public async Task<List<Receipt>> GetAll()
        {
            using ProductsDBContext context = Context;
            return await context.Receipts.Select(x => new Receipt
            {
                Id = x.Id,
                Employee = new Employee
                {
                    Id = x.Employee.Id,
                    Name = x.Employee.Name,
                    Address = x.Employee.Address,
                    Phone = x.Employee.Phone,
                    Position = x.Employee.Position
                },
                Date = x.Date
            }).ToListAsync();
        }
    }
}

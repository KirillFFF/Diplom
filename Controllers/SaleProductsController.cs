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
    public class SaleProductsController : ControllerBase
    {
        private readonly ProductsDBContext _context;

        public SaleProductsController(ProductsDBContext context)
        {
            _context = context;
        }

        private ProductsDBContext Context => _context;

        [HttpGet]
        public async Task<List<SaleProduct>> GetAll()
        {
            using ProductsDBContext context = Context;
            return await context.SaleProducts.Select(x => new SaleProduct
            {
                Id = x.Id,
                Product = new ArrivalProduct
                {
                    Id = x.Product.Id,
                    Note = new NoteProduct
                    {
                        Id = x.Product.Note.Id,
                        Supplier = x.Product.Note.Supplier,
                        Employee = new Employee
                        {
                            Id = x.Product.Note.Employee.Id,
                            Name = x.Product.Note.Employee.Name,
                            Phone = x.Product.Note.Employee.Phone,
                            Address = x.Product.Note.Employee.Address,
                            Position = x.Product.Note.Employee.Position
                        },
                        Date = x.Product.Note.Date
                    },
                    Product = new Product
                    {
                        IdName = x.Product.Product.IdName,
                        Name = x.Product.Product.Name,
                        Group = x.Product.Product.Group,
                        Unit = x.Product.Product.Unit,
                        Manufacture = new Manufacture
                        {
                            Id = x.Product.Product.Manufacture.Id,
                            Name = x.Product.Product.Manufacture.Name,
                            Country = x.Product.Product.Manufacture.Country
                        }
                    },
                    Count = x.Product.Count,
                    ValidUntil = x.Product.ValidUntil,
                    PurchaseCost = x.Product.PurchaseCost,
                    SellCost = x.Product.SellCost
                },
                Receipt = new Receipt
                {
                    Id = x.Receipt.Id,
                    Employee = new Employee
                    {
                        Id = x.Receipt.Employee.Id,
                        Name = x.Receipt.Employee.Name,
                        Phone = x.Receipt.Employee.Phone,
                        Address = x.Receipt.Employee.Address,
                        Position = x.Receipt.Employee.Position
                    },
                    Date = x.Receipt.Date
                },
                Count = x.Count
            }).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(List<SaleProduct> sales)
        {
            using ProductsDBContext context = Context;
            context.Entry(sales[0].Receipt).State = EntityState.Added;
            sales.ForEach(x =>
            {
                context.Entry(x).State = EntityState.Added;
            });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ProductsServer.DataBase;
using ProductsServer.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ProductsServer.Bot.Classes;
using Telegram.Bot;

namespace ProductsServer.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ArrivalProductsController : ControllerBase
    {
        private readonly ProductsDBContext _context;

        public ArrivalProductsController(ProductsDBContext context)
        {
            _context = context;
        }

        private ProductsDBContext Context => _context;

        [HttpGet]
        public async Task<List<ArrivalProduct>> GetAll()
        {
            //return await _context.ArrivalProducts.ToListAsync();
            using ProductsDBContext context = Context;
            return await context.ArrivalProducts.Select(x => new ArrivalProduct
            {
                Id = x.Id,
                Note = new NoteProduct
                {
                    Id = x.Note.Id,
                    Supplier = x.Note.Supplier,
                    Employee = new Employee
                    {
                        Id = x.Note.Employee.Id,
                        Name = x.Note.Employee.Name,
                        Phone = x.Note.Employee.Phone,
                        Address = x.Note.Employee.Address,
                        Position = x.Note.Employee.Position
                    },
                    Date = x.Note.Date
                },
                Product = new Product
                {
                    IdName = x.Product.IdName,
                    Name = x.Product.Name,
                    Group = x.Product.Group,
                    Unit = x.Product.Unit,
                    Manufacture = new Manufacture
                    {
                        Id = x.Product.Manufacture.Id,
                        Name = x.Product.Manufacture.Name,
                        Country = x.Product.Manufacture.Country
                    }
                },
                Count = x.Count,
                ValidUntil = x.ValidUntil,
                PurchaseCost = x.PurchaseCost,
                SellCost = x.SellCost
            }).OrderBy(x => x.Id).ToListAsync();
            //return await ManageDataDB.Instance.GetArrivalProductsAsync();
        }

        [HttpGet("Remains")]
        public async Task<List<ArrivalProduct>> GetRemains()
        {
            using ProductsDBContext context = Context;
            return await context.ArrivalProducts.FromSqlRaw(
                "SELECT `Приход товаров`.`Код`, `Приход товаров`.`ТТН`, `Приход товаров`.`Товар`, (`Приход товаров`.`Количество` - IFNULL(`Расход товаров`.`Количество`, 0) - IFNULL(`Списание товаров`.`Количество`, 0)) AS 'Количество', `Приход товаров`.`Годен до`, `Приход товаров`.`Цена закупки`, `Приход товаров`.`Цена продажи`" +
                "FROM `Приход товаров` LEFT JOIN `Расход товаров`" +
                "ON `Расход товаров`.`Товар` = `Приход товаров`.`Код` LEFT JOIN `Списание товаров`" +
                "ON `Списание товаров`.`Товар` = `Приход товаров`.`Код`").Select(x => new ArrivalProduct
                {
                    Id = x.Id,
                    Note = new NoteProduct
                    {
                        Id = x.Note.Id,
                        Supplier = x.Note.Supplier,
                        Employee = new Employee
                        {
                            Id = x.Note.Employee.Id,
                            Name = x.Note.Employee.Name,
                            Phone = x.Note.Employee.Phone,
                            Address = x.Note.Employee.Address,
                            Position = x.Note.Employee.Position
                        },
                        Date = x.Note.Date
                    },
                    Product = new Product
                    {
                        IdName = x.Product.IdName,
                        Name = x.Product.Name,
                        Group = x.Product.Group,
                        Unit = x.Product.Unit,
                        Manufacture = new Manufacture
                        {
                            Id = x.Product.Manufacture.Id,
                            Name = x.Product.Manufacture.Name,
                            Country = x.Product.Manufacture.Country
                        }
                    },
                    Count = x.Count,
                    ValidUntil = x.ValidUntil,
                    PurchaseCost = x.PurchaseCost,
                    SellCost = x.SellCost
                }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ArrivalProduct> GetById(uint id)
        {
            using ProductsDBContext context = Context;
            return await context.ArrivalProducts.Where(x => x.Id.Equals(id)).Select(x => new ArrivalProduct
            {
                Id = x.Id,
                Note = new NoteProduct
                {
                    Id = x.Note.Id,
                    Supplier = x.Note.Supplier,
                    Employee = new Employee
                    {
                        Id = x.Note.Employee.Id,
                        Name = x.Note.Employee.Name,
                        Phone = x.Note.Employee.Phone,
                        Address = x.Note.Employee.Address,
                        Position = x.Note.Employee.Position
                    },
                    Date = x.Note.Date
                },
                Product = new Product
                {
                    IdName = x.Product.IdName,
                    Name = x.Product.Name,
                    Group = x.Product.Group,
                    Unit = x.Product.Unit,
                    Manufacture = new Manufacture
                    {
                        Id = x.Product.Manufacture.Id,
                        Name = x.Product.Manufacture.Name,
                        Country = x.Product.Manufacture.Country
                    }
                },
                Count = x.Count,
                ValidUntil = x.ValidUntil,
                PurchaseCost = x.PurchaseCost,
                SellCost = x.SellCost
            }).FirstOrDefaultAsync();
            //return await ManageDataDB.Instance.GetArrivalProductAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ArrivalProduct arrival)
        {
            using ProductsDBContext context = Context;
            if (!await context.ArrivalProducts.AnyAsync(x => x.Id.Equals(arrival.Id)))
            {
                Context.Entry(arrival).State = EntityState.Added;
                await Context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Already have arrival");
            }
            //await ManageDataDB.Instance.InsertArrivalProductAsync(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(uint id, ArrivalProduct arrival)
        {
            using ProductsDBContext context = Context;
            if (await context.ArrivalProducts.AnyAsync(x => x.Id.Equals(id)))
            {
                //Context.Entry(entity).State = EntityState.Modified;
                //await Context.SaveChangesAsync();
                await ManageDataDB.Instance.UpdateArrivalProductAsync(id, arrival);
                return Ok();
            }
            else
            {
                return NotFound();
            }
            //await ManageDataDB.Instance.UpdateArrivalProductAsync(id, product);
        }
    }
}

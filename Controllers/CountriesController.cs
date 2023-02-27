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
    public class CountriesController : ControllerBase
    {
        private readonly ProductsDBContext _context;

        public CountriesController(ProductsDBContext context)
        {
            _context = context;
        }

        private ProductsDBContext Context => _context;

        [HttpGet]
        public async Task<List<Country>> Get()
        {
            using ProductsDBContext context = Context;
            return await context.Countries.ToListAsync();
            //return await ManageDataDB.Instance.GetCountriesAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Country country)
        {
            using ProductsDBContext context = Context;
            if (!await context.Countries.AnyAsync(x => x.Id.Equals(country.Id) || x.IdName.Equals(country.IdName) || x.ShortName.Equals(country.ShortName) || x.FullName.Equals(country.FullName)))
            {
                context.Entry(country).State = EntityState.Added;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Already have country");
            }
            //await ManageDataDB.Instance.InsertCountryAsync(country);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(uint id, Country country)
        {
            using ProductsDBContext context = Context;
            if (await context.Countries.AnyAsync(x => x.Id.Equals(id)))
            {
                country.Id = id;
                context.Entry(country).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }

            //await ManageDataDB.Instance.UpdateCountryAsync(id, country);
        }
    }
}

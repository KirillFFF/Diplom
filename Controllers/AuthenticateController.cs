using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductsServer.Classes;
using ProductsServer.DataBase;
using ProductsServer.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly ProductsDBContext _context;

        public AuthenticateController(IConfiguration configuration, ProductsDBContext context)
        {
            Configuration = configuration;
            _context = context;
        }

        private ProductsDBContext Context => _context;
        private IConfiguration Configuration { get; set; }

        [Route("AppLogin")]
        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            string hwid = Request.Headers.ToList().Find(header => header.Key.Equals("HWID")).Value;

            if (string.IsNullOrEmpty(hwid))
                return BadRequest("Not found HWID in request");

            User user = await Authenticate(hwid, userLogin);

            if (user != null)
                return Ok(TokenGenerate(hwid, user));

            return NotFound("User not found");
        }

        [Authorize]
        [Route("GetUser")]
        [HttpPost]
        public async Task<IActionResult> GetUser(UserLogin userLogin)
        {
            IEnumerable<Claim> currentUser = HttpContext.User.Claims;
            string hwid = currentUser.FirstOrDefault(x => x.Type.Equals(ClaimTypes.SerialNumber))?.Value;
            return Ok(await Authenticate(hwid, userLogin));
        }

        private async Task<User> Authenticate(string hwid, UserLogin userLogin)
        {
            using ProductsDBContext context = Context;
            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE `Пользователи` SET `HWID` = EncryptedString({hwid}) WHERE `Логин` = {userLogin.Login} AND `Пароль` = EncryptedString({userLogin.Password}) AND `HWID` IS NULL");
            return (await context.Users.FromSqlInterpolated(
                $"SELECT `Пользователи`.`Сотрудник`, `Пользователи`.`Логин`, `Пользователи`.`Доступ`, `Пользователи`.ChatId FROM `Пользователи` WHERE `Логин` = {userLogin.Login} AND `Пароль` = EncryptedString({userLogin.Password}) AND `HWID` = EncryptedString({hwid})")
                .Select(x => new User
            {
                Employee = new Employee
                {
                    Id = x.Employee.Id,
                    Name = x.Employee.Name,
                    Address = x.Employee.Address,
                    Phone = x.Employee.Phone,
                    Position = x.Employee.Position
                },
                Login = x.Login,
                AccessLevel = x.AccessLevel,
                ChatId = x.ChatId
            }).ToListAsync()).FirstOrDefault();

            //return await ManageDataDB.Instance.GetUserAsync(hwid, userLogin);
        }
        private string TokenGenerate(string hwid, User user)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Login),
                new Claim(ClaimTypes.Role, user.AccessLevel.Name),
                new Claim(ClaimTypes.SerialNumber, hwid),
                new Claim(ClaimTypes.Dns, IPAddressClient.Instance.Get(HttpContext)),
                new Claim(ClaimTypes.UserData, user.Employee.Name)
            };

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(Configuration["JWT:Issuer"], Configuration["JWT:Audience"], claims: claims, expires: DateTime.Now.AddHours(1), signingCredentials: credentials));
        }
    }
}

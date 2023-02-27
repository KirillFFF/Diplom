using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductsServer.Bot.Classes;
using ProductsServer.Classes;
using ProductsServer.DataBase;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ProductsServer.Middlewares;
using ProductsServer.Models;

namespace ProductsServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TelegramBot>();
            services.AddScoped<ProductsDBContext>();
            services.AddDbContextPool<ProductsDBContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("ProductsServer"));
            });

            services.AddHttpContextAccessor();
            services.AddControllers().AddNewtonsoftJson();
            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"])),
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = async (context) =>
                        {
                            IEnumerable<Claim> currentUser = (context.SecurityToken as JwtSecurityToken).Claims;

                            ProductsDBContext dBContext = context.HttpContext.RequestServices.GetRequiredService<ProductsDBContext>();
                            string login = currentUser.First(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;

                            if (currentUser.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.SerialNumber))?.Value.Equals(context.Request.Headers.ToList().Find(header => header.Key.Equals("HWID")).Value) == true &&
                                currentUser.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Dns)).Value?.Equals(IPAddressClient.Instance.Get(context)) == true &&
                                currentUser.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Role)).Value?.Equals(
                                    (await dBContext.Users.Where(x => x.Login.Equals(login)).Select(x => new User { AccessLevel = x.AccessLevel }).FirstOrDefaultAsync())?.AccessLevel?.Name) == true)
                            {
                                context.Success();
                            }
                            else
                            {
                                context.Fail("Token validation failed");
                            }
                        }
                    };
                });
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductsServer", Version = "v1" });
            });
            //services.AddMemoryCache();
            //services.Configure<IpRateLimitOptions>(options =>
            //{
            //    options.EnableEndpointRateLimiting = true;
            //    options.StackBlockedRequests = false;
            //    options.HttpStatusCode = 429;
            //    options.RealIpHeader = "X-Real-IP";
            //    options.ClientIdHeader = "X-ClientId";
            //    options.GeneralRules = new List<RateLimitRule>
            //    {
            //        new RateLimitRule
            //        {
            //            Endpoint = "*",
            //            Period = "1s",
            //            Limit = 10,
            //        },
            //        new RateLimitRule
            //        {
            //            Endpoint = "*",
            //            Period = "1m",
            //            Limit = 300,
            //        }
            //    };
            //});
            //services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            //services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            //services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            //services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            //services.AddInMemoryRateLimiting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductsServer v1"));
            }

            app.UseDeveloperExceptionPage();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            //app.UseIpRateLimiting();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            serviceProvider.GetRequiredService<TelegramBot>();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            //ProductsDB.SetConnection = Configuration.GetConnectionString("ProductsServer");
            //await TelegramBot.SetBotClientAsync(Configuration["TelegramBot:Token"], Configuration["TelegramBot:Host"]);
        }
    }
}
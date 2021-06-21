using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace ApertureScience
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
            AuthentificationOptions.ISSUER = Configuration["AuthentificationOptions:Issuer"] ?? "MyAuthServer";
            AuthentificationOptions.AUDIENCE = Configuration["AuthentificationOptions:Audience"] ?? "MyAuthClient";
            AuthentificationOptions.LIFETIME = int.Parse(Configuration["AuthentificationOptions:Lifetime"] ?? "1");
            AuthentificationOptions.KEY = Configuration["AuthentificationOptions:Key"] ?? "mysupersecret_secretkey!123";

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthentificationOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthentificationOptions.AUDIENCE,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = AuthentificationOptions.SECURITY_KEY,
                        ValidateLifetime = false,
                    };
                });

            services.AddCors();
            services.AddControllers();

            services.AddDbContext<EmployeesManager>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("EmployeesDatabase")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                var AuthentificationRoute = "api" + Configuration["Routs:Authentification"];
                var EmployeesRoute = "api" + Configuration["Routs:Employees"];
                var PhotoRoute = EmployeesRoute + "/{employeeId:int}/photos";

                ConfigurableRoute.AddRoute("Authentification", AuthentificationRoute);
                ConfigurableRoute.AddRoute("Employees", EmployeesRoute);
                ConfigurableRoute.AddRoute("Photo", PhotoRoute);

                endpoints.MapControllers();
            });
        }
    }
}

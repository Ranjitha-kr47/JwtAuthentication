using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Api_project.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;


namespace Api_project
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
            services.AddControllers();
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(opt => opt.UseMySQL(connectionString));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // ===== Add our DbContext ========
            services.AddDbContext<ApplicationContext>();

            // ===== Add Identity ========
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                // .AddJwtBearer(cfg =>
                // {
                //     cfg.RequireHttpsMetadata = false;
                //     cfg.SaveToken = true;
                //     cfg.TokenValidationParameters = new TokenValidationParameters
                //     {
                //         ValidIssuer = Configuration["JwtIssuer"],
                //         ValidAudience = Configuration["JwtIssuer"],
                //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                //         ClockSkew = TimeSpan.Zero // remove delay of token when expire
                //     };
                // });
            //     .AddJwtBearer(options =>  
            // {  
            //     options.SaveToken = true;  
            //     options.RequireHttpsMetadata = false;  
            //     options.TokenValidationParameters = new TokenValidationParameters()  
            //     {  
            //         ValidateIssuer = true,  
            //         ValidateAudience = true,  
            //         ValidAudience = Configuration["JWT:ValidAudience"],  
            //         ValidIssuer = Configuration["JWT:ValidIssuer"],  
            //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))  
            //     };  
            // });  

            .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["JWT:JwtIssuer"],
                        ValidAudience = Configuration["JWT:JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });
        }  

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ApplicationContext dbContext )
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
    
        
            app.UseAuthentication();  
            app.UseAuthorization();  

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //dbContext.Database.EnsureCreated();
        }
    }
}

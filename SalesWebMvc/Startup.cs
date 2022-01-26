using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using SalesWebMvc.Data;

namespace SalesWebMvc {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

    services.AddDbContext<SalesWebMvcContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("SalesWebMvcContext"), builder => //entre aspas deve ficar o nome da classe criada dentro da pasta Data (Sales WebMvcContext)
builder.MigrationsAssembly("SalesWebMvc"))); //entre aspas é o nome do projeto(SalesWebMvc)

            services.AddScoped<SeedingService>(); //aqui se registra o nosso serviço no sistema de injeção de independecia da aplicação
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,SeedingService seedingservice) {
            if (env.IsDevelopment()) { //Se eu estou em um perfil de desenv. 
                app.UseDeveloperExceptionPage();
                seedingservice.Seed();
            }
            else                       //Se eu estou em um perfil de produção
            {                      
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Departments}/{action=Index}/{id?}");
            });
        }
    }
}

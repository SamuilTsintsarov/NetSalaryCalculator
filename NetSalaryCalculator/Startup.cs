namespace NetSalaryCalculator
{
    using BusinessLogicLayer.Models;
    using BusinessLogicLayer.Models.Contracts;
    using BusinessLogicLayer.Services;
    using BusinessLogicLayer.Services.Contracts;
    using DataAccessLayer;
    using DataAccessLayer.Repository;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NetSalaryCalculator.Middleware;

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
            services.AddScoped<ICalculatorService, CalculatorService>();
            services.AddScoped<ITaxes, Taxes>();
            services.AddScoped<ITaxPayer, TaxPayer>();
            services.AddScoped<ICalculatorDAL, CalculatorDAL>();
            services.AddTransient<IFakeCalculatorDbContext, FakeCalculatorDbContext>();

            services.AddControllers();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

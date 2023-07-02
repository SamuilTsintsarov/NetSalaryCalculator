namespace CalculatorTests
{
    using BusinessLogicLayer.Services;
    using BusinessLogicLayer.Services.Contracts;
    using DataAccessLayer;
    using DataAccessLayer.Repository;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;

    public class Setup
    {
        private readonly IHostBuilder _defaultBuilder;
        private IServiceProvider _services;
        private bool _built = false;

        public Setup()
        {
            _defaultBuilder = Host.CreateDefaultBuilder();
        }

        public IServiceProvider Services => _services ?? Build();

        private IServiceProvider Build()
        {
            if (_built)
                throw new InvalidOperationException("Build can only be called once.");
            _built = true;

            _defaultBuilder.ConfigureServices((context, services) =>
            {
                services.AddScoped<ICalculatorService, CalculatorService>();
                services.AddScoped<ICalculatorDAL, CalculatorDAL>();
                services.AddTransient<IFakeCalculatorDbContext, FakeCalculatorDbContext>();
            });

            _services = _defaultBuilder.Build().Services;
            return _services;
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Contracts;
using BusinessLogicLayer.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Caching.Memory;
using NetSalaryCalculator.Models;

namespace NetSalaryCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorService _calculatorService;
        private readonly ILogger<CalculatorController> _logger;
        private IMemoryCache _cache;

        public CalculatorController(ILogger<CalculatorController> logger, ICalculatorService calculatorService, IMemoryCache cache)
        {
            _logger = logger;
            _calculatorService = calculatorService;
            _cache = cache;
        }

        [HttpPost]
        [Route("calculate")]
        public async Task<ActionResult<ITaxes>> Calculate([FromBody] TaxPayer taxPayer)
        {
            try
            {
                var cacheKey = $"GrossIncome:{taxPayer.GrossIncome},CharitySpent:{taxPayer.CharitySpent}";

                var taxSettings = await _calculatorService.GetTaxSettings();

                if (_cache.TryGetValue(cacheKey, out CachedTaxesWrapperModel cachedTaxesWrapper))
                {
                    // If the Taxes that are cached are using the same tax settings, they will be exactly the same, so return them from the cache
                    if (cachedTaxesWrapper.TaxSettings.Equals(taxSettings))
                    {
                        return Ok(cachedTaxesWrapper.Taxes);
                    }

                    //someone changed the settings in the database. The cached Taxes is no longer valid
                    _cache.Remove(cacheKey);   
                }

                var taxes = _calculatorService.CalculateTaxes(taxPayer, taxSettings);

                _cache.Set(cacheKey, new CachedTaxesWrapperModel { Taxes = taxes, TaxSettings = taxSettings });

                return Ok(taxes);
            }
            catch (ArgumentException ex)
            {
                // logger not implemented let us assume that it is logged somewhere
                //_logger.LogError(ex.Message, new {TaxPayer = taxPayer, Exception = ex});

                var errorMessage = "Please revise your Tax Payer data. If you believe your data is correct, please contact the Support";

                return StatusCode(StatusCodes.Status400BadRequest, errorMessage);
            }
            catch (Exception ex)
            {
                // logger not implemented let us assume that it is logged somewhere
                //_logger.LogError(ex.Message, new {TaxPayer = taxPayer, Exception = ex});

                var errorMessage = "We are experiencing some minor issues right now. Your request is logged and we are working on fixing it.";

                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
        }
    }
}

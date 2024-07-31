using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Data;

namespace TestApi.Services.ValidatorService
{
    public class ValidatorService : IValidatorService
    {
        
        private readonly IHttpContextAccessor _httpContext;
        private readonly DataContext _context;
        private readonly ILogger<ValidatorService> _logger;
        public ValidatorService(IHttpContextAccessor httpContext, DataContext context, ILogger<ValidatorService> logger)
        {
            _logger = logger;
            _context = context;
            _httpContext = httpContext;

        }
        
        public string PageValidation(int Page, int Limit)
        {
            var result = "";
            var limitCondition = 100;

            if (Limit > limitCondition)
            {
                result = $"The parameter value 'Limit' couldn't be greater than {limitCondition}";
            }
            return result;
        }
    }
}
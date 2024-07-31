using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Services.ValidatorService
{
    public interface IValidatorService
    {
         string PageValidation(int Page, int Limit);        
    }
}
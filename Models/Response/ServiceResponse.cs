using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Response
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public PageResponse Page { get; set; } = new PageResponse();
        public string Code { get; set; } = "";
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
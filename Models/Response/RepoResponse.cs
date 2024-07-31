using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Response
{
    public class RepoResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
    }
}
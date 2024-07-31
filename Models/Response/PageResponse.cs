using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Response
{
    public class PageResponse
    {
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 1;
    }
}
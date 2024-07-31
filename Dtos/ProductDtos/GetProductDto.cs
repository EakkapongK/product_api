using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Dtos.ProductDtos
{
    public class GetProductDto
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int ItemNo { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        public string? BarCode { get; set; }

        public string? Remark { get; set; }

        public string CreageBy { get; set; } = null!;

        public DateTime CteateDate { get; set; }

        public string UpdateBy { get; set; } = null!;

        public DateTime UpdateDate { get; set; }
    }
}
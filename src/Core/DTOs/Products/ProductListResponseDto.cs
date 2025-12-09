using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Products
{
    public class ProductListResponseDto
    {
        public int TotalCount { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}

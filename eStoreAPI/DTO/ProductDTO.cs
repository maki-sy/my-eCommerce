using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStoreAPI.DTO
{
    public class ProductDTO
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int? UnitsInStock { get; set; }
        public IFormFile? imageFile {  get; set; }
    }
}

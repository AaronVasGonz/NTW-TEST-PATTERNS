using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NTW_TEST_PATTERNS.Models.EFModels
{
   public  class Product_Image
    {
        public int? ProductImageId { get; set; }

        public int? ProductId { get; set; }

        public string ImageUrl { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOS;
public class ProductRequest
{
    public string ProductName { get; set; }
    public string SupplierName { get; set; }

    public string CategoryName { get; set; }

    public string Unit { get; set; }

    public int Price { get; set; }

    public int Stock { get; set; }

    public string Status { get; set; }
}

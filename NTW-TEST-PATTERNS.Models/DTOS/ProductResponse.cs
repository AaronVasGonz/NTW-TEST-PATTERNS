using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOS;

public class ProductResponse
{
    public int? Id { get; set; }

    public string? ProductName { get; set; }

    public string? CategoryName { get; set; }

    public string? SupplierName { get; set; }

    public decimal? Price { get; set; }

    public string? Unit { get; set; }

    public int? Stock { get; set; }

    public string? Status { get; set; }

    public List<string>? Images { get; set; }

}

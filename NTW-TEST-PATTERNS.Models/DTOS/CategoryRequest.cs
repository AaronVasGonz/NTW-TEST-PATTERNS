using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOS;

public class CategoryRequest
{
    public int? Id { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
}

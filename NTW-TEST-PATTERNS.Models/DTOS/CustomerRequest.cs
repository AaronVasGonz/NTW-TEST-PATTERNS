using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOS;
public class CustomerRequest
{
    public int? CustomerId { get; set; }
    public string? Name { get; set; }
    public string? Contact { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public int? UserId { get; set; }

}

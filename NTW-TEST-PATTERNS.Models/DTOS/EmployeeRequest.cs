using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOS;

public class EmployeeRequest
{
    public int? EmployeeId { get; set; }
    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public DateTime? BirthDate { get; set; }

    public IFormFile? Photo { get; set; }

    public string? Notes { get; set; }

    public int? UserId { get; set; }

}

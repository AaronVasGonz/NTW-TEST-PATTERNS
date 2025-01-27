using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOS;
using Service.Services;
using Service.Services.validations.Suppliers;
using Services;
using System.ComponentModel.DataAnnotations;

namespace NTW_TEST_PATTERNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierApiController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly IValidateSuppliersService _validateSuppliersService;
        public SupplierApiController(ISupplierService supplierService, IValidateSuppliersService validateSuppliersService)
        {
            _supplierService = supplierService;
            _validateSuppliersService = validateSuppliersService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetSuppliers()
        {
            var suppliers = await _supplierService.GetSuppliersAsync();
            if (suppliers == null)
            {
                throw new KeyNotFoundException("Suppliers not found)");
            }
            return Ok(suppliers);
        }

        [HttpGet("supplier/{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                throw new KeyNotFoundException("Supplier not found");
            }
            return Ok(supplier);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveSupplier([FromBody] SupplierRequest supplierRequest)
        {

            //Validate SupplierRequest
            if (supplierRequest == null)
            {
                throw new ArgumentNullException("SupplierRequest is null");
            }

            //validate supplierRequest
            _validateSuppliersService.ValidateSupplierRequest(supplierRequest);

            var supplier = new Supplier
            {
                SupplierName = supplierRequest.SupplierName,
                ContactName = supplierRequest.ContactName,
                Address = supplierRequest.Address,
                City = supplierRequest.City,
                PostalCode = supplierRequest.PostalCode,
                Country = supplierRequest.Country,
                Phone = supplierRequest.Phone,
                Status = supplierRequest.Status
            };

            var savedSupplier = await _supplierService.SaveSupplierAsync(supplier);

            return savedSupplier == null ? throw new ValidationException("Supplier not saved") : (IActionResult)Ok("");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateSupplier([FromBody] SupplierRequest supplierRequest)
        {
            //Validate SupplierRequest
            if (supplierRequest == null)
            {
                throw new ArgumentNullException("SupplierRequest is null");
            }
            //validate supplierRequest
            _validateSuppliersService.ValidateSupplierRequest(supplierRequest);
            var supplier = new Supplier
            {
                SupplierId = supplierRequest.SupplierId,
                SupplierName = supplierRequest.SupplierName,
                ContactName = supplierRequest.ContactName,
                Address = supplierRequest.Address,
                City = supplierRequest.City,
                PostalCode = supplierRequest.PostalCode,
                Country = supplierRequest.Country,
                Phone = supplierRequest.Phone,
                Status = supplierRequest.Status
            };

            var updatedSupplier = await _supplierService.SaveSupplierAsync(supplier);
            return updatedSupplier == null ? throw new ValidationException("Supplier not updated") : (IActionResult)Ok("");
        }
    }

}


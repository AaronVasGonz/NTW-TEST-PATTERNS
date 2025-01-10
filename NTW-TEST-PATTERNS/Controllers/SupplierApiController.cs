using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Services;

namespace NTW_TEST_PATTERNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierApiController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        public SupplierApiController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet(Name = "Get Suppliers")]
        public async Task<IActionResult> GetSupplier() {
            try
            {
               var suppliers = await _supplierService.GetSuppliersAsync();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }}
        }
   
    }


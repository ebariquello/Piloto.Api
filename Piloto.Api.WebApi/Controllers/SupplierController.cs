using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piloto.Api.Application.DTO.DTO;
using Piloto.Api.Application.Interfaces;

namespace Piloto.Api.WebApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize]
    public class SupplierController(IApplicationServiceSupplier applicationServiceSupplier) : ControllerBase
    {
        private readonly IApplicationServiceSupplier applicationServiceSupplier = applicationServiceSupplier;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await applicationServiceSupplier.GetAll());
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] SupplierDTO supplierDTO)
        {
            return Ok(await applicationServiceSupplier.Add(supplierDTO));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] SupplierDTO supplierDTO)
        {
            return Ok(await applicationServiceSupplier.Update(supplierDTO));
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveAsync([FromBody] SupplierDTO supplierDTO)
        {
            await applicationServiceSupplier.Remove(supplierDTO);
            return Ok();
        }
      
    }
}

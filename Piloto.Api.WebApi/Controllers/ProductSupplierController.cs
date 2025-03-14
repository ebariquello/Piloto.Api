using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piloto.Api.Application.DTO.DTO;
using Piloto.Api.Application.Interfaces;

namespace Piloto.Api.WebApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize]
    public class ProductSupplierController(IApplicationServiceProductSupplier applicationServiceProductSupplier) : ControllerBase
    {
        private readonly IApplicationServiceProductSupplier applicationServiceProductSupplier = applicationServiceProductSupplier;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await applicationServiceProductSupplier.GetAll());
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ProductSupplierDTO productDTO)
        {
            return Ok(await applicationServiceProductSupplier.Add(productDTO));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] ProductSupplierDTO productDTO)
        {
            return Ok(await applicationServiceProductSupplier.Update(productDTO));
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveAsync([FromBody] ProductSupplierDTO productDTO)
        {
            await applicationServiceProductSupplier.Remove(productDTO);
            return Ok();
        }
      
    }
}

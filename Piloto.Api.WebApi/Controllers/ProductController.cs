using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piloto.Api.Application.DTO.DTO;
using Piloto.Api.Application.Interfaces;

namespace Piloto.Api.WebApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize]
    public class ProductController(IApplicationServiceProduct applicationServiceProduct) : ControllerBase
    {
        private readonly IApplicationServiceProduct applicationServiceProduct = applicationServiceProduct;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await applicationServiceProduct.GetAll());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            return Ok(await applicationServiceProduct.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ProductDTO productDTO)
        {
            return Ok(await applicationServiceProduct.Add(productDTO));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] ProductDTO productDTO)
        {
            return Ok(await applicationServiceProduct.Update(productDTO));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            await applicationServiceProduct.Remove(id);
            return Ok();
        }
      
    }
}

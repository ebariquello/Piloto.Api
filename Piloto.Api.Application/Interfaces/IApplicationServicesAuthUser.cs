using Microsoft.AspNetCore.Identity;
using Piloto.Api.Application.DTO.DTO;
using System.Threading.Tasks;

namespace Piloto.Api.Application.Interfaces
{
    public interface IApplicationServiceAuthUser
    {
        Task<string> LoginUserAsync(LoginDto model);
        Task<IdentityResult> RegisterUserAsync(RegisterDTO model);
    }
}
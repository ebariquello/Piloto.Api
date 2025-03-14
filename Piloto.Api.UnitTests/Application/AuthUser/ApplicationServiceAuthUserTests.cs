using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Piloto.Api.Application.DTO.DTO;
using Piloto.Api.Application.Interfaces;
using Piloto.Api.Application.Services;
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.UnitTests.Application.AuthUser
{
    public class ApplicationServicesUserTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly IApplicationServiceAuthUser _applicationServicesUser;

        public ApplicationServicesUserTests()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(_userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);
            _configurationMock = new Mock<IConfiguration>();

            _applicationServicesUser = new ApplicationServiceAuthUser(_userManagerMock.Object, _signInManagerMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnSuccessResult()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                Password = "Password123!",
                Roles = new List<string> { "User" }
            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { Email = registerDto.Email });

            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _applicationServicesUser.RegisterUserAsync(registerDto);

            // Assert
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task LoginUserAsync_ShouldReturnToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "testuser@example.com",
                Password = "Password123!"
            };

            var user = new ApplicationUser { Email = loginDto.Email };

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false))
                .ReturnsAsync(SignInResult.Success);

            _configurationMock.Setup(x => x.GetSection("JwtSettings")["Secret"]).Returns("your-very-long-secret-key-that-is-at-least-32-characters-long");
            _configurationMock.Setup(x => x.GetSection("JwtSettings")["Issuer"]).Returns("your-issuer");
            _configurationMock.Setup(x => x.GetSection("JwtSettings")["Audience"]).Returns("your-audience");

            // Act
            var token = await _applicationServicesUser.LoginUserAsync(loginDto);

            // Assert
            Assert.NotNull(token);
        }
    }
}

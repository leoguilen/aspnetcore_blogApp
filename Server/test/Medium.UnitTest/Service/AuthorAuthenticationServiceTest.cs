using Bogus;
using FluentAssertions;
using Medium.Core.Domain;
using Medium.Core.Options;
using Medium.Core.Repositories;
using Medium.Core.UnitOfWork;
using Medium.Infrastructure.Data.Context;
using Medium.Infrastructure.Services;
using Medium.IntegrationTest;
using Medium.IntegrationTest.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Medium.UnitTest.Service
{
    public class AuthorAuthenticationServiceTest
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorAuthenticationService _authenticationService;
        private readonly DataContext _dbContext;
        private readonly Faker _faker;

        public AuthorAuthenticationServiceTest()
        {
            _faker = new Faker("pt_BR");

            var provider = ServicesConfiguration.Configure();
            _dbContext = provider
                .GetRequiredService<DataContext>()
                .SeedTestData();
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            
            _authenticationService = new AuthorAuthenticationService(_unitOfWork, 
                new JwtOptions 
                { 
                    Secret = "Teste", 
                    TokenLifetime = TimeSpan.FromMinutes(5) 
                });
        }

        #region Login Tests

        [Fact]
        public async Task ShouldBeReturnedAuthenticationResultWithFailedIfEmailNotExists()
        {
            var invalidEmail = _faker.Person.Email;

            var authenticationResult = await _authenticationService.LoginAsync(invalidEmail, _faker.Internet.Password());

            authenticationResult.Should().BeOfType<AuthenticationResult>();
            authenticationResult.Success.Should().BeFalse();
            authenticationResult.Errors.Should().SatisfyRespectively(
                err => err.Should().Be("User does not exist"));
        }

        [Fact]
        public async Task ShouldBeReturnedAuthenticationResultWithFailedIfEmailExistsButPasswordIsInvalid()
        {
            var validEmail = "joao@email.com";

            var authenticationResult = await _authenticationService.LoginAsync(validEmail, _faker.Internet.Password());

            authenticationResult.Should().BeOfType<AuthenticationResult>();
            authenticationResult.Success.Should().BeFalse();
            authenticationResult.Errors.Should().SatisfyRespectively(
                err => err.Should().Be("Email/password combination is invalid"));
        }

        [Fact]
        public async Task ShouldBeReturnedAuthenticationResultWithSuccess()
        {
            const string validEmail = "joao@email.com";
            const string validPassword = "joao123";

            var authenticationResult = await _authenticationService.LoginAsync(validEmail, validPassword);

            authenticationResult.Should().BeOfType<AuthenticationResult>();
            authenticationResult.Success.Should().BeTrue();
            authenticationResult.Errors.Should().BeNull();
        }

        [Fact]
        public async Task ShouldBeReturnedAuthenticationResultWithSuccessAndToken()
        {
            const string validEmail = "maria@email.com";
            const string validPassword = "maria123";

            var authenticationResult = await _authenticationService.LoginAsync(validEmail, validPassword);

            authenticationResult.Should().BeOfType<AuthenticationResult>();
            authenticationResult.Success.Should().BeTrue();
            authenticationResult.Errors.Should().BeNull();
            authenticationResult.Token.Should().NotBeNullOrEmpty();
        }

        #endregion
    }
}

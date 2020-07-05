using Bogus;
using FluentAssertions;
using Medium.Core.UnitOfWork;
using Medium.Infrastructure.Data.Context;
using Medium.Infrastructure.Helpers;
using Medium.IntegrationTest;
using Medium.IntegrationTest.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
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
            var provider = ServicesConfiguration.Configure();

            _dbContext = provider
                .GetRequiredService<DataContext>()
                .SeedTestData();
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            _authenticationService = new AuthorAuthenticationService(_unitOfWork);
            _faker = new Faker("pt_BR");
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

        #endregion

    }

    public class AuthorAuthenticationService : IAuthorAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorAuthenticationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var author = await _unitOfWork.Authors
                .FindByConditionAsync(a => a.Email == email)
                .Result.SingleOrDefaultAsync();

            if (author == null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }

            bool isValidPassword = SecurePasswordHasher
                .AreEqual(password, author.Password, author.Salt);

            if (!isValidPassword)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "Email/password combination is invalid" }
                };
            }

            return new AuthenticationResult
            {
                Success = true
            };
        }
    }

    public interface IAuthorAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(string email, string password);
    }

    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}

using Bogus;
using FluentAssertions;
using Medium.Core.Common.Builder;
using Medium.Core.Domain;
using Medium.Core.Options;
using Medium.Core.Services;
using Medium.Core.UnitOfWork;
using Medium.Infrastructure.Data.Context;
using Medium.Infrastructure.Helpers;
using Medium.Infrastructure.Services;
using Medium.IntegrationTest;
using Medium.IntegrationTest.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Medium.UnitTest.Service
{
    public class AuthorAuthenticationServiceTest
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorAuthenticationService _authenticationService;
        private readonly DataContext _dbContext;
        private readonly Faker _faker;
        private readonly ITestOutputHelper _output;

        public AuthorAuthenticationServiceTest(ITestOutputHelper output)
        {
            _faker = new Faker("pt_BR");
            _output = output;

            var provider = ServicesConfiguration.Configure();
            _dbContext = provider
                .GetRequiredService<DataContext>()
                .SeedTestData();
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();

            _authenticationService = new AuthorAuthenticationService(_unitOfWork,
                new JwtOptions
                {
                    Secret = "9ce891b219b6fb5b0088e3e05e05baf5",
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
                err => err.Should().Be("Author does not exist"));
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

            _output.WriteLine($"Success: {authenticationResult.Success} | Token: {authenticationResult.Token}");
        }

        #endregion

        #region Register Tests

        [Fact]
        public async Task ShouldBeReturnedAuthenticationResultWithFailedIfEmailAlreadyExists()
        {
            var alreadyEmail = "joao@email.com";
            var salt = SecurePasswordHasher.CreateSalt(8);
            var newAuthor = new AuthorBuilder()
                .WithFirstName(_faker.Person.FirstName)
                .WithLastName(_faker.Person.LastName)
                .WithUsername(_faker.Person.UserName)
                .WithEmail(alreadyEmail)
                .WithPassword(SecurePasswordHasher.GenerateHash("joao123", salt))
                .WithSalt(salt)
                .Build();

            var authenticationResult = await _authenticationService.RegisterAsync(newAuthor);

            authenticationResult.Should().BeOfType<AuthenticationResult>();
            authenticationResult.Success.Should().BeFalse();
            authenticationResult.Token.Should().BeNull();
            authenticationResult.Errors.Should().SatisfyRespectively(
                err => err.Should().Be("Author with this email already exists"));
        }

        [Fact]
        public async Task ShouldBeReturnedAuthenticationResultWithSuccessAndCreatedNewAuthor()
        {
            var salt = SecurePasswordHasher.CreateSalt(8);
            var newAuthor = new AuthorBuilder()
                .WithFirstName(_faker.Person.FirstName)
                .WithLastName(_faker.Person.LastName)
                .WithUsername(_faker.Person.UserName)
                .WithEmail(_faker.Person.Email)
                .WithPassword(SecurePasswordHasher.GenerateHash("joao123", salt))
                .WithSalt(salt)
                .Build();

            var authenticationResult = await _authenticationService.RegisterAsync(newAuthor);

            authenticationResult.Should().BeOfType<AuthenticationResult>();
            authenticationResult.Success.Should().BeTrue();
            authenticationResult.Errors.Should().BeNull();
            authenticationResult.Token.Should().NotBeNullOrEmpty();

            _output.WriteLine($"Success: {authenticationResult.Success} | Token: {authenticationResult.Token}");

            // Verificando se o autor foi inserido no banco de dados
            var insertedAuthor = await _unitOfWork.Authors
                .FindByConditionAsync(a => a.Id == newAuthor.Id)
                .Result.SingleOrDefaultAsync();

            insertedAuthor.Should().NotBeNull();
            insertedAuthor.FirstName.Should().Be(newAuthor.FirstName);
            insertedAuthor.Username.Should().Be(newAuthor.Username);
            insertedAuthor.Email.Should().Be(newAuthor.Email);
        }

        #endregion

        #region Reset Password Tests

        [Fact]
        public async Task ShouldBeReturnedAuthenticationResultWithFailedIfEmailNotRegistered()
        {
            var notExistingEmail = _faker.Person.Email;

            var authenticationResult = await _authenticationService.ResetPasswordAsync(notExistingEmail, "novasenha");

            authenticationResult.Should().BeOfType<AuthenticationResult>();
            authenticationResult.Success.Should().BeFalse();
            authenticationResult.Token.Should().BeNull();
            authenticationResult.Errors.Should().SatisfyRespectively(
                err => err.Should().Be("Email does not exist"));
        }

        [Fact]
        public async Task ShouldBeReturnedAuthenticationResultWithFailedIfAuthorWasDeleted()
        {
            var deletedUser = await _dbContext.Authors.FirstAsync();
            deletedUser.Deleted = true;

            await _unitOfWork.Authors.UpdateAuthorAsync(deletedUser);
            await _unitOfWork.Commit();

            var authenticationResult = await _authenticationService.ResetPasswordAsync(deletedUser.Email, "novasenha");

            authenticationResult.Should().BeOfType<AuthenticationResult>();
            authenticationResult.Success.Should().BeFalse();
            authenticationResult.Token.Should().BeNull();
            authenticationResult.Errors.Should().SatisfyRespectively(
                err => err.Should().Be("Email does not exist"));
        }

        [Fact]
        public async Task ShouldBeReturnedAuthenticationResultWithSuccessAndAuthorPasswordWasReseted()
        {
            var email = "maria@email.com";
            var newPassword = _faker.Internet.Password();

            var authenticationResult = await _authenticationService.ResetPasswordAsync(email, newPassword);

            authenticationResult.Should().BeOfType<AuthenticationResult>();
            authenticationResult.Success.Should().BeTrue();
            authenticationResult.Token.Should().BeNull();
            authenticationResult.Errors.Should().BeNullOrEmpty();

            var resetedAuthor = await _dbContext.Authors
                .SingleAsync(a => a.Email == email);

            bool isValidNewPassword = SecurePasswordHasher.AreEqual(newPassword,
                resetedAuthor.Password, resetedAuthor.Salt);

            isValidNewPassword.Should().BeTrue();
        }

        #endregion
    }
}

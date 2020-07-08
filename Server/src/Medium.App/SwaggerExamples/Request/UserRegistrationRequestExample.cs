using Medium.Core.Contracts.V1.Request;
using Swashbuckle.AspNetCore.Filters;

namespace Medium.App.SwaggerExamples.Request
{
    public class UserRegistrationRequestExample : IExamplesProvider<UserRegistrationRequest>
    {
        public UserRegistrationRequest GetExamples()
        {
            return new UserRegistrationRequest
            {
                FirstName = "first name",
                LastName = "last name",
                Username = "username",
                Email = "email@domain.com",
                Password = "pass123"
            };
        }
    }
}

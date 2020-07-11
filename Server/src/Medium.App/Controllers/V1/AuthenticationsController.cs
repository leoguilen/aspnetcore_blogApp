using AutoMapper;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Request;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Domain;
using Medium.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Medium.App.Controllers.V1
{
    /// <summary>
    /// Endpoint responsible for authentication services
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    [Produces("application/json")]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IAuthorAuthenticationService _authService;
        private readonly IMapper _mapper;

        public AuthenticationsController(IAuthorAuthenticationService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        /// <summary>
        /// Register a new author in the system
        /// </summary>
        /// <response code="200">Register a new author in the system</response>
        /// <response code="400">An error occurred when try register a new author in the system</response>
        [HttpPost(ApiRoutes.Authentication.Register)]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(AuthFailedResponse), 400)]
        public async Task<IActionResult> Register([FromBody] AuthorRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x =>
                        x.Errors.Select(e => e.ErrorMessage))
                });
            }

            var author = _mapper.Map<Author>(request);

            var authResponse = await _authService
                .RegisterAsync(author)
                .ConfigureAwait(false);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }

        /// <summary>
        /// Login a author in the system
        /// </summary>
        /// <response code="200">Author login validated in the system</response>
        /// <response code="400">An error occurred when try login a author in the system</response>
        [HttpPost(ApiRoutes.Authentication.Login)]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(AuthFailedResponse), 400)]
        public async Task<IActionResult> Login([FromBody] AuthorLoginRequest request)
        {
            var authResponse = await _authService
                .LoginAsync(request.Email, request.Password)
                .ConfigureAwait(false);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }

        /// <summary>
        /// Reset password a author in the system 
        /// </summary>
        /// <response code="200">Author password reseted succefully in the system</response>
        /// <response code="400">An error occurred when try reset password a author in the system</response>
        [HttpPost(ApiRoutes.Authentication.ResetPassword)]
        [ProducesResponseType(typeof(ResetPasswordSuccessResponse), 200)]
        [ProducesResponseType(typeof(AuthFailedResponse), 400)]
        public async Task<IActionResult> ResetPassword([FromBody] AuthorResetPasswordRequest request)
        {
            var resetResponse = await _authService
                .ResetPasswordAsync(request.Email, request.NewPassword)
                .ConfigureAwait(false);

            if (!resetResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = resetResponse.Errors
                });
            }

            return Ok(new ResetPasswordSuccessResponse 
            {
                Reseted = true,
                SuccessMessage = "Senha redefinida com sucesso"
            });
        }
    }
}

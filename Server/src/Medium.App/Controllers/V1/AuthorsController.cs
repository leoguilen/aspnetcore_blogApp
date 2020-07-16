using AutoMapper;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Request.Author;
using Medium.Core.Contracts.V1.Request.Queries;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Contracts.V1.Response.Author;
using Medium.Core.Domain;
using Medium.Core.Helpers;
using Medium.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medium.App.Controllers.V1
{
    /// <summary>
    /// Endpoint responsible for manage author registration
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorService authorService, IUriService uriService, IMapper mapper)
        {
            _authorService = authorService;
            _uriService = uriService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all authors in the system
        /// </summary>
        /// <response code="200">Returns all authors in the system</response>
        [HttpGet(ApiRoutes.Authors.GetAll)]
        [ProducesResponseType(typeof(List<AuthorResponse>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var authors = await _authorService
                .GetAuthorsAsync(pagination)
                .ConfigureAwait(false);
            var authorsResponse = _mapper.Map<List<AuthorResponse>>(authors);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<AuthorResponse>(authorsResponse));
            }

            var paginationResponse = PaginationHelpers
                .CreatePaginatedResponse(_uriService, pagination, authorsResponse);

            return Ok(paginationResponse);
        }

        /// <summary>
        /// Returns author in the system by your id
        /// </summary>
        /// <response code="200">Return author in the system by your id</response>
        /// <response code="404">Not found any author with this id</response>
        [HttpGet(ApiRoutes.Authors.Get)]
        [ProducesResponseType(typeof(AuthorResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get([FromRoute] Guid authorId)
        {
            var author = await _authorService
                .GetAuthorByIdAsync(authorId)
                .ConfigureAwait(false);

            if (author == null)
                return NotFound();

            var authorResponse = new Response<AuthorResponse>(
                _mapper.Map<AuthorResponse>(author));

            return Ok(authorResponse);
        }

        /// <summary>
        /// Update author in the system by your id
        /// </summary>
        /// <response code="200">Update author in the system by your id</response>
        /// <response code="404">Not found any author with this id</response>
        /// <response code="400">An error occurred when try update author</response>
        [HttpPut(ApiRoutes.Authors.Update)]
        [ProducesResponseType(typeof(AuthorResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update([FromRoute] Guid authorId, [FromBody] UpdateAuthorRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = ModelState.Values.SelectMany(x =>
                        x.Errors.Select(e => e.ErrorMessage))
                });
            }

            var author = await _authorService
                .GetAuthorByIdAsync(authorId)
                .ConfigureAwait(false);

            if (author == null)
                return NotFound();

            author.FirstName = request?.FirstName;
            author.LastName = request?.LastName;
            author.Username = request?.Username;
            author.Email = request?.Email;
            author.Bio = request?.Bio;
            author.Avatar = request?.Avatar;

            var updated = await _authorService
                .UpdateAuthorAsync(author)
                .ConfigureAwait(false);

            if (!updated)
                return NotFound();

            var authorResponse = new Response<AuthorResponse>(
                _mapper.Map<AuthorResponse>(author));

            return Ok(authorResponse);
        }

        /// <summary>
        /// Delete author in the system by your id
        /// </summary>
        /// <response code="204">Delete author in the system by your id</response>
        /// <response code="404">Not found any author with this id</response>
        [HttpDelete(ApiRoutes.Authors.Delete)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete([FromRoute] Guid authorId)
        {
            var deleted = await _authorService
                .DeleteAuthorAsync(authorId)
                .ConfigureAwait(false);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}

using AutoMapper;
using Medium.Core.Common.Builder;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Request;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medium.App.Controllers.V1
{
    [ApiController]
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

        [HttpGet(ApiRoutes.Authors.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _authorService
                .GetAuthorsAsync()
                .ConfigureAwait(false);
            var authorsResponse = _mapper.Map<List<AuthorResponse>>(authors);

            return Ok(authorsResponse);
        }

        [HttpGet(ApiRoutes.Authors.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid authorId)
        {
            var author = await _authorService
                .GetAuthorByIdAsync(authorId)
                .ConfigureAwait(false);

            if (author == null)
                return NotFound();

            var authorResponse = _mapper.Map<AuthorResponse>(author);

            return Ok(authorResponse);
        }

        [HttpPost(ApiRoutes.Authors.Create)]
        public async Task<IActionResult> Create([FromBody] CreateAuthorRequest request)
        {
            var newAuthorId = Guid.NewGuid();
            var author = new AuthorBuilder()
                .WithId(newAuthorId)
                .WithFirstName(request.FirstName)
                .WithLastName(request.LastName)
                .WithUsername(request.Username)
                .WithPassword(request.Password)
                .WithEmail(request.Email)
                .WithBio(request.Bio)
                .WithAvatar(request.Avatar)
                .Build();

            await _authorService.CreateAuthorAsync(author).ConfigureAwait(false);

            var locationUrl = _uriService.GetAuthorUri(author.Id.ToString());

            return Created(locationUrl, _mapper.Map<AuthorResponse>(author));
        }

        [HttpPut(ApiRoutes.Authors.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid authorId, [FromBody] UpdateAuthorRequest request)
        {
            return Ok("Updated");
        }

        [HttpDelete(ApiRoutes.Authors.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid authorId)
        {
            return Ok("Deleted");
        }
    }
}

using Medium.App.Contracts.V1;
using Medium.App.Contracts.V1.Request;
using Medium.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Medium.App.Controllers.V1
{
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet(ApiRoutes.Authors.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok("All Authors");
        }

        [HttpGet(ApiRoutes.Authors.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid authorId)
        {
            return Ok($"Author with id {authorId}");
        }

        [HttpPost(ApiRoutes.Authors.Create)]
        public async Task<IActionResult> Create([FromBody] CreateAuthorRequest request)
        {
            return Ok("Created");
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

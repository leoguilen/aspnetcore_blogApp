using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Request.Queries;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Contracts.V1.Response.Tag;
using Medium.Core.Domain;
using Medium.Core.Helpers;
using Medium.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medium.App.Controllers.V1
{
    /// <summary>
    /// Endpoint responsible for manage tags
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;

        public TagsController(ITagService tagService, IUriService uriService, IMapper mapper)
        {
            _tagService = tagService;
            _uriService = uriService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all tags in the system
        /// </summary>
        /// <response code="200">Returns all tags in the system</response>
        [HttpGet(ApiRoutes.Tags.GetAll)]
        [ProducesResponseType(typeof(List<TagResponse>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var tags = await _tagService
                .GetTagsAsync(pagination)
                .ConfigureAwait(false);
            var tagsResponse = _mapper.Map<List<TagResponse>>(tags);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<TagResponse>(tagsResponse));
            }

            var paginationResponse = PaginationHelpers
                .CreatePaginatedResponse(_uriService, pagination, tagsResponse);

            return Ok(paginationResponse);
        }
    }
}

using AutoMapper;
using Medium.App.Extensions;
using Medium.Core.Common.Builder;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Request.Queries;
using Medium.Core.Contracts.V1.Request.Tag;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Contracts.V1.Response.Tag;
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
#pragma warning disable CA1307 // Especificar StringComparison
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
        private readonly ICacheService _cacheService;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;

        public TagsController(ITagService tagService,
            IUriService uriService, IMapper mapper,
            ICacheService cacheService)
        {
            _tagService = tagService;
            _uriService = uriService;
            _cacheService = cacheService;
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
            // Pegando tags cacheados
            var tagsResponse = _cacheService
                .GetCachedResponse<List<TagResponse>>(
                    ApiRoutes.Tags.GetAll);

            if (tagsResponse == null)
            {
                var tags = await _tagService
                    .GetTagsAsync(pagination)
                    .ConfigureAwait(false);
                tagsResponse = _mapper.Map<List<TagResponse>>(tags);

                _cacheService.SetCacheResponse(
                    ApiRoutes.Tags.GetAll,
                    tagsResponse,
                    TimeSpan.FromMinutes(2));
            }

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<TagResponse>(tagsResponse));
            }

            var paginationResponse = PaginationHelpers
                .CreatePaginatedResponse(_uriService, pagination, tagsResponse);

            return Ok(paginationResponse);
        }

        /// <summary>
        /// Returns tag in the system by your id
        /// </summary>
        /// <response code="200">Return tag in the system by your id</response>
        /// <response code="404">Not found any tag with this id</response>
        [HttpGet(ApiRoutes.Tags.Get)]
        [ProducesResponseType(typeof(TagResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get([FromRoute] Guid tagId)
        {
            // Pegando tag cacheada
            var tagResponse = _cacheService
                .GetCachedResponse<Response<TagResponse>>(
                    ApiRoutes.Tags.Get.Replace("{tagId}",
                    tagId.ToString()));

            if (tagResponse == null)
            {
                var tag = await _tagService
                .GetTagByIdAsync(tagId)
                .ConfigureAwait(false);

                if (tag == null)
                    return NotFound();

                tagResponse = new Response<TagResponse>(
                    _mapper.Map<TagResponse>(tag));

                _cacheService.SetCacheResponse(ApiRoutes.Tags.Get
                    .Replace("{tagId}", tagId.ToString()),
                    tagResponse);
            }

            return Ok(tagResponse);
        }

        /// <summary>
        /// Create a new tag in the system
        /// </summary>
        /// <response code="201">Create a new tag in the system</response>
        /// <response code="400">An error occurred when try create a new tag in the system</response>
        [HttpPost(ApiRoutes.Tags.Create)]
        [ProducesResponseType(typeof(TagResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = ModelState.Values.SelectMany(x =>
                        x.Errors.Select(e => e.ErrorMessage))
                });
            }

            var newTag = new TagBuilder()
                .WithName(request?.Name)
                .WithAuthor(HttpContext.GetUserId())
                .Build();

            await _tagService
                .CreateTagAsync(newTag)
                .ConfigureAwait(false);

            var locationUrl = _uriService
                .GetTagUri(newTag.Id.ToString());

            return Created(locationUrl, new Response<TagResponse>(
                _mapper.Map<TagResponse>(newTag)));
        }

        /// <summary>
        /// Update tag in the system by your id
        /// </summary>
        /// <response code="200">Update tag in the system by your id</response>
        /// <response code="404">Not found any tag with this id</response>
        /// <response code="400">An error occurred when try update tag</response>
        [HttpPut(ApiRoutes.Tags.Update)]
        [ProducesResponseType(typeof(TagResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update([FromRoute] Guid tagId, [FromBody] UpdateTagRequest request)
        {
            var tag = await _tagService
                .GetTagByIdAsync(tagId)
                .ConfigureAwait(false);

            if (tag == null)
                return NotFound();

            tag.Name = request?.Name;

            var updated = await _tagService
                .UpdateTagAsync(tag)
                .ConfigureAwait(false);

            if (!updated)
                return NotFound();

            var tagResponse = new Response<TagResponse>(
                _mapper.Map<TagResponse>(tag));

            return Ok(tagResponse);
        }

        /// <summary>
        /// Delete tag in the system by your id
        /// </summary>
        /// <response code="204">Delete tag in the system by your id</response>
        /// <response code="404">Not found any tag with this id</response>
        [HttpDelete(ApiRoutes.Tags.Delete)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete([FromRoute] Guid tagId)
        {
            var deleted = await _tagService
                .DeleteTagAsync(tagId)
                .ConfigureAwait(false);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
#pragma warning restore CA1307 // Especificar StringComparison
}

using AutoMapper;
using Medium.Core.Common.Builder;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Request.Post;
using Medium.Core.Contracts.V1.Request.Queries;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Contracts.V1.Response.Post;
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
    /// Endpoint responsible for manage posts
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICacheService _cacheService;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;

        public PostsController(IPostService postService, 
            IUriService uriService, IMapper mapper, 
            ICacheService cacheService)
        {
            _postService = postService;
            _cacheService = cacheService;
            _uriService = uriService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all posts in the system
        /// </summary>
        /// <response code="200">Returns all posts in the system</response>
        [HttpGet(ApiRoutes.Posts.GetAll)]
        [ProducesResponseType(typeof(List<PostResponse>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            // Pegando posts cacheados
            var postsResponse = _cacheService
                .GetCachedResponse<List<PostResponse>>(
                    ApiRoutes.Posts.GetAll);
            
            if(postsResponse == null)
            {
                var posts = await _postService
                    .GetPostsAsync(pagination)
                    .ConfigureAwait(false);
                postsResponse = _mapper.Map<List<PostResponse>>(posts);

                _cacheService.SetCacheResponse(
                    ApiRoutes.Posts.GetAll, 
                    postsResponse, 
                    TimeSpan.FromMinutes(2));
            }

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<PostResponse>(postsResponse));
            }

            var paginationResponse = PaginationHelpers
                .CreatePaginatedResponse(_uriService, pagination, postsResponse);

            return Ok(paginationResponse);
        }

        /// <summary>
        /// Returns post in the system by your id
        /// </summary>
        /// <response code="200">Return post in the system by your id</response>
        /// <response code="404">Not found any post with this id</response>
        [HttpGet(ApiRoutes.Posts.Get)]
        [ProducesResponseType(typeof(PostResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            // Pegando post cacheado
            var postResponse = _cacheService
                .GetCachedResponse<Response<PostResponse>>(
                    ApiRoutes.Posts.Get.Replace("{postId}", 
                    postId.ToString()));

            if(postResponse == null)
            {
                var post = await _postService
                    .GetPostByIdAsync(postId)
                    .ConfigureAwait(false);

                if (post == null)
                    return NotFound();

                postResponse = new Response<PostResponse>(
                    _mapper.Map<PostResponse>(post));

                _cacheService.SetCacheResponse(ApiRoutes.Posts.Get
                    .Replace("{postId}", postId.ToString()), 
                    postResponse);
            }

            return Ok(postResponse);
        }

        /// <summary>
        /// Create a new post in the system
        /// </summary>
        /// <response code="201">Create a new post in the system</response>
        /// <response code="400">An error occurred when try create a new post in the system</response>
        [HttpPost(ApiRoutes.Posts.Create)]
        [ProducesResponseType(typeof(PostResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = ModelState.Values.SelectMany(x =>
                        x.Errors.Select(e => e.ErrorMessage))
                });
            }

            var newPost = new PostBuilder()
                .WithTitle(request?.Title)
                .WithContent(request?.Content)
                .WithAttachments(request?.Attachments)
                .Build();

            await _postService
                .CreatePostAsync(newPost)
                .ConfigureAwait(false);

            var locationUrl = _uriService
                .GetPostUri(newPost.Id.ToString());

            return Created(locationUrl, new Response<PostResponse>(
                _mapper.Map<PostResponse>(newPost)));
        }

        /// <summary>
        /// Update post in the system by your id
        /// </summary>
        /// <response code="200">Update post in the system by your id</response>
        /// <response code="404">Not found any post with this id</response>
        /// <response code="400">An error occurred when try update post</response>
        [HttpPut(ApiRoutes.Posts.Update)]
        [ProducesResponseType(typeof(PostResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var post = await _postService
                .GetPostByIdAsync(postId)
                .ConfigureAwait(false);

            if (post == null)
                return NotFound();

            post.Title = request?.Title;
            post.Content = request?.Content;
            post.Attachments = request?.Attachments;

            var updated = await _postService
                .UpdatePostAsync(post)
                .ConfigureAwait(false);

            if (!updated)
                return NotFound();

            var postResponse = new Response<PostResponse>(
                _mapper.Map<PostResponse>(post));

            return Ok(postResponse);
        }

        /// <summary>
        /// Delete post in the system by your id
        /// </summary>
        /// <response code="204">Delete post in the system by your id</response>
        /// <response code="404">Not found any post with this id</response>
        [HttpDelete(ApiRoutes.Posts.Delete)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var deleted = await _postService
                .DeletePostAsync(postId)
                .ConfigureAwait(false);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
#pragma warning restore CA1307 // Especificar StringComparison
}

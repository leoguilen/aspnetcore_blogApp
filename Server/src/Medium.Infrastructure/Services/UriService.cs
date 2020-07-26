using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Request.Queries;
using Medium.Core.Services;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;

namespace Medium.Infrastructure.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetAllUri(PaginationQuery pagination = null)
        {
            var uri = new Uri(_baseUri);

            if (pagination == null) return uri;

            var modifieldUri = QueryHelpers.AddQueryString(_baseUri, new Dictionary<string, string>(
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("pageNumber", pagination.PageNumber.ToString()),
                    new KeyValuePair<string, string>("pageSize", pagination.PageSize.ToString())
                }));

            return new Uri(modifieldUri);
        }

        public Uri GetAuthorUri(string authorId) => 
            new Uri(_baseUri + ApiRoutes.Authors.Get.Replace("{authorId}", authorId));

        public Uri GetPostUri(string postId) => 
            new Uri(_baseUri + ApiRoutes.Posts.Get.Replace("{postId}", postId));

        public Uri GetTagUri(string tagId) => 
            new Uri(_baseUri + ApiRoutes.Tags.Get.Replace("{tagId}", tagId));
    }
}

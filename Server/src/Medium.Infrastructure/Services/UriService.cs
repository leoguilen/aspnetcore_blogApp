using Medium.Core.Contracts.V1;
using Medium.Core.Services;
using System;

namespace Medium.Infrastructure.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetAuthorUri(string authorId)
        {
            return new Uri(_baseUri + ApiRoutes.Authors.Get.Replace("{authorId}", authorId));
        }
    }
}

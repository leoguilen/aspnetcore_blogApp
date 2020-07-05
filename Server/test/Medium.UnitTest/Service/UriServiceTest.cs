using FluentAssertions;
using Medium.Core.Contracts.V1;
using Medium.Core.Services;
using Medium.Infrastructure.Services;
using System;
using Xunit;

namespace Medium.UnitTest.Service
{
    public class UriServiceTest
    {
        private readonly IUriService _uriService;
        private readonly string _baseUri = "http://localhost:5555/";

        public UriServiceTest()
        {
            _uriService = new UriService(_baseUri);
        }

        [Fact]
        public void ShouldBeFormatedUriForPostAuthor()
        {
            var authorId = Guid.NewGuid().ToString();
            var expectedRoute = ApiRoutes.Authors.Get.Replace("{authorId}", authorId);

            var formatedUri = _uriService.GetAuthorUri(authorId);

            formatedUri.Should().Be($"{_baseUri}{expectedRoute}");
        }
    }
}

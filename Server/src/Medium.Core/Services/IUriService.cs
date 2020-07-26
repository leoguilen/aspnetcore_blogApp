using Medium.Core.Contracts.V1.Request.Queries;
using System;

namespace Medium.Core.Services
{
    public interface IUriService
    {
        Uri GetAuthorUri(string authorId);
        Uri GetPostUri(string postId);
        Uri GetTagUri(string tagId);
        Uri GetAllUri(PaginationQuery pagination = null);
    }
}

using System;

namespace Medium.Core.Services
{
    public interface IUriService
    {
        Uri GetAuthorUri(string authorId);
    }
}

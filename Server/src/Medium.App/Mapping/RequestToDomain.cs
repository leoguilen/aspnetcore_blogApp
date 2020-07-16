using AutoMapper;
using Medium.Core.Contracts.V1.Request.Authentication;
using Medium.Core.Contracts.V1.Request.Queries;
using Medium.Core.Domain;

namespace Medium.App.Mapping
{
    public class RequestToDomain : Profile
    {
        public RequestToDomain()
        {
            CreateMap<AuthorRegistrationRequest, Author>();
            CreateMap<PaginationQuery, PaginationFilter>();
        }
    }
}

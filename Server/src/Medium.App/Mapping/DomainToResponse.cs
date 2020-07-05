using AutoMapper;
using Medium.App.Contracts.V1.Response;
using Medium.Core.Domain;

namespace Medium.App.Mapping
{
    public class DomainToResponse : Profile
    {
        public DomainToResponse()
        {
            CreateMap<Author, AuthorResponse>();
        }
    }
}

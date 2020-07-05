using AutoMapper;
using Medium.Core.Contracts.V1.Response;
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

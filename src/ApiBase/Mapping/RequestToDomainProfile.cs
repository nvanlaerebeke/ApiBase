using AutoMapper;
using ApiBase.Filter.Pagination;
using ApiBase.Filter.Sorting;

namespace ApiBase.Mapping
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            _ = CreateMap<PaginationQueryString, PaginationFilter>();
            _ = CreateMap<SortQueryString, SortFilter>();
        }
    }
}

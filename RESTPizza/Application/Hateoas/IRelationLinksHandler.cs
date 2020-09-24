using System;
using System.Collections.Generic;
using RESTPizza.Application.Dto;

namespace RESTPizza.Application.Hateoas
{
    public interface IRelationLinksHandler<TResponseModel, TApplicationState>
        where TResponseModel : APIResponseModel, new()
        where TApplicationState : Enum
    {
        List<Link> GenerateRelationLinks(string urlBase, TResponseModel responseModel, TApplicationState applicationState);
    }
}
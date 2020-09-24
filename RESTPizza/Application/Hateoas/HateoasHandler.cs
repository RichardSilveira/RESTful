using System;
using System.Collections.Generic;
using RESTPizza.Application.Dto;

namespace RESTPizza.Application.Hateoas
{
    public class HateoasHandler<TResponseModel, TApplicationState, TRelationLinksHandler>
        where TResponseModel : APIResponseModel, new()
        where TApplicationState : Enum
        where TRelationLinksHandler : IRelationLinksHandler<TResponseModel, TApplicationState>
    {
        private readonly string _urlBase;
        private readonly TRelationLinksHandler _relationLinksHandler;

        public HateoasHandler(string urlBase)
        {
            _urlBase = urlBase;
            _relationLinksHandler = Activator.CreateInstance<TRelationLinksHandler>();
        }

        public List<Link> GetLinksFor(TResponseModel responseModel, TApplicationState applicationState) =>
            _relationLinksHandler.GenerateRelationLinks(_urlBase, responseModel, applicationState);
    }
}
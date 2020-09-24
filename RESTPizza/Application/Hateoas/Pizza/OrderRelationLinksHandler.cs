using System;
using System.Collections.Generic;
using RESTPizza.Application.Dto;

namespace RESTPizza.Application.Hateoas.Pizza
{
    public class PizzaRelationLinksHandler : IRelationLinksHandler<PizzaResponse, PizzaApplicationState>
    {
        public PizzaRelationLinksHandler()
        {
        }

        public List<Link> GenerateRelationLinks(string urlBase, PizzaResponse responseModel, PizzaApplicationState applicationState) =>
            RelationLinksFunctionBy[applicationState](responseModel, new PizzaRelationLinkOptions(urlBase, responseModel));

        // Tell Don't Ask principle (there is a good chance to add more relation options later on)
        private static readonly IReadOnlyDictionary<PizzaApplicationState, Func<PizzaResponse, PizzaRelationLinkOptions, List<Link>>>
            RelationLinksFunctionBy =
                new Dictionary<PizzaApplicationState, Func<PizzaResponse, PizzaRelationLinkOptions, List<Link>>>()
                {
                    {
                        PizzaApplicationState.GettingPizza, (pizza, relations) => new List<Link>()
                        {
                            {
                                relations.SelfLink()
                            }
                        }
                    }
                };
    }
}
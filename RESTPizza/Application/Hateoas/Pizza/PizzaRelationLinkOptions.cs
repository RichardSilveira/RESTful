using RESTPizza.Application.Dto;

namespace RESTPizza.Application.Hateoas.Pizza
{
    public class PizzaRelationLinkOptions
    {
        private readonly string _urlBase;
        private readonly PizzaResponse _pizzaResponse;

        public PizzaRelationLinkOptions(string urlBase, PizzaResponse pizzaResponse)
        {
            _urlBase = urlBase;
            _pizzaResponse = pizzaResponse;
        }

        public Link SelfLink() => new Link()
        {
            Rel = "self",
            Method = "GET",
            Href = _urlBase + _pizzaResponse.PizzaID
        };
    }
}
using RESTPizza.Application.Dto;

namespace RESTPizza.Application.Hateoas.Order
{
    public class OrderRelationLinkOptions
    {
        private readonly string _urlBase;
        private readonly OrderResponse _orderResponse;

        public OrderRelationLinkOptions(string urlBase, OrderResponse orderResponse)
        {
            _urlBase = urlBase;
            _orderResponse = orderResponse;
        }

        public Link SelfLink() => new Link()
        {
            Rel = "self",
            Method = "GET",
            Href = _urlBase + _orderResponse.OrderID
        };

        public Link RejectLink() => new Link()
        {
            Rel = "reject",
            Method = "PUT",
            Href = _urlBase + _orderResponse.OrderID + "/reject"
        };

        public Link ApproveLink() => new Link()
        {
            Rel = "approve",
            Method = "PUT",
            Href = _urlBase + _orderResponse.OrderID + "/approve"
        };
    }
}
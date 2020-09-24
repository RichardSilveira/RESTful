using System;
using System.Collections.Generic;
using RESTPizza.Application.Dto;
using RESTPizza.Domain;

namespace RESTPizza.Application.Hateoas.Order
{
    public class OrderRelationLinksHandler : IRelationLinksHandler<OrderResponse, OrdersApplicationState>
    {
        public OrderRelationLinksHandler()
        {
        }

        public List<Link> GenerateRelationLinks(string urlBase, OrderResponse responseModel, OrdersApplicationState applicationState) =>
            RelationLinksFunctionBy[applicationState](responseModel, new OrderRelationLinkOptions(urlBase, responseModel));

        // Tell Don't Ask principle
        private static readonly IReadOnlyDictionary<OrdersApplicationState, Func<OrderResponse, OrderRelationLinkOptions, List<Link>>>
            RelationLinksFunctionBy =
                new Dictionary<OrdersApplicationState, Func<OrderResponse, OrderRelationLinkOptions, List<Link>>>()
                {
                    {
                        OrdersApplicationState.GettingOrder, (order, relations) =>
                        {
                            var links = new List<Link>();

                            links.Add(relations.SelfLink());
                            if (order.Status == (int) OrderStatus.WaitingAttendance)
                            {
                                links.Add(relations.ApproveLink());
                                links.Add(relations.RejectLink());
                            }

                            return links;
                        }
                    },
                    {
                        OrdersApplicationState.RegisteringOrder, (order, relations) => new List<Link>()
                        {
                            relations.SelfLink(),
                            relations.ApproveLink(),
                            relations.RejectLink()
                        }
                    },
                    {
                        OrdersApplicationState.ApprovingOrder, (order, relations) => new List<Link>()
                        {
                            relations.SelfLink()
                        }
                    },
                    {
                        OrdersApplicationState.RejectingOrder, (order, relations) => new List<Link>()
                        {
                            relations.SelfLink()
                        }
                    }
                };
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Omu.ValueInjecter;
using RESTPizza.Application.Dto;
using RESTPizza.Application.Hateoas;
using RESTPizza.Application.Hateoas.Order;
using RESTPizza.Domain;
using Swashbuckle.Swagger.Annotations;

namespace RESTPizza.Application
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly OrderService _orderService;

        private readonly string _urlBase;
        private readonly HateoasHandler<OrderResponse, OrdersApplicationState, OrderRelationLinksHandler> _hateoas;

        public OrderController(OrderService orderService, IConfiguration configuration)
        {
            _configuration = configuration;
            _orderService = orderService;

            _urlBase = $"{_configuration["BaseUrl"]}/orders/";
            _hateoas = new HateoasHandler<OrderResponse, OrdersApplicationState, OrderRelationLinksHandler>(_urlBase);
        }

        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(OrderResponse))]
        [HttpGet("/api/orders/{id}")]
        public ActionResult Get(Guid id)
        {
            var orderRequest = new OrderRequest();
            var order = _orderService.Get().SingleOrDefault(p => p.OrderID == id);

            if (order == null)
                return NotFound(null);

            var orderResponse = new OrderResponse();
            orderResponse.InjectFrom(order);
            orderResponse.Status = (int) order.Status;
            orderResponse.StatusDescription = order.Status.GetEnumDescription();

            orderResponse.Links = _hateoas.GetLinksFor(orderResponse, OrdersApplicationState.GettingOrder);

            return Ok(orderResponse);
        }

        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(IEnumerable<OrderRequest>))]
        [HttpGet("/api/orders")]
        public ActionResult Get()
        {
            var orderResponses = _orderService.Get()
                .Where(p => p.Status == OrderStatus.WaitingAttendance)
                .ToList()
                .Select(order =>
                {
                    var orderResponse = new OrderResponse();
                    orderResponse.InjectFrom(order);
                    orderResponse.Status = (int) order.Status;
                    orderResponse.StatusDescription = order.Status.GetEnumDescription();

                    orderResponse.Links = _hateoas.GetLinksFor(orderResponse, OrdersApplicationState.GettingOrder);
                    return orderResponse;
                });

            if (!orderResponses.Any())
                return NotFound(null);

            return Ok(orderResponses);
        }

        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Created, type: typeof(OrderResponse))]
        [HttpPost("/api/orders")]
        public ActionResult RegisterNew(OrderRequest orderRequest)
        {
            var order = new Order();
            order.InjectFrom(orderRequest);

            _orderService.RegisterNew(order, out List<string> errors);

            if (errors.Any())
                return BadRequest(errors.Aggregate((a, b) => { return a + ", " + b; }));

            var orderResponse = new OrderResponse();
            orderResponse.InjectFrom(order);
            orderResponse.Status = (int) order.Status;
            orderResponse.StatusDescription = order.Status.GetEnumDescription();

            orderResponse.Links = _hateoas.GetLinksFor(orderResponse, OrdersApplicationState.RegisteringOrder);

            return Created(new Uri(_urlBase + orderResponse.OrderID), orderResponse);
        }

        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(OrderResponse))]
        [HttpPut("/api/orders/{id}/approve")]
        public ActionResult Approve(Guid id)
        {
            //todo: handle the NullReferenceException
            var order = _orderService.Approve(id);
            var orderResponse = new OrderResponse();
            orderResponse.InjectFrom(order);
            orderResponse.Status = (int) order.Status;
            orderResponse.StatusDescription = order.Status.GetEnumDescription();

            orderResponse.Links = _hateoas.GetLinksFor(orderResponse, OrdersApplicationState.ApprovingOrder);

            return Ok(orderResponse);
        }

        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [HttpPut("/api/orders/{id}/reject")]
        public ActionResult Reject(Guid id)
        {
            _orderService.Reject(id);

            return NoContent();
        }
    }
}
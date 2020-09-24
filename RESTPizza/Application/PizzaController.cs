using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Omu.ValueInjecter;
using RESTPizza.Application.Dto;
using RESTPizza.Application.Hateoas;
using RESTPizza.Application.Hateoas.Pizza;
using RESTPizza.Domain;
using Swashbuckle.Swagger.Annotations;

namespace RESTPizza.Application
{
    [ApiController]
    [Route("[controller]")]
    public class PizzaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly PizzaService _pizzaService;

        private readonly string _urlBase;
        private readonly HateoasHandler<PizzaResponse, PizzaApplicationState, PizzaRelationLinksHandler> _hateoas;
        
        public PizzaController(IConfiguration configuration, PizzaService pizzaService)
        {
            _configuration = configuration;
            _pizzaService = pizzaService;
            
            _urlBase = $"{_configuration["BaseUrl"]}/pizzas/";
            _hateoas = new HateoasHandler<PizzaResponse, PizzaApplicationState, PizzaRelationLinksHandler>(_urlBase);
        }

        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(PizzaResponse))]
        [HttpGet("/api/pizzas/{id}")]
        public ActionResult Get(Guid id)
        {
            var pizza = _pizzaService.Get().SingleOrDefault(p => p.PizzaID == id);

            if (pizza == null)
                return NotFound(null);

            var pizzaResponse = new PizzaResponse().InjectFrom(pizza);

            return Ok(pizzaResponse);
        }

        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(IEnumerable<PizzaResponse>))]
        [HttpGet("/api/pizzas")]
        public ActionResult Get()
        {
            var pizzasResponse = _pizzaService.Get()
                .ToList()
                .Select(pizza =>
                {
                    var pizzaResponse = new PizzaResponse();
                    pizzaResponse.InjectFrom(pizza);
                    pizzaResponse.Links = _hateoas.GetLinksFor(pizzaResponse, PizzaApplicationState.GettingPizza);
                    return pizzaResponse;
                });

            if (!pizzasResponse.Any())
                return NotFound(null);

            return Ok(pizzasResponse);
        }

        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Created, type: typeof(PizzaResponse))]
        [HttpPost("/api/pizzas")]
        public ActionResult Create(PizzaRequest pizzaRequest)
        {
            var pizza = new Pizza();
            pizza.InjectFrom(pizzaRequest);

            _pizzaService.AddNew(pizza, out List<string> errors);

            if (errors.Any())
                return BadRequest(errors.Aggregate((a, b) => { return a + ", " + b; }));

            var pizzaResponse = new PizzaResponse();
            pizzaResponse.InjectFrom(pizza);
            pizzaResponse.Links = _hateoas.GetLinksFor(pizzaResponse, PizzaApplicationState.GettingPizza);

            return Created(new Uri($"{_urlBase}${pizzaResponse.PizzaID}"), pizzaResponse);
        }
    }
}
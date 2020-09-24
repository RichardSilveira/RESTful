using System;
using System.Collections.Generic;
using RESTPizza.Application.Hateoas;

namespace RESTPizza.Application.Dto
{
    public class PizzaResponse : APIResponseModel
    {
        public Guid PizzaID { get; set; }

        public string Name { get; set; }

        public string Ingredients { get; set; }
        public List<Link> Links { get; set; }
    }
}
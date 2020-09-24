using System;
using System.Collections.Generic;
using RESTPizza.Application.Hateoas;

namespace RESTPizza.Application.Dto
{
    public class OrderResponse : APIResponseModel
    {
        public Guid OrderID { get; set; }

        public string CustomerName { get; set; }

        public int Status { get; set; }
        public string StatusDescription { get; set; }

        public Guid PizzaID { get; set; }

        public List<Link> Links { get; set; }
    }
}
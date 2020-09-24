using System;

namespace RESTPizza.Application.Dto
{
    public class OrderRequest
    {
        public string CustomerName { get; set; }

        public Guid PizzaID { get; set; }
    }
}
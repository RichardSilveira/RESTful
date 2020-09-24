using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTPizza.Domain
{
    public class Order
    {
        [Key]
        public Guid OrderID { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public Guid PizzaID { get; set; }

        public virtual Pizza Pizza { get; set; }
    }
}
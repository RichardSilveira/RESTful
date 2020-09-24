using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTPizza.Domain
{
    public class Pizza
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PizzaID { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Ingredients { get; set; }
    }
}
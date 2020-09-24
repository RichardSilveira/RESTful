using Microsoft.EntityFrameworkCore;
using RESTPizza.Domain;

namespace RESTPizza.Infrastructure
{
    public class PizzaContext : DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Order> Orders { get; set; }

        public PizzaContext(DbContextOptions options) : base(options)
        {
        }
    }
}
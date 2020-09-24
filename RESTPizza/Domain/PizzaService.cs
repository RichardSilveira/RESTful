using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RESTPizza.Infrastructure;

namespace RESTPizza.Domain
{
    public class PizzaService
    {
        private readonly PizzaContext _context;
        private readonly DbSet<Pizza> _dbSet;

        public PizzaService(PizzaContext context)
        {
            _context = context;
            _dbSet = _context.Set<Pizza>();
        }

        public IQueryable<Pizza> Get() => _dbSet;

        public void AddNew(Pizza pizza, out List<string> errors)
        {
            errors = new List<string>();

            if (pizza == null) throw new NullReferenceException();

            if (string.IsNullOrEmpty(pizza.Name))
                errors.Add("The name is required");

            if (string.IsNullOrEmpty(pizza.Ingredients))
                errors.Add("You need to inform the ingredients");

            if (errors.Any()) return;

            _dbSet.Add(pizza);
            _context.SaveChanges();
        }
    }
}

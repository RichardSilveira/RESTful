using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RESTPizza.Infrastructure;

namespace RESTPizza.Domain
{
    public class OrderService
    {
        private readonly PizzaContext _context;
        private readonly DbSet<Order> _dbSet;

        public OrderService(PizzaContext context)
        {
            _context = context;
            _dbSet = _context.Set<Order>();
        }

        public IQueryable<Order> Get() => _dbSet;

        public void RegisterNew(Order order, out List<string> errors)
        {
            errors = new List<string>();

            if (order == null) throw new NullReferenceException();

            if (string.IsNullOrEmpty(order.CustomerName))
                errors.Add("The customer name was not informed");

            if (errors.Any()) return;

            order.Status = OrderStatus.WaitingAttendance;

            _dbSet.Add(order);
            _context.SaveChanges();
        }

        public Order Approve(Guid orderID)
        {
            var order = this.Get().SingleOrDefault(p => p.OrderID == orderID);

            if (order == null) throw new NullReferenceException();

            order.Status = OrderStatus.Approved;
            _dbSet.Attach(order);
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();

            return order;
        }

        public void Reject(Guid orderID)
        {
            var order = this.Get().SingleOrDefault(p => p.OrderID == orderID);

            if (order == null) throw new NullReferenceException();

            order.Status = OrderStatus.Rejected;

            _dbSet.Attach(order);
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
using MyShop.Core.Contracts;
using MyShop.Core.InMemory;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MyShop.Services
{
    public class OrderService : IOrderService
    {
        IRepository<Order> orderContext;

        public OrderService(IRepository<Order> OrderContext)
        {
            this.orderContext = OrderContext;
        }

        public void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems)
        {
            foreach (var item in basketItems)
            {
                baseOrder.OrderItems.Add(new OrderItem()
                {
                    ProductID = item.ID,
                    Image = item.Image,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                });
            }

            orderContext.Insert(baseOrder);
            orderContext.Commit();
        }

        public List<Order> GetOrderList()
        {
            return orderContext.Collection().ToList();
        }

        public Order GetOrder(string ID)
        {
            return orderContext.Find(ID);
        }

        public void UpdateOrder(Order updated)
        {
            orderContext.Update(updated);
            orderContext.Commit();
        }
    }
}

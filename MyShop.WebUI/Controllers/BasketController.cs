using MyShop.Core.Contracts;
using MyShop.Core.InMemory;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IRepository<Customer> customers;
        IBasketService basketService;
        IOrderService orderService;

        public BasketController(IBasketService basketService, IOrderService OrderService, IRepository<Customer> customers)
        {
            this.basketService = basketService;
            this.orderService = OrderService;
            this.customers = customers;
        }

        public ActionResult Index()
        {
            var model = basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToBasket(string ID)
        {
            basketService.AddToBasket(this.HttpContext, ID);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveToBasket(string ID)
        {
            basketService.RemoveFromBasket(this.HttpContext, ID);
            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary()
        {
            var basketSummary = basketService.GetBasketSummary(this.HttpContext);
            return PartialView(basketSummary);
        }

        [Authorize]
        public ActionResult Checkout()
        {
            Customer customer = customers.Collection().FirstOrDefault(c => c.Email == User.Identity.Name);

            if (customer != null)
            {
                Order order = new Order()
                {
                    Email = customer.Email,
                    City = customer.City,
                    State = customer.State,
                    Street = customer.Street,
                    FirstName = customer.FirstName,
                    Surname = customer.LastName,
                    ZipCode = customer.ZipCode
                };

                return View(order);
            } else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost, Authorize]
        public ActionResult Checkout(Order order)
        {
            var basketItems = basketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order Created";
            order.Email = User.Identity.Name;

            // process payment

            order.OrderStatus = "Payment Processed";
            orderService.CreateOrder(order, basketItems);
            basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("ThankYou", new { orderID = order.ID });
        }

        public ActionResult ThankYou(string orderID)
        {
            ViewBag.orderID = orderID;
            return View();
        }
    }
}
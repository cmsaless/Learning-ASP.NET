using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {

        ProductCategoryRepository context;

        public ProductCategoryManagerController()
        {
            context = new ProductCategoryRepository();
        }

        public ActionResult Index()
        {
            List<ProductCategory> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductCategory product = new ProductCategory();
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            } else
            {
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(String ID)
        {
            ProductCategory product = context.Find(ID);
            if (product == null)
            {
                return HttpNotFound();
            } else
            {
                return View(product);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory product, string ID)
        {
            ProductCategory productToEdit = context.Find(ID);
            if (productToEdit == null)
            {
                return HttpNotFound();
            } else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                productToEdit.Category = product.Category;

                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string ID)
        {
            ProductCategory productToDelete = context.Find(ID);

            if (productToDelete == null)
            {
                return HttpNotFound();
            } else
            {
                return View(productToDelete);
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(string ID)
        {
            ProductCategory productToDelete = context.Find(ID);

            if (productToDelete == null)
            {
                return HttpNotFound();
            } else
            {
                context.Delete(ID);
                context.Commit();

                return RedirectToAction("Index");
            }
        }
    }
}
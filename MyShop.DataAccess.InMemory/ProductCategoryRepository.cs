using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories = new List<ProductCategory>();

        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>;
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }

        public void Insert(ProductCategory prod)
        {
            productCategories.Add(prod);
        }

        public void Update(ProductCategory prod)
        {
            ProductCategory productToUpdate = productCategories.Find(p => p.ID == prod.ID);

            if (productToUpdate != null)
            {
                productToUpdate = prod;
            } else
            {
                throw new Exception("ProductCategory not found");
            }
        }

        public ProductCategory Find(string ID)
        {
            ProductCategory productToFind = productCategories.Find(p => p.ID == ID);

            if (productToFind != null)
            {
                return productToFind;
            } else
            {
                throw new Exception("ProductCategory not found");
            }
        }

        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string ID)
        {
            ProductCategory productToDelete = productCategories.Find(p => p.ID == ID);

            if (productToDelete != null)
            {
                productCategories.Remove(productToDelete);
            } else
            {
                throw new Exception("ProductCategory not found");
            }
        }
    }
}

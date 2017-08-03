using System.Collections.Generic;

namespace NKStrata.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Products = new List<Product>();
        }

        public string CustomerName { get; set; }
        public List<Product> Products { get; set; }

        public void AddProduct(Product product)
        {
            var currentProduct = Products.Find(p => p.ProductCode == product.ProductCode);

            if (currentProduct != null)
                currentProduct.Quantity += product.Quantity;
            else
                Products.Add(product);
        }

        public Product GetProductByCode(string productCode)
        {
            return Products.Find(p => p.ProductCode == productCode);
        }

        public void RemoveProduct(Product product)
        {
            var selectedProduct = GetProductByCode(product.ProductCode);

            if (selectedProduct != null)
                Products.Remove(product);
        }

        public void EditProduct(Product product)
        {
            var selectedProduct = GetProductByCode(product.ProductCode);

            if (selectedProduct != null)
                selectedProduct = product;
        }
    }
}
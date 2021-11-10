using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopManagement.ViewModel
{
    public class Product
    {
        public string image { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public Product()
        {
            image = "/Resource/Image/ao.jpg";
            name = "Sample";
            price = "350.000VND";
        }
    }
    public class ProductsViewModel:BaseViewModel 
    {
        public ObservableCollection<Product> listproduct { get; set; }
        public ProductsViewModel()
        {
            listproduct = new ObservableCollection<Product>();
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
            listproduct.Add(new Product());
        }    
    }
}

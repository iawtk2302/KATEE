using ClothesShopManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopManagement.ViewModel
{
    public class ProductsViewModel:BaseViewModel 
    {
        private ObservableCollection<SANPHAM> _listSP;
        public ObservableCollection<SANPHAM> listSP { get => _listSP; set { _listSP = value; OnPropertyChanged(); } }
        public ProductsViewModel()
        {
            listSP = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs);
        }
    }
}

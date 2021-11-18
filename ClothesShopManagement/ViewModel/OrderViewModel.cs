using ClothesShopManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopManagement.ViewModel
{
    public class OrderViewModel:BaseViewModel
    {
        private ObservableCollection<HOADON> _listHD;
        public ObservableCollection<HOADON> listHD { get => _listHD; set { _listHD = value; OnPropertyChanged(); } }
        public OrderViewModel()
        {
            listHD= new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs);
        }
    }
}

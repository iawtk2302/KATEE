using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClothesShopManagement.ViewModel
{
    public class OrderViewModel:BaseViewModel
    {
        private ObservableCollection<HOADON> _listHD;
        public ObservableCollection<HOADON> listHD { get => _listHD; set { _listHD = value; OnPropertyChanged(); } }
        public ICommand OpenAddOrder { get; set; }
        public OrderViewModel()
        {
            listHD= new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs);
            OpenAddOrder = new RelayCommand<OrderView>((p) => true, (p) => _OpenAdd(p));
        }
        void _OpenAdd(OrderView paramater)
        {
            AddOrder addOrder = new AddOrder();
            addOrder.ShowDialog();
        }
    }
}

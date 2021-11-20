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
        public ICommand SearchCommand { get; set; }
        public OrderViewModel()
        {
            listHD= new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs);
            OpenAddOrder = new RelayCommand<OrderView>((p) => true, (p) => _OpenAdd(p));
            SearchCommand = new RelayCommand<OrderView>((p) => true, (p) => _SearchCommand(p));
        }
        void _OpenAdd(OrderView paramater)
        {
            AddOrderView addOrder = new AddOrderView();
            addOrder.ShowDialog();
            listHD = new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs);
        }
        void _SearchCommand(OrderView paramater)
        {
            ObservableCollection<HOADON> temp = new ObservableCollection<HOADON>();
            if (paramater.txbSearch.Text != "")
            {
                foreach (HOADON s in listHD)
                {
                    if (s.KHACHHANG.HOTEN.Contains(paramater.txbSearch.Text))
                    {
                        temp.Add(s);
                    }
                }
                paramater.ListViewHD.ItemsSource = temp;
            }
            else
                paramater.ListViewHD.ItemsSource = listHD;
        }
    }
}

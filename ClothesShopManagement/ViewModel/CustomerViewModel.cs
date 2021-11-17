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
    public class CustomerViewModel:BaseViewModel
    {
        private ObservableCollection<KHACHHANG> _listKH;
        public ObservableCollection<KHACHHANG> listKH { get => _listKH; set { _listKH = value; OnPropertyChanged(); } }
        public ICommand SearchCommand { get; set; }
        public ICommand Detail { get; set; }
        public CustomerViewModel()
        {
            listKH = new ObservableCollection<KHACHHANG>(DataProvider.Ins.DB.KHACHHANGs);
            SearchCommand = new RelayCommand<CustomerView>((p) => true, (p) => _SearchCommand(p));
            Detail = new RelayCommand<CustomerView>((p) => { return p.ListViewKH.SelectedItem == null ? false : true; }, (p) => _DetailCs(p));
        }
        void _SearchCommand(CustomerView paramater)
        {
            ObservableCollection<KHACHHANG> temp = new ObservableCollection<KHACHHANG>();
            if (paramater.txbSearch.Text != "")
            {
                foreach (KHACHHANG s in listKH)
                {
                    if (s.SDT.Contains(paramater.txbSearch.Text))
                    {
                        temp.Add(s);
                    }
                }
                paramater.ListViewKH.ItemsSource = temp;
            }
            else
                paramater.ListViewKH.ItemsSource = listKH;
        }
        void _DetailCs(CustomerView paramater)
        {
            DetailCustomerView detailCustomerView = new DetailCustomerView();
            detailCustomerView.ShowDialog();
            KHACHHANG temp=(KHACHHANG) paramater.ListViewKH.SelectedItem;
            detailCustomerView.MaKH.Text = temp.MAKH;
            detailCustomerView.TenKH.Text = temp.HOTEN;
            detailCustomerView.SDT.Text = temp.SDT;
            detailCustomerView.GT.Text = temp.GIOITINH;
            
        }
    }
}

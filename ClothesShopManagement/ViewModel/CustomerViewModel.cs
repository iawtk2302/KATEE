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
        public ICommand AddCsCommand { get; set; }
        public ICommand LoadCsCommand { get; set; }
        private ObservableCollection<string> _listTK;
        public ObservableCollection<string> listTK { get => _listTK; set { _listTK = value; OnPropertyChanged(); } }
        public CustomerViewModel()
        {
            listKH = new ObservableCollection<KHACHHANG>(DataProvider.Ins.DB.KHACHHANGs);
            listTK = new ObservableCollection<string>() { "Họ tên", "Mã KH", "SĐT" };
            SearchCommand = new RelayCommand<CustomerView>((p) => true, (p) => _SearchCommand(p));
            Detail = new RelayCommand<CustomerView>((p) => { return p.ListViewKH.SelectedItem == null ? false : true; }, (p) => _DetailCs(p));
            AddCsCommand = new RelayCommand<CustomerView>((p) => true, (p) => _AddCs(p));
            LoadCsCommand = new RelayCommand<CustomerView>((p) => true, (p) => _LoadCsCommand(p));
        }
        void _LoadCsCommand(CustomerView parameter)
        {
            parameter.cbxChon.SelectedIndex = 0;
        }
        void _SearchCommand(CustomerView paramater)
        {
            ObservableCollection<KHACHHANG> temp = new ObservableCollection<KHACHHANG>(); 
            if (paramater.txbSearch.Text != "")
            {
                switch (paramater.cbxChon.SelectedItem.ToString())
                {
                    case "Mã KH":
                        {
                            foreach (KHACHHANG s in listKH)
                            {
                                if (s.MAKH.Contains(paramater.txbSearch.Text))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    case "Họ tên":
                        {
                            foreach (KHACHHANG s in listKH)
                            {
                                if (s.HOTEN.ToLower().Contains(paramater.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    case "SĐT":
                        {
                            foreach (KHACHHANG s in listKH)
                            {
                                if (s.SDT.Contains(paramater.txbSearch.Text))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    default:
                        {
                            foreach (KHACHHANG s in listKH)
                            {
                                if (s.HOTEN.Contains(paramater.txbSearch.Text))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
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
            KHACHHANG temp=(KHACHHANG) paramater.ListViewKH.SelectedItem;
            detailCustomerView.MaKH.Text = temp.MAKH;
            detailCustomerView.TenKH.Text = temp.HOTEN;
            detailCustomerView.SDT.Text = temp.SDT;
            detailCustomerView.GT.Text = temp.GIOITINH;
            detailCustomerView.DC.Text = temp.DCHI;
            int doanhso=0;
            foreach(HOADON a in DataProvider.Ins.DB.HOADONs)
            {
                if (a.MAKH == temp.MAKH)
                    doanhso += a.TRIGIA;
            }    
            detailCustomerView.DS.Text = doanhso.ToString();
            string hang = "Đồng";
            if (doanhso > 2000000 && doanhso <= 5000000)
                hang = "Bạc";
            else if (doanhso > 5000000 && doanhso <= 10000000)
                hang = "Vàng";
            else if(doanhso>10000000)
                hang = "Kim cương";
            detailCustomerView.Rank.Text = hang;
            detailCustomerView.ShowDialog();
            listKH = new ObservableCollection<KHACHHANG>(DataProvider.Ins.DB.KHACHHANGs);
            paramater.ListViewKH.ItemsSource = listKH;
            paramater.ListViewKH.SelectedItem = null;
        }
        void _AddCs(CustomerView paramater)
        {
            AddCustomerView addCustomerView = new AddCustomerView();
            addCustomerView.ShowDialog();
            listKH = new ObservableCollection<KHACHHANG>(DataProvider.Ins.DB.KHACHHANGs);
            paramater.ListViewKH.ItemsSource = listKH;
        }
    }
}

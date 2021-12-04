using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClothesShopManagement.ViewModel
{
    public class OrderViewModel:BaseViewModel
    {
        private ObservableCollection<HOADON> _listHD;
        public ObservableCollection<HOADON> listHD { get => _listHD; set { _listHD = value; OnPropertyChanged(); } }
        public ICommand OpenAddOrder { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand Detail { get; set; }
        public ICommand LoadCsCommand { get; set; }
        private ObservableCollection<string> _listTK;
        public ObservableCollection<string> listTK { get => _listTK; set { _listTK = value; OnPropertyChanged(); } }
        public OrderViewModel()
        {
            listTK = new ObservableCollection<string>() { "Họ tên", "Số HD", "Ngày" };
            listHD = new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs);
            OpenAddOrder = new RelayCommand<OrderView>((p) => true, (p) => _OpenAdd(p));
            SearchCommand = new RelayCommand<OrderView>((p) => true, (p) => _SearchCommand(p));
            Detail= new RelayCommand<OrderView>((p) => p.ListViewHD.SelectedItem!=null?true:false, (p) => _Detail(p));
            LoadCsCommand = new RelayCommand<OrderView>((p) => true, (p) => _LoadCsCommand(p));
        }
        void _LoadCsCommand(OrderView parameter)
        {
            parameter.cbxChon.SelectedIndex = 0;
        }
        bool check(int m)
        {
            foreach (HOADON temp in DataProvider.Ins.DB.HOADONs)
            {
                if (temp.SOHD == m)
                    return true;
            }
            return false;
        }
        int rdma()
        {
            int ma;
            do
            {
                Random rand = new Random();
                ma = rand.Next(0, 10000);
            } while (check(ma));
            return ma;
        }
        void _OpenAdd(OrderView paramater)
        {
            AddOrderView addOrder = new AddOrderView();
            addOrder.SoHD.Text=rdma().ToString();
            addOrder.ShowDialog();
            listHD = new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs);
            paramater.ListViewHD.ItemsSource = listHD;
            paramater.ListViewHD.Items.Refresh();
        }
        void _SearchCommand(OrderView paramater)
        {
            ObservableCollection<HOADON> temp = new ObservableCollection<HOADON>();
            if (paramater.txbSearch.Text != "")
            {
                switch (paramater.cbxChon.SelectedItem.ToString())
                {
                    case "Số HD":
                        {
                            try
                            {
                                foreach (HOADON s in listHD)
                                {
                                    if (s.SOHD == int.Parse(paramater.txbSearch.Text))
                                    {
                                        temp.Add(s);
                                    }
                                }
                                
                            }
                            catch  { }
                            break;
                        }
                    case "Họ tên":
                        {
                                foreach (HOADON s in listHD)
                                {
                                    if (s.KHACHHANG.HOTEN.ToLower().Contains(paramater.txbSearch.Text.ToLower()))
                                    {
                                        temp.Add(s);
                                    }
                                }
                                break;
                        }
                    case "Ngày":
                        {
                            foreach (HOADON s in listHD)
                            {
                                if (s.NGHD.ToString("dd/MM/yyyy").Contains(paramater.txbSearch.Text))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    default:
                        {
                            foreach (HOADON s in listHD)
                            {
                                if (s.KHACHHANG.HOTEN.ToLower().Contains(paramater.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                }
                paramater.ListViewHD.ItemsSource = temp;
            }
            else
                paramater.ListViewHD.ItemsSource = listHD;
        }
        void _Detail(OrderView parameter)
        {
            DetailOrder detailOrder = new DetailOrder();
            HOADON temp = (HOADON) parameter.ListViewHD.SelectedItem;
            detailOrder.MaND.Text = temp.NGUOIDUNG.MAND;
            detailOrder.TenND.Text= temp.NGUOIDUNG.TENND;
            detailOrder.Ngay.Text = temp.NGHD.ToString("dd/MM/yyyy hh:mm tt");
            detailOrder.SoHD.Text = temp.SOHD.ToString();
            detailOrder.MaKH.Text = temp.MAKH.ToString();
            detailOrder.TenKH.Text = temp.KHACHHANG.HOTEN;
            List<HienThi> list = new List<HienThi>();
            foreach(CTHD a in temp.CTHDs)
            {
                list.Add(new HienThi(a.MASP, a.SANPHAM.TENSP, a.SANPHAM.SIZE, a.SL,a.SANPHAM.GIA, a.SL * a.SANPHAM.GIA));    
            }    
            detailOrder.ListViewSP.ItemsSource = list;
            detailOrder.TT.Text= String.Format("{0:0,0}", temp.TRIGIA) + " VND";
            detailOrder.DG.Text=temp.DANHGIA.ToString();
            detailOrder.ShowDialog();
            parameter.ListViewHD.SelectedItem=null;
            listHD = new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs);
            parameter.ListViewHD.ItemsSource = listHD;
            _SearchCommand(parameter);
        }
    }
}

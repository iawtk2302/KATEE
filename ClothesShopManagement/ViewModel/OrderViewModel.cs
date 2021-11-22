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
        public ICommand Detail { get; set; }
        public OrderViewModel()
        {
            listHD= new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs);
            OpenAddOrder = new RelayCommand<OrderView>((p) => true, (p) => _OpenAdd(p));
            SearchCommand = new RelayCommand<OrderView>((p) => true, (p) => _SearchCommand(p));
            Detail= new RelayCommand<OrderView>((p) => p.ListViewHD.SelectedItem!=null?true:false, (p) => _Detail(p));
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
        void _Detail(OrderView parameter)
        {
            DetailOrder detailOrder = new DetailOrder();
            HOADON temp = (HOADON) parameter.ListViewHD.SelectedItem;
            detailOrder.MaND.Text = Const.ND.MAND;
            detailOrder.TenND.Text= Const.ND.TENND;
            detailOrder.Ngay.Text = temp.NGHD.ToString();
            detailOrder.SoHD.Text = temp.SOHD.ToString();
            detailOrder.MaKH.Text = temp.MAKH.ToString();
            detailOrder.TenKH.Text = temp.KHACHHANG.HOTEN;
            List<HienThi> list = new List<HienThi>();
            foreach(CTHD a in temp.CTHDs)
            {
                list.Add(new HienThi(a.MASP, a.SANPHAM.TENSP, a.SANPHAM.SIZE, a.SL, a.SL * a.SANPHAM.GIA));    
            }    
            detailOrder.ListViewSP.ItemsSource = list;
            detailOrder.TT.Text= String.Format("{0:0,0}", temp.TRIGIA) + " VND";
            detailOrder.DG.Text=temp.DANHGIA.ToString();
            detailOrder.ShowDialog();
        }
    }
}

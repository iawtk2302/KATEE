using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ClothesShopManagement.ViewModel
{
    public class AddOrderViewModel:BaseViewModel
    {
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand Choose { get; set; }
        private List<KHACHHANG> _LKH;
        public List<KHACHHANG> LKH { get=>_LKH; set { _LKH = value;OnPropertyChanged(); } }
        private List<SANPHAM> _LSP;
        public List<SANPHAM> LSP { get => _LSP; set { _LSP = value; OnPropertyChanged(); } }
        private List<SANPHAM> _LSPSelected;
        public List<SANPHAM> LSPSelected { get=> _LSPSelected; set { _LSPSelected = value; OnPropertyChanged(); } }
        public ICommand AddSP { get; set; }
        public AddOrderViewModel()
        {
            _LSPSelected = new List<SANPHAM>();
            Closewd = new RelayCommand<AddOrderView>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<AddOrderView>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<AddOrderView>((p) => true, (p) => moveWindow(p));
            Loadwd = new RelayCommand<AddOrderView>((p) => true, (p) => _Loadwd(p));
            Choose= new RelayCommand<AddOrderView>((p) => true, (p) => _Choose(p));
            AddSP=new RelayCommand<AddOrderView>((p) => true, (p) => _AddSP(p));
        }
        void moveWindow(AddOrderView p)
        {
            p.DragMove();
        }
        void Close(AddOrderView p)
        {
            p.Close();
        }
        void Minimize(AddOrderView p)
        {
            p.WindowState = WindowState.Minimized;
        }
        void _Loadwd(AddOrderView paramater)
        {
            LKH= DataProvider.Ins.DB.KHACHHANGs.ToList();
            LSP = DataProvider.Ins.DB.SANPHAMs.ToList();
            paramater.KH.ItemsSource = LKH;
            paramater.SP.ItemsSource = LSP;
            paramater.MaND.Text = Const.ND.TENND;
            paramater.Ngay.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        void _Choose(AddOrderView paramater)
        {
            SANPHAM temp = (SANPHAM)paramater.SP.SelectedItem;
            paramater.DG.Text=temp.GIA.ToString();
        }
        void _AddSP(AddOrderView paramater)
        {
            LSPSelected.Add((SANPHAM)paramater.SP.SelectedItem);
            paramater.ListViewSP.ItemsSource= LSPSelected;
            paramater.ListViewSP.Items.Refresh();
        }
    }
}

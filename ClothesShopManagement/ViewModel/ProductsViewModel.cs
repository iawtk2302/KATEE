using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ClothesShopManagement.ViewModel
{
    public class ProductsViewModel : BaseViewModel
    {
        public ObservableCollection<SANPHAM> a;
        private string _localLink = System.Reflection.Assembly.GetExecutingAssembly().Location.Remove(System.Reflection.Assembly.GetExecutingAssembly().Location.IndexOf(@"bin\Debug"));
        private ObservableCollection<SANPHAM> _listSP;
        public ObservableCollection<SANPHAM> listSP { get => _listSP; set { _listSP = value; OnPropertyChanged(); } }
        public ICommand SearchCommand { get; set; }
        public ICommand DetailPdCommand { get; set; }
        public ICommand AddPdPdCommand { get; set; }
        private string _LSP="Tất cả";
        public string LSP
        {
            get => _LSP; set
            {
                _LSP = value;
                if (!string.IsNullOrEmpty(_LSP))
                    OnPropertyChanged();
            }
        }
        public ProductsViewModel()
        {
            foreach (SANPHAM temp1 in DataProvider.Ins.DB.SANPHAMs)
            {
                if (!temp1.HINHSP.Contains(_localLink))
                     temp1.HINHSP = _localLink + temp1.HINHSP;
            }
            DataProvider.Ins.DB.SaveChanges();
            listSP = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.GroupBy(p => p.TENSP).Select(grp => grp.FirstOrDefault()));
            AddPdPdCommand = new RelayCommand<ProductsView>((p) => { return p == null ? false : true; }, (p) => _AddPdCommand(p));
            SearchCommand = new RelayCommand<ProductsView>((p) => { return p == null ? false : true; }, (p) => _SearchCommand(p));
            DetailPdCommand = new RelayCommand<ProductsView>((p) => { return p.ListViewProduct.SelectedItem == null ? false : true; }, (p) => _DetailPd(p));
        }
        void _SearchCommand(ProductsView paramater)
        {
            ObservableCollection<SANPHAM> temp = new ObservableCollection<SANPHAM>();
            if (paramater.txbSearch.Text != "")
            {
                foreach (SANPHAM s in listSP)
                {
                    if (s.TENSP.ToLower().Contains(paramater.txbSearch.Text.ToLower()))
                    {
                        temp.Add(s);
                    }
                }
                paramater.ListViewProduct.ItemsSource = temp;
            }
            else
                paramater.ListViewProduct.ItemsSource = listSP;
        }
        void _DetailPd(ProductsView paramater)
        {
            DetailProduct detailProduct = new DetailProduct();
            SANPHAM temp = (SANPHAM)paramater.ListViewProduct.SelectedItem;
            detailProduct.MaSP.Text = "Mã SP: " + temp.MASP;
            detailProduct.TenSP.Text = temp.TENSP;
            detailProduct.GiaSP.Text = temp.GIA.ToString();
            detailProduct.LoaiSP.Text = temp.LOAISP;
            string SL = DataProvider.Ins.DB.SANPHAMs.Where(p => p.TENSP == temp.TENSP).Sum(p => p.SL).ToString();
            detailProduct.SLSP.Text = "Số lượng: " + SL;
            detailProduct.kichco.ItemsSource = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.Where(p => p.TENSP == temp.TENSP));
            detailProduct.Mota.Text = temp.MOTA;
            Uri fileUri = new Uri(temp.HINHSP);
            detailProduct.HinhAnh.Source = new BitmapImage(fileUri);
            detailProduct.ShowDialog();
            paramater.ListViewProduct.SelectedItem = null;
        }
        void _AddPdCommand(ProductsView paramater)
        {
            AddProductView addProductView = new AddProductView();
            addProductView.ShowDialog();
            foreach (SANPHAM temp1 in DataProvider.Ins.DB.SANPHAMs)
            {
                if (!temp1.HINHSP.Contains(_localLink))
                    temp1.HINHSP = _localLink + temp1.HINHSP;
            }
            DataProvider.Ins.DB.SaveChanges();
            listSP = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.GroupBy(p => p.TENSP).Select(grp => grp.FirstOrDefault()));
            paramater.ListViewProduct.ItemsSource = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.GroupBy(p => p.TENSP).Select(grp => grp.FirstOrDefault()));
        }
    }
}

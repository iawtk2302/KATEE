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
        private string _localLink = System.Reflection.Assembly.GetExecutingAssembly().Location.Remove(System.Reflection.Assembly.GetExecutingAssembly().Location.IndexOf(@"bin\Debug"));
        private ObservableCollection<SANPHAM> _listSP;
        public ObservableCollection<SANPHAM> listSP { get => _listSP; set { _listSP = value; OnPropertyChanged(); } }
        private ObservableCollection<SANPHAM> _listSP1;
        public ObservableCollection<SANPHAM> listSP1 { get => _listSP1; set { _listSP1 = value; OnPropertyChanged(); } }
        public ICommand SearchCommand { get; set; }
        public ICommand DetailPdCommand { get; set; }
        public ICommand AddPdPdCommand { get; set; }
        public ICommand LoadCsCommand { get; set; }
        private ObservableCollection<string> _listTK;
        public ObservableCollection<string> listTK { get => _listTK; set { _listTK = value; OnPropertyChanged(); } }
        public ProductsViewModel()
        {
            listTK = new ObservableCollection<string>() { "Tên SP", "Loại SP" };
            listSP1 = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.Where(p=>p.SL>=0));  
            listSP = new ObservableCollection<SANPHAM>(listSP1.GroupBy(p => p.TENSP).Select(grp => grp.FirstOrDefault()));
            AddPdPdCommand = new RelayCommand<ProductsView>((p) => { return p == null ? false : true; }, (p) => _AddPdCommand(p));
            SearchCommand = new RelayCommand<ProductsView>((p) => { return p == null ? false : true; }, (p) => _SearchCommand(p));
            DetailPdCommand = new RelayCommand<ProductsView>((p) => { return p.ListViewProduct.SelectedItem == null ? false : true; }, (p) => _DetailPd(p));
            LoadCsCommand = new RelayCommand<ProductsView>((p) => true, (p) => _LoadCsCommand(p));
            
        }
        void _LoadCsCommand(ProductsView parameter)
        {
            parameter.cbxChon.SelectedIndex = 0;
        }
        void _SearchCommand(ProductsView paramater)
        {
            ObservableCollection<SANPHAM> temp = new ObservableCollection<SANPHAM>();
            if (paramater.txbSearch.Text != "")
            {
                switch (paramater.cbxChon.SelectedItem.ToString())
                {
                    case "Tên SP":
                        {
                            foreach (SANPHAM s in listSP)
                            {
                                if (s.TENSP.ToLower().Contains(paramater.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    case "Loại SP":
                        {
                            foreach (SANPHAM s in listSP)
                            {
                                if (s.LOAISP.ToLower().Contains(paramater.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    default:
                        {
                            foreach (SANPHAM s in listSP)
                            {
                                if (s.TENSP.ToLower().Contains(paramater.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
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
            string SL = listSP1.Where(p => p.TENSP == temp.TENSP&&p.SL>=0).Select(p=>p.SL).Sum().ToString();
            detailProduct.SLSP.Text = "Số lượng: " + SL;
            detailProduct.kichco.ItemsSource = new ObservableCollection<SANPHAM>(listSP1.Where(p => p.TENSP == temp.TENSP&&p.SL>=0));
            detailProduct.Mota.Text = temp.MOTA;
            Uri fileUri = new Uri(temp.HINHSP);
            detailProduct.HinhAnh.Source = new BitmapImage(fileUri);
            detailProduct.ShowDialog();
            listSP1 = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.Where(p => p.SL >= 0));
            listSP = new ObservableCollection<SANPHAM>(listSP1.GroupBy(p => p.TENSP).Select(grp => grp.FirstOrDefault()));
            paramater.ListViewProduct.ItemsSource = listSP;
            paramater.ListViewProduct.SelectedItem = null;
        }
        bool check(string m)
        {
            foreach (SANPHAM temp in DataProvider.Ins.DB.SANPHAMs)
            {
                if (temp.MASP == m)
                    return true;
            }
            return false;
        }
        string rdma()
        {
            string ma;
            do
            {
                Random rand = new Random();
                ma = "PD" + rand.Next(0, 10000).ToString();
            } while (check(ma));
            return ma;
        }
        void _AddPdCommand(ProductsView paramater)
        {
            AddProductView addProductView = new AddProductView();
            addProductView.MaSp.Text=rdma();
            addProductView.ShowDialog();
            listSP1 = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.Where(p=>p.SL>=0));
            listSP = new ObservableCollection<SANPHAM>(listSP1.GroupBy(p => p.TENSP).Select(grp => grp.FirstOrDefault()));
            paramater.ListViewProduct.ItemsSource = listSP;
        }
    }
}

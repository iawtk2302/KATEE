using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ClothesShopManagement.ViewModel
{
    public class ProductsViewModel:BaseViewModel 
    {
        private ObservableCollection<SANPHAM> _listSP;
        public ObservableCollection<SANPHAM> listSP { get => _listSP; set { _listSP = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _listLSP;
        public ObservableCollection<string> listLSP { get => _listLSP; set { _listLSP = value; OnPropertyChanged(); } }
        public ICommand ChoosePDCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand DetailPdCommand { get; set; }

        public ProductsViewModel()
        {
            listSP = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.GroupBy(p => p.TENSP).Select(grp => grp.FirstOrDefault()));   
            loadLSP();
        }
        public void loadLSP()
        {
            ObservableCollection<string> temp= new ObservableCollection<string>(DataProvider.Ins.DB.SANPHAMs.Select(p => p.LOAISP).Distinct().ToList());
            temp.Add("Tất cả");
            listLSP = temp;
            ChoosePDCommand = new RelayCommand<ProductsView>((p) => { return p == null ? false : true; }, (p) => _ChoosePDCommand(p));
            SearchCommand= new RelayCommand<ProductsView>((p) => { return p == null ? false : true; }, (p) => _SearchCommand(p));
            DetailPdCommand= new RelayCommand<ProductsView>((p) => { return p.ListViewProduct.SelectedItem  == null ? false : true; }, (p) => _DetailPd(p));
        }
        void _ChoosePDCommand(ProductsView paramater)
        {
            listSP= new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.Where(p => p.LOAISP == paramater.cbxLSP.SelectedItem.ToString()));
            if (paramater.cbxLSP.SelectedItem.ToString() != "Tất cả")
                paramater.ListViewProduct.ItemsSource = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.GroupBy(p => p.TENSP).Select(grp => grp.FirstOrDefault()).Where(p => p.LOAISP == paramater.cbxLSP.SelectedItem.ToString()));
            else
                paramater.ListViewProduct.ItemsSource = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.GroupBy(p => p.TENSP).Select(grp => grp.FirstOrDefault()));
        }
        void _SearchCommand(ProductsView paramater)
        {
            ObservableCollection<SANPHAM> temp=new ObservableCollection<SANPHAM>();
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
            listSP = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.GroupBy(p => p.TENSP).Select(grp => grp.FirstOrDefault()));
        }
        void _DetailPd(ProductsView paramater)
        {
            DetailProduct detailProduct = new DetailProduct();
            SANPHAM temp= (SANPHAM)paramater.ListViewProduct.SelectedItem;
            detailProduct.MaSP.Text ="Mã SP: " + temp.MASP;
            detailProduct.TenSP.Text = temp.TENSP;
            detailProduct.GiaSP.Text = temp.GIA.ToString();
            Uri fileUri = new Uri(temp.HINHSP, UriKind.Relative);
            detailProduct.HinhAnh.Source = new BitmapImage(fileUri);
            detailProduct.ShowDialog();
        }
    }
}

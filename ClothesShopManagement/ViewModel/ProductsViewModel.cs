using ClothesShopManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ProductsViewModel()
        {
            listSP = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs);   
            loadLSP();
        }
        public void loadLSP()
        {
            listLSP = new ObservableCollection<string>(DataProvider.Ins.DB.SANPHAMs.Select(p => p.LOAISP).Distinct().ToList());
            ChoosePDCommand = new RelayCommand<ProductsView>((p) => { return p == null ? false : true; }, (p) => _ChoosePDCommand(p));
            SearchCommand= new RelayCommand<ProductsView>((p) => { return p == null ? false : true; }, (p) => _SearchCommand(p));   
        }
        void _ChoosePDCommand(ProductsView paramater)
        {
            listSP= new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.Where(p => p.LOAISP == paramater.cbxLSP.SelectedItem.ToString()));
            paramater.ListViewProduct.ItemsSource = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.Where(p => p.LOAISP == paramater.cbxLSP.SelectedItem.ToString()));
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
    }
}

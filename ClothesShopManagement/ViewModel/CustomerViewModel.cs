using ClothesShopManagement.Model;
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
        public CustomerViewModel()
        {
            listKH = new ObservableCollection<KHACHHANG>(DataProvider.Ins.DB.KHACHHANGs);
        }
    }
}

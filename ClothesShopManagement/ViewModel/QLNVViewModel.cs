using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ClothesShopManagement.ViewModel
{
    public class QLNVViewModel:BaseViewModel
    {
        private ObservableCollection<NGUOIDUNG> _listND;
        public ObservableCollection<NGUOIDUNG> listND { get => _listND; set { _listND = value; OnPropertyChanged(); } }
        public ICommand SearchCommand { get; set; }
        public ICommand Detail { get; set; }
        public ICommand UpdateNDCommand { get; set; }
        public ICommand AddNDCommand { get; set; }
        public QLNVViewModel()
        {
            
            listND = new ObservableCollection<NGUOIDUNG>(DataProvider.Ins.DB.NGUOIDUNGs);
            SearchCommand = new RelayCommand<QLNVView>((p) => true, (p) => _SearchCommand(p));
            Detail = new RelayCommand<QLNVView>((p) => { return p.ListViewND.SelectedItem == null ? false : true; }, (p) => _DetailND(p));
            AddNDCommand = new RelayCommand<QLNVView>((p) => true, (p) => _AddND(p));
            UpdateNDCommand = new RelayCommand<DetailNDView>((p) => true, (p) => _UpdateNDCommand(p));
            
        }
        void _SearchCommand(QLNVView paramater)
        {
            ObservableCollection<NGUOIDUNG> temp = new ObservableCollection<NGUOIDUNG>();
            if (paramater.txbSearch.Text != "")
            {
                foreach (NGUOIDUNG s in listND)
                {
                    if (s.TENND.ToLower().Contains(paramater.txbSearch.Text.ToLower()))
                    {
                        temp.Add(s);
                    }
                }
                paramater.ListViewND.ItemsSource = temp;
            }
            else
                paramater.ListViewND.ItemsSource = listND;
        }
        void _DetailND(QLNVView paramater)
        {
            DetailNDView detailNDView=new DetailNDView();
            NGUOIDUNG temp= (NGUOIDUNG)paramater.ListViewND.SelectedItem;
            detailNDView.MaND.Text = temp.MAND;
            detailNDView.TenND.Text = temp.TENND;
            detailNDView.SDT.Text = temp.SDT;
            detailNDView.GT.Text = temp.GIOITINH;
            detailNDView.NS.Text = temp.NGSINH.ToString();
            detailNDView.TT.Text = temp.TTND;
            Uri fileUri = new Uri(temp.AVA);
            detailNDView.HinhAnh.Source= new BitmapImage(fileUri);
            detailNDView.DC.Text = temp.DIACHI;
            detailNDView.QTV.Text = temp.QTV == true ? "1" : "0";
            detailNDView.ShowDialog();
            paramater.ListViewND.SelectedItem = null;
        }
        void _UpdateNDCommand(DetailNDView p)
        {
            foreach(NGUOIDUNG a in DataProvider.Ins.DB.NGUOIDUNGs)
            {
                if (a.MAND == p.MaND.Text)
                {
                    if(p.TT.Text=="Đã nghỉ việc")
                    {
                        a.TTND = p.TT.Text;
                        a.USERNAME = "";
                        a.PASS = "";
                    }
                    else if(p.TT.Text == "Đang làm việc")
                    {
                        a.TTND = p.TT.Text;
                        a.USERNAME = a.MAND;
                        a.PASS = LoginViewModel.MD5Hash(LoginViewModel.Base64Encode(a.MAND));
                    }
                    if (p.QTV.Text == "1")
                        a.QTV = true;
                    else
                        a.QTV = false;
                }                      
            }
            DataProvider.Ins.DB.SaveChanges();
            MessageBox.Show("Cập nhật thành công !", "THÔNG BÁO");
        }
        void _AddND(QLNVView parameter)
        {
            AddNDView addNDView = new AddNDView();
            addNDView.ShowDialog();
            listND = new ObservableCollection<NGUOIDUNG>(DataProvider.Ins.DB.NGUOIDUNGs);
            parameter.ListViewND.ItemsSource = listND;
            parameter.ListViewND.Items.Refresh();
        }
    }
}

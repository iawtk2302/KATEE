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
        public ICommand DeleteNDCommand { get; set; }
        public ICommand AddNDCommand { get; set; }
        public ICommand LoadCsCommand { get; set; }
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        private ObservableCollection<string> _listTK;
        public ObservableCollection<string> listTK { get => _listTK; set { _listTK = value; OnPropertyChanged(); } }
        public QLNVViewModel()
        {
            
            listND = new ObservableCollection<NGUOIDUNG>(DataProvider.Ins.DB.NGUOIDUNGs.Where(p=>p.TTND==true));
            listTK = new ObservableCollection<string>() { "Họ tên", "Mã NV", "SĐT" };
            SearchCommand = new RelayCommand<QLNVView>((p) => true, (p) => _SearchCommand(p));
            Detail = new RelayCommand<QLNVView>((p) => { return p.ListViewND.SelectedItem == null ? false : true; }, (p) => _DetailND(p));
            AddNDCommand = new RelayCommand<QLNVView>((p) => true, (p) => _AddND(p));
            UpdateNDCommand = new RelayCommand<DetailNDView>((p) => true, (p) => _UpdateNDCommand(p));
            DeleteNDCommand = new RelayCommand<DetailNDView>((p) => true, (p) => _DeleteNDCommand(p));
            LoadCsCommand = new RelayCommand<QLNVView>((p) => true, (p) => _LoadCsCommand(p));
            Closewd = new RelayCommand<DetailNDView>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<DetailNDView>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<DetailNDView>((p) => true, (p) => moveWindow(p));

        }
        void moveWindow(DetailNDView p)
        {
            p.DragMove();
        }
        void Close(DetailNDView p)
        {
            p.Close();
        }
        void Minimize(DetailNDView p)
        {
            p.WindowState = WindowState.Minimized;
        }
        void _LoadCsCommand(QLNVView parameter)
        {
            parameter.cbxChon.SelectedIndex = 0;
        }
        void _SearchCommand(QLNVView paramater)
        {
            ObservableCollection<NGUOIDUNG> temp = new ObservableCollection<NGUOIDUNG>();
            if (paramater.txbSearch.Text != "")
            {
                switch (paramater.cbxChon.SelectedItem.ToString())
                {
                    case "Mã NV":
                        {
                            foreach (NGUOIDUNG s in listND)
                            {
                                if (s.MAND.Contains(paramater.txbSearch.Text))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    case "Họ tên":
                        {
                            foreach (NGUOIDUNG s in listND)
                            {
                                if (s.TENND.ToLower().Contains(paramater.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    case "SĐT":
                        {
                            foreach (NGUOIDUNG s in listND)
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
                            foreach (NGUOIDUNG s in listND)
                            {
                                if (s.TENND.Contains(paramater.txbSearch.Text))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
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
            Uri fileUri = new Uri(temp.AVA);
            detailNDView.HinhAnh.ImageSource= new BitmapImage(fileUri);
            detailNDView.DC.Text = temp.DIACHI;
            detailNDView.QTV.Text = temp.QTV == true ? "1" : "0";
            detailNDView.ShowDialog();
            listND = new ObservableCollection<NGUOIDUNG>(DataProvider.Ins.DB.NGUOIDUNGs.Where(p => p.TTND == true));
            paramater.ListViewND.ItemsSource = listND;
            paramater.ListViewND.SelectedItem = null;
        }
        void _UpdateNDCommand(DetailNDView p)
        {
            MessageBoxResult h = System.Windows.MessageBox.Show("  Bạn muốn cập nhật thông tin ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel);
            if (h == MessageBoxResult.Yes)
            {
                foreach (NGUOIDUNG a in DataProvider.Ins.DB.NGUOIDUNGs.Where(pa=>pa.TTND==true))
                {
                    if (a.MAND == p.MaND.Text)
                    {
                        if (p.QTV.Text == "1")
                            a.QTV = true;
                        else
                            a.QTV = false;
                    }
                }
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Cập nhật thành công !", "THÔNG BÁO");
            }          
        }
        void _DeleteNDCommand(DetailNDView p)
        {
            MessageBoxResult h = System.Windows.MessageBox.Show("  Bạn muốn xóa người dùng này ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel);
            if (h == MessageBoxResult.Yes)
            {
                foreach (NGUOIDUNG a in DataProvider.Ins.DB.NGUOIDUNGs.Where(pa => pa.TTND == true))
                {
                    if (a.MAND == p.MaND.Text)
                    {
                        a.TTND = false;
                    }
                }
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Xóa người dùng thành công !", "THÔNG BÁO");
            }
        }
        void _AddND(QLNVView parameter)
        {
            AddNDView addNDView = new AddNDView();
            addNDView.ShowDialog();
            listND = new ObservableCollection<NGUOIDUNG>(DataProvider.Ins.DB.NGUOIDUNGs.Where(p=>p.TTND==true));
            parameter.ListViewND.ItemsSource = listND;
            parameter.ListViewND.Items.Refresh();
        }
    }
}

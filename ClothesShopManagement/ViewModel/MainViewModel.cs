using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClothesShopManagement.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand CloseLogin { get; set; }
        public ICommand MinimizeLogin { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand GetIdTab { get; set; }
        public ICommand SwitchTab { get; set; }
        public ICommand TenDangNhap_Loaded { get; set; }
        public ICommand Quyen_Loaded { get; set; }
        public ICommand LogOutCommand { get; set; }
        private NGUOIDUNG _User;
        public NGUOIDUNG User { get => _User; set { _User = value; OnPropertyChanged(); } }
        private Visibility _SetQuanLy;
        public Visibility SetQuanLy { get => _SetQuanLy; set { _SetQuanLy = value; OnPropertyChanged(); } }
        public string Name;
        private string _Ava;
        public string Ava { get => _Ava; set { _Ava = value; OnPropertyChanged(); } }
        public ICommand Loadwd { get; set; }

        public MainViewModel()
        {
            Loadwd=new RelayCommand<MainWindow>((p) => true, (p) => _Loadwd(p));
            CloseLogin = new RelayCommand<MainWindow>((p) => true, (p) => Close());
            MinimizeLogin = new RelayCommand<MainWindow>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<MainWindow>((p) => true, (p) => moveWindow(p));
            GetIdTab = new RelayCommand<RadioButton>((p) => true, (p) => Name = p.Uid);
            SwitchTab = new RelayCommand<MainWindow>((p) => true, (p) => switchtab(p));
            TenDangNhap_Loaded = new RelayCommand<MainWindow>((p) => true, (p) => LoadTenND(p));
            Quyen_Loaded = new RelayCommand<MainWindow>((p) => true, (p) => LoadQuyen(p));
            LogOutCommand = new RelayCommand<MainWindow>((p) => { return true; }, (p) => LogOut(p));
        }
        void _Loadwd(MainWindow p)
        {
            if (LoginViewModel.IsLogin)
            {
                string a = Const.TenDangNhap;
                User = DataProvider.Ins.DB.NGUOIDUNGs.Where(x => x.USERNAME == a).FirstOrDefault();
                Const.ND = User;
                SetQuanLy = User.QTV ? Visibility.Visible : Visibility.Collapsed;
                Const.Admin = User.QTV;
                Ava = User.AVA;
            }
        }
        void LogOut(MainWindow p)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            p.Close();
        }
        public void LoadTenND(MainWindow p)
        {
            p.TenDangNhap.Text = string.Join(" ", User.TENND.Split().Reverse().Take(2).Reverse());
        }
        public void LoadQuyen(MainWindow p)
        {
            p.Quyen.Text = User.QTV ? "Quản trị viên" : "Nhân viên";
        }
        void switchtab(MainWindow p)
        {
            
            int index = int.Parse(Name);
            switch (index)
            {
                case 0:
                    {
                        p.Main.NavigationService.Navigate(new HomeView());
                        break;
                    }
                case 1:
                    {

                        p.Main.NavigationService.Navigate(new OrderView());
                        break;
                    }
                case 2:
                    {
                        p.Main.NavigationService.Navigate(new ProductsView());
                        break;
                    }
                case 3:
                    {
                        p.Main.NavigationService.Navigate(new CustomerView());
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        public void moveWindow(MainWindow p)
        {
            p.DragMove();
        }
        public void Close()
        {
            System.Windows.Application.Current.Shutdown();
        }
        public void Minimize(MainWindow p)
        {
            p.WindowState = WindowState.Minimized;
        }
    }
}

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
    public class LoginViewModel : BaseViewModel
    {
        public ICommand CloseLogin { get; set; }
        public ICommand MinimizeLogin { get; set; }
        public ICommand MoveLogin { get; set; }
        public ICommand Login { get; set; }
        public LoginViewModel()
        {
            CloseLogin = new RelayCommand<LoginWindow>((p) => true, (p) => Close());
            MinimizeLogin = new RelayCommand<LoginWindow>((p) => true, (p) => Minimize(p));
            MoveLogin = new RelayCommand<LoginWindow>((p) => true, (p) => Move(p));
            Login = new RelayCommand<LoginWindow>((p) => true, (p) => login(p));
        }
        public void Close()
        {
            System.Windows.Application.Current.Shutdown();
        }
        public void Minimize(LoginWindow p)
        {
            p.WindowState = WindowState.Minimized;
        }
        public void Move(LoginWindow p)
        {
            p.DragMove();
        }
        public void login(LoginWindow p)
        {
            MainWindow mainWindow = new MainWindow();
            p.Close();
            mainWindow.ShowDialog();
        }
    }
}

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
        public string Name;
        public MainViewModel()
        {
            CloseLogin = new RelayCommand<MainWindow>((p) => true, (p) => Close());
            MinimizeLogin = new RelayCommand<MainWindow>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<MainWindow>((p) => true, (p) => moveWindow(p));
            GetIdTab = new RelayCommand<RadioButton>((p) => true, (p) => Name = p.Uid);
            SwitchTab = new RelayCommand<MainWindow>((p) => true, (p) => switchtab(p));
        }
        public void switchtab(MainWindow p)
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

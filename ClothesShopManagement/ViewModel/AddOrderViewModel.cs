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
    public class AddOrderViewModel:BaseViewModel
    {
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public AddOrderViewModel()
        {
            Closewd = new RelayCommand<AddOrder>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<AddOrder>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<AddOrder>((p) => true, (p) => moveWindow(p));
        }
        void moveWindow(AddOrder p)
        {
            p.DragMove();
        }
        void Close(AddOrder p)
        {
            p.Close();
        }
        void Minimize(AddOrder p)
        {
            p.WindowState = WindowState.Minimized;
        }
    }
}

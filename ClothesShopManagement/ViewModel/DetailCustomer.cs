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
    public class DetailCustomer:BaseViewModel
    {
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public DetailCustomer()
        {
            Closewd = new RelayCommand<DetailCustomerView>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<DetailCustomerView>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<DetailCustomerView>((p) => true, (p) => moveWindow(p));
        }
        void moveWindow(DetailProduct p)
        {
            p.DragMove();
        }
        void Close(DetailProduct p)
        {
            p.Close();
        }
        void Minimize(DetailProduct p)
        {
            p.WindowState = WindowState.Minimized;
        }
    }
}

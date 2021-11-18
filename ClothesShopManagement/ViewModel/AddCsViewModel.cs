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
    public class AddCsView:BaseViewModel
    {
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public AddCsView()
        {
            Closewd = new RelayCommand<AddCustomerView>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<AddCustomerView>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<AddCustomerView>((p) => true, (p) => moveWindow(p));
        }
        void moveWindow(AddCustomerView p)
        {
            p.DragMove();
        }
        void Close(AddCustomerView p)
        {
            p.Close();
        }
        void Minimize(AddCustomerView p)
        {
            p.WindowState = WindowState.Minimized;
        }
    }
}

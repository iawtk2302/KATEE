using ClothesShopManagement.Model;
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
    public class DetailOrderViewModel
    {
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand Loadwd { get; set; }
        public DetailOrderViewModel()
        {
            Closewd = new RelayCommand<DetailOrder>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<DetailOrder>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<DetailOrder>((p) => true, (p) => moveWindow(p));

        }
        void moveWindow(DetailOrder p)
        {
            p.DragMove();
        }
        void Close(DetailOrder p)
        {
            p.Close();
        }
        void Minimize(DetailOrder p)
        {
            p.WindowState = WindowState.Minimized;
        }
    }
}

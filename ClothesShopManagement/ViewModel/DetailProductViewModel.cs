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
    public class DetailProductViewModel:BaseViewModel
    {
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public string HinhAnh { get; set; }
        public DetailProductViewModel()
        {
            Closewd = new RelayCommand<DetailProduct>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<DetailProduct>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<DetailProduct>((p) => true, (p) => moveWindow(p));
        }
        public void moveWindow(DetailProduct p)
        {
            p.DragMove();
        }
        public void Close(DetailProduct p)
        {
            p.Close();
        }
        public void Minimize(DetailProduct p)
        {
            p.WindowState = WindowState.Minimized;
        }
    }
}

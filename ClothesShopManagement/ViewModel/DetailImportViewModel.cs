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
    class DetailImportViewModel 
    {
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand Loadwd { get; set; }
        public DetailImportViewModel()
        {
            Closewd = new RelayCommand<DetailImport>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<DetailImport>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<DetailImport>((p) => true, (p) => moveWindow(p));

        }
        void moveWindow(DetailImport p)
        {
            p.DragMove();
        }
        void Close(DetailImport p)
        {
            p.Close();
        }
        void Minimize(DetailImport p)
        {
            p.WindowState = WindowState.Minimized;
        }
    }
}

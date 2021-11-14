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
        public ICommand UpdateProduct { get; set; }
        public ICommand GetName { get; set; }
        private string TenSP1;
        public DetailProductViewModel()
        {
            Closewd = new RelayCommand<DetailProduct>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<DetailProduct>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<DetailProduct>((p) => true, (p) => moveWindow(p));
            GetName = new RelayCommand<DetailProduct>((p) => true, (p) => _GetName(p));
            UpdateProduct =new RelayCommand<DetailProduct>((p) => true, (p) => _UpdateProduct(p));
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
        void _GetName(DetailProduct p)
        {
            TenSP1 = p.TenSP.Text;
        }
        void _UpdateProduct(DetailProduct p)
        {
            var temp = DataProvider.Ins.DB.SANPHAMs.Where(pa => pa.TENSP == TenSP1);
            foreach(SANPHAM a in temp)
            {
                a.TENSP = p.TenSP.Text;
                a.LOAISP = p.LoaiSP.Text;
                a.MOTA = p.Mota.Text;
            }              
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}

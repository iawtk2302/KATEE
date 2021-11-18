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
        private string MaPD1;
        public ICommand Loadwd { get; set; }
        public DetailProductViewModel()
        {
            Closewd = new RelayCommand<DetailProduct>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<DetailProduct>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<DetailProduct>((p) => true, (p) => moveWindow(p));
            GetName = new RelayCommand<DetailProduct>((p) => true, (p) => _GetName(p));
            UpdateProduct =new RelayCommand<DetailProduct>((p) => true, (p) => _UpdateProduct(p));
            Loadwd=new RelayCommand<DetailProduct>((p) => true, (p) => _Loadwd(p));
        }
        void _Loadwd(DetailProduct parmater)
        {
            if(Const.Admin)
            {
                parmater.TenSP.IsEnabled = true;
                parmater.Mota.IsEnabled = true;
            } 
            else
            {
                parmater.TenSP.IsEnabled = false;
                parmater.Mota.IsEnabled = false;
            }    
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
            MaPD1 = p.MaSP.Text;
        }
        void _UpdateProduct(DetailProduct p)
        {
            if (string.IsNullOrEmpty(p.TenSP.Text) || string.IsNullOrEmpty(p.Mota.Text) || string.IsNullOrEmpty(p.Mota.Text))
            {
                MessageBox.Show("Thông tin chưa đầy đủ !", "THÔNG BÁO");
            }
            else
            {
                var temp = DataProvider.Ins.DB.SANPHAMs.Where(pa => pa.MASP == MaPD1);
                foreach (SANPHAM a in temp)
                {
                    a.TENSP = p.TenSP.Text;
                    a.MOTA = p.Mota.Text;
                    a.MOTA = p.Mota.Text;
                }
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Cập nhật sản phẩm thành công !", "THÔNG BÁO");
            }              
        }
    }
}

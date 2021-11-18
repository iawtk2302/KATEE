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
    public class DetailCustomer:BaseViewModel
    {
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        private string MaKH;
        public ICommand GetMAKH { get; set; }
        public ICommand Update { get; set; }
        public DetailCustomer()
        {
            Closewd = new RelayCommand<DetailCustomerView>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<DetailCustomerView>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<DetailCustomerView>((p) => true, (p) => moveWindow(p));
            GetMAKH = new RelayCommand<DetailCustomerView>((p) => true, (p) => _GetMAKH(p));
            Update = new RelayCommand<DetailCustomerView>((p) => true, (p) => _Update(p));
        }
        void moveWindow(DetailCustomerView p)
        {
            p.DragMove();
        }
        void Close(DetailCustomerView p)
        {
            p.Close();
        }
        void Minimize(DetailCustomerView p)
        {
            p.WindowState = WindowState.Minimized;
        }
        void _GetMAKH(DetailCustomerView paramater)
        {
            MaKH = paramater.MaKH.Text;
        }
        void _Update(DetailCustomerView p)
        {
            if(string.IsNullOrEmpty(p.TenKH.Text)|| string.IsNullOrEmpty(p.SDT.Text) || string.IsNullOrEmpty(p.GT.Text) || string.IsNullOrEmpty(p.DC.Text))
            {
                MessageBox.Show("Thông tin chưa đầy đủ !", "THÔNG BÁO");
            }
            else
            {
                var temp = DataProvider.Ins.DB.KHACHHANGs.Where(pa => pa.MAKH == MaKH);
                foreach (KHACHHANG a in temp)
                {
                    a.HOTEN = p.TenKH.Text;
                    a.SDT = p.SDT.Text;
                    a.GIOITINH = p.GT.Text;
                    a.DCHI = p.DC.Text;
                }
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Cập nhật thông tin thành công !", "THÔNG BÁO");
            }           
        }
    }
}

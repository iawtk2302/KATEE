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
    public class AddCsView:BaseViewModel
    {
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand AddCs { get; set; }
        public AddCsView()
        {
            Closewd = new RelayCommand<AddCustomerView>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<AddCustomerView>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<AddCustomerView>((p) => true, (p) => moveWindow(p));
            AddCs = new RelayCommand<AddCustomerView>((p) => true, (p) => _AddCs(p));
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
        void _AddCs(AddCustomerView paramater)
        {
            if(string.IsNullOrEmpty(paramater.MaKH.Text)|| string.IsNullOrEmpty(paramater.TenKH.Text)|| string.IsNullOrEmpty(paramater.SDT.Text) || string.IsNullOrEmpty(paramater.GT.Text)|| string.IsNullOrEmpty(paramater.DC.Text))
            {
                MessageBox.Show("Thông tin chưa đầy đủ !","THÔNG BÁO");
            }  
            else
            {
                KHACHHANG temp = new KHACHHANG();
                temp.MAKH = paramater.MaKH.Text;
                temp.HOTEN = paramater.TenKH.Text;
                temp.SDT = paramater.SDT.Text;
                temp.GIOITINH = paramater.GT.Text;
                temp.DCHI = paramater.DC.Text;
                DataProvider.Ins.DB.KHACHHANGs.Add(temp);
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Thêm khách hàng thành công.", "THÔNG BÁO");
            }    
        }
    }
}

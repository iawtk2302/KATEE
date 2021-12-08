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
        public ICommand DeleteOrder { get; set; }
        public DetailOrderViewModel()
        {
            Closewd = new RelayCommand<DetailOrder>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<DetailOrder>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<DetailOrder>((p) => true, (p) => moveWindow(p));
            DeleteOrder = new RelayCommand<DetailOrder>((p) => true, (p) => _DeleteOrder(p));
        }
        void _DeleteOrder(DetailOrder parameter)
        {
            MessageBoxResult h = System.Windows.MessageBox.Show("Bạn muốn xóa hóa đơn này?", "THÔNG BÁO", MessageBoxButton.YesNoCancel,MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                foreach (HOADON temp in DataProvider.Ins.DB.HOADONs)
                {
                    if (temp.SOHD == int.Parse(parameter.SoHD.Text))
                    {
                        foreach (CTHD temp1 in temp.CTHDs)
                        {
                            foreach (SANPHAM temp2 in DataProvider.Ins.DB.SANPHAMs)
                            {
                                if (temp1.MASP == temp2.MASP)
                                {
                                    if(temp2.SL==-1)
                                        temp2.SL += temp1.SL+1;
                                    else if(temp2.SL>=0)
                                        temp2.SL += temp1.SL;
                                }
                            }
                        }
                        DataProvider.Ins.DB.HOADONs.Remove(temp);
                    }
                }
                DataProvider.Ins.DB.SaveChanges();
            }               
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

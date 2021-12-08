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
    class DetailImportViewModel 
    {
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand Delete { get; set; }
        public DetailImportViewModel()
        {
            Closewd = new RelayCommand<DetailImport>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<DetailImport>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<DetailImport>((p) => true, (p) => moveWindow(p));
            Delete = new RelayCommand<DetailImport>((p) => true, (p) => _Delete(p));
        }
        void _Delete(DetailImport parameter)
        {
            MessageBoxResult h = System.Windows.MessageBox.Show("Bạn muốn xóa phiếu nhập này?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                foreach (PHIEUNHAP temp in DataProvider.Ins.DB.PHIEUNHAPs)
                {
                    if (temp.MAPN == int.Parse(parameter.MaPN.Text))
                    {
                        foreach(CTPN temp1 in temp.CTPNs)
                        {
                            foreach(SANPHAM temp2 in DataProvider.Ins.DB.SANPHAMs)
                            {
                                if(temp1.MASP==temp2.MASP)
                                {
                                    if(temp2.SL-temp1.SL<0)
                                    {
                                        MessageBox.Show("Không thể xóa phiếu nhập vì sản phẩm nhập đã được bán !","THÔNG BÁO",MessageBoxButton.OK,MessageBoxImage.Error);
                                        return;
                                    }
                                    else
                                    temp2.SL -= temp1.SL;
                                }    
                            }    
                        }    
                        DataProvider.Ins.DB.PHIEUNHAPs.Remove(temp);
                    }
                }
                DataProvider.Ins.DB.SaveChanges();
            }    
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

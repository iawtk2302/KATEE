﻿using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClothesShopManagement.ViewModel
{
    public class HienThi
    {
        public string MaSp { get; set; }
        public string TenSP { get; set; }
        public int SL { get; set; }
        public int Dongia { get; set; }
        public int Tong { get; set; }
        public string Size { get; set; }
        public HienThi(string MaSp="", string TenSP="",string Size="", int SL=0,int Dongia=0, int Tong=0)
        {
            this.MaSp = MaSp;
            this.TenSP = TenSP;
            this.SL = SL;
            this.Tong = Tong;
            this.Size = Size;
            this.Dongia = Dongia;
        }
    }
    public class AddOrderViewModel : BaseViewModel
    {
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand Choose { get; set; }
        private List<KHACHHANG> _LKH;
        public List<KHACHHANG> LKH { get => _LKH; set { _LKH = value; OnPropertyChanged(); } }
        private List<SANPHAM> _LSP;
        public List<SANPHAM> LSP { get => _LSP; set { _LSP = value; OnPropertyChanged(); } }
        private ObservableCollection<HienThi> _LHT;
        public ObservableCollection<HienThi> LHT { get => _LHT; set { _LHT = value; OnPropertyChanged(); } }
        private ObservableCollection<SANPHAM> _LSPSelected;
        public ObservableCollection<string> LDG { get; set; }
        public ObservableCollection<SANPHAM> LSPSelected { get=> _LSPSelected; set { _LSPSelected = value; OnPropertyChanged(); } }
        private ObservableCollection<CTHD> _LCTHD;
        public ObservableCollection<CTHD> LCTHD { get => _LCTHD; set { _LCTHD = value; OnPropertyChanged(); } }
        public ICommand AddSP { get; set; }
        public ICommand DeleteSP { get; set; }
        public ICommand PrintSP { get; set; }
        public ICommand SaveHD { get; set; }
        public int tongtien { get; set; }
        public AddOrderViewModel()
        {
            tongtien = 0;
            LSPSelected = new ObservableCollection<SANPHAM>();
            LDG = new ObservableCollection<string>() { "1", "2", "3", "4", "5" };
            LHT = new ObservableCollection<HienThi>();
            LCTHD = new ObservableCollection<CTHD>();
            Closewd = new RelayCommand<AddOrderView>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<AddOrderView>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<AddOrderView>((p) => true, (p) => moveWindow(p));
            Loadwd = new RelayCommand<AddOrderView>((p) => true, (p) => _Loadwd(p));
            Choose= new RelayCommand<AddOrderView>((p) => true, (p) => _Choose(p));
            AddSP=new RelayCommand<AddOrderView>((p) => true, (p) => _AddSP(p));
            DeleteSP = new RelayCommand<AddOrderView>((p) => true, (p) => _DeleteSP(p));
            PrintSP = new RelayCommand<AddOrderView>((p) => true, (p) => print(p));
            SaveHD = new RelayCommand<AddOrderView>((p) => true, (p) => _SaveHD(p));
        }
        void moveWindow(AddOrderView p)
        {
            p.DragMove();
        }
        void Close(AddOrderView p)
        {
            p.Close();
        }
        void Minimize(AddOrderView p)
        {
            p.WindowState = WindowState.Minimized;
        }
        void _Loadwd(AddOrderView paramater)
        {
            LKH= DataProvider.Ins.DB.KHACHHANGs.ToList();
            LSP = DataProvider.Ins.DB.SANPHAMs.Where(p=>p.SL>=0).ToList();
            paramater.KH.ItemsSource = LKH;
            paramater.SP.ItemsSource = LSP;
            paramater.MaND.Text = Const.ND.MAND;
            paramater.TenND.Text = Const.ND.TENND;
            paramater.Ngay.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
            paramater.TT.Text = String.Format("{0:0,0}", tongtien) + " VND";
        }
        void _Choose(AddOrderView paramater)
        {
            if(paramater.SP.SelectedItem!=null)
            {
                SANPHAM temp = (SANPHAM)paramater.SP.SelectedItem;
                paramater.DG.Text = temp.GIA.ToString();
            }
            else
            {
                paramater.DG.Text = "";
            }    
        }
        void _AddSP(AddOrderView paramater)
        {
            if(paramater.SP.SelectedItem==null)
            {
                System.Windows.MessageBox.Show("Bạn chưa chọn sản phẩm để thêm !", "THÔNG BÁO");
                return;
            }    
            if(paramater.SoHD.Text=="")
            {
                System.Windows.MessageBox.Show("Bạn chưa nhập số hóa đơn !", "THÔNG BÁO");
                return;
            }
            if (paramater.SL.Text == "")
            {
                System.Windows.MessageBox.Show("Bạn chưa nhập số lượng sản phẩm !", "THÔNG BÁO");
                return;
            }
            foreach (HOADON s in DataProvider.Ins.DB.HOADONs)
            {
                if(int.Parse(paramater.SoHD.Text)==s.SOHD)
                {
                    System.Windows.MessageBox.Show("Số hóa đơn đã tồn tại !", "THÔNG BÁO");
                    return;
                }    
            }    
            SANPHAM a = (SANPHAM)paramater.SP.SelectedItem;
            if (a.SL >= int.Parse(paramater.SL.Text))
            {
                foreach(HienThi temp in LHT)
                {
                    if(temp.MaSp==a.MASP)
                    {
                        temp.SL+=int.Parse(paramater.SL.Text);
                        temp.Tong = temp.SL * a.GIA;
                        foreach(CTHD temp1 in LCTHD)
                        {
                            if(temp1.MASP==a.MASP)
                            {
                                temp1.SL += int.Parse(paramater.SL.Text); ;
                            }    
                        }
                        tongtien += int.Parse(paramater.SL.Text) * a.GIA;
                        paramater.TT.Text = String.Format("{0:0,0}", tongtien) + " VND";
                        paramater.ListViewSP.ItemsSource = LHT;
                        paramater.ListViewSP.Items.Refresh();
                        paramater.SP.SelectedItem = null;
                        paramater.SL.Text = "";
                        return;
                    }    
                }    
                HienThi b = new HienThi(a.MASP, a.TENSP, a.SIZE, int.Parse(paramater.SL.Text),a.GIA, int.Parse(paramater.SL.Text) * a.GIA);  
                CTHD cthd = new CTHD()
                {
                    MASP = a.MASP,
                    SL = int.Parse(paramater.SL.Text),
                    SANPHAM = a,
                    SOHD = int.Parse(paramater.SoHD.Text),
                };
                tongtien += int.Parse(paramater.SL.Text) * a.GIA;
                paramater.TT.Text = String.Format("{0:0,0}", tongtien) + " VND";
                LCTHD.Add(cthd);
                LHT.Add(b);
                //foreach (SANPHAM x in LSP)
                //{
                //    if (x.MASP == a.MASP)
                //        x.SL -= int.Parse(paramater.SL.Text);
                //}
                paramater.ListViewSP.ItemsSource = LHT;
                paramater.ListViewSP.Items.Refresh();
                //paramater.SP.ItemsSource = LSP;
                //paramater.SP.Items.Refresh();
                paramater.SP.SelectedItem = null;
                paramater.SL.Text = "";
            }
            else
                System.Windows.MessageBox.Show("Sản phẩm tồn kho không đủ cung cấp !", "THÔNG BÁO");
        }
        void _DeleteSP(AddOrderView paramater)
        {
            if(paramater.ListViewSP.SelectedItem==null)
            {
                System.Windows.MessageBox.Show("Bạn chưa chọn sản phẩm để xóa !", "THÔNG BÁO");
                return;
            }    
            MessageBoxResult h = System.Windows.MessageBox.Show("  Bạn có chắc muốn xóa sản phẩm.", "THÔNG BÁO", MessageBoxButton.YesNoCancel);
            if (h == MessageBoxResult.Yes)
            {
                HienThi a = (HienThi)paramater.ListViewSP.SelectedItem;
                tongtien -= a.Tong;
                paramater.TT.Text = String.Format("{0:0,0}", tongtien) + " VND";
                LHT.Remove(a);            
                foreach (CTHD b in LCTHD)
                {
                    if (b.MASP == a.MaSp && b.SL == a.SL)
                    {
                        LCTHD.Remove(b);
                        break;
                    }
                }
                paramater.SP.ItemsSource = LSP;
                paramater.SP.Items.Refresh();
                paramater.ListViewSP.Items.Refresh();
            }
            else
                return;
        }
        bool check(int m)
        {
            foreach (HOADON temp in DataProvider.Ins.DB.HOADONs)
            {
                if (temp.SOHD == m)
                    return true;
            }
            return false;
        }
        int rdma()
        {
            int ma;
            do
            {
                Random rand = new Random();
                ma = rand.Next(0, 10000);
            } while (check(ma));
            return ma;
        }
        void _SaveHD(AddOrderView paramater)
        {
            DataProvider.Ins.DB.SaveChangesAsync();
            if(paramater.KH.SelectedItem==null||paramater.ListViewSP.Items.Count==0||paramater.DG1.Text=="")
            {
                System.Windows.MessageBox.Show("Thông tin hóa đơn chưa đầy đủ !", "THÔNG BÁO");
                return;
            }
            MessageBoxResult h = System.Windows.MessageBox.Show("  Bạn muốn thanh toán ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel);
            if (h == MessageBoxResult.Yes)
            {
                KHACHHANG a = (KHACHHANG)paramater.KH.SelectedItem;
                int tonggia = 0;
                foreach (HienThi b in LHT)
                {
                    tonggia += b.Tong;
                }
                HOADON temp = new HOADON()
                {
                    SOHD = int.Parse(paramater.SoHD.Text),
                    MAKH = a.MAKH,
                    MAND = Const.ND.MAND,
                    NGHD = DateTime.Now,
                    CTHDs = new ObservableCollection<CTHD>(LCTHD),
                    TRIGIA = tonggia,
                    DANHGIA=int.Parse(paramater.DG1.Text)
                };
                foreach(CTHD s in LCTHD)
                {
                    foreach (SANPHAM x in DataProvider.Ins.DB.SANPHAMs)
                    {
                        if (x.MASP == s.SANPHAM.MASP)
                        {
                            x.SL -= s.SL;
                        }
                    }
                    //DataProvider.Ins.DB.SaveChanges();
                }
                DataProvider.Ins.DB.HOADONs.Add(temp);
                DataProvider.Ins.DB.SaveChanges();
                MessageBoxResult d = System.Windows.MessageBox.Show("  Bạn có muốn in hóa đơn ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel);
                if (d == MessageBoxResult.Yes)
                {
                    print(paramater);
                }    
                tongtien = 0;
                LSPSelected.Clear();
                paramater.KH.SelectedItem = null;
                LHT.Clear();
                LCTHD.Clear();
                paramater.ListViewSP.ItemsSource = LHT;
                paramater.TT.Text = tongtien.ToString();
                paramater.SoHD.Text=rdma().ToString();
                paramater.DG1.SelectedItem = null;
                MessageBox.Show("Thanh toán hóa đơn thành công !", "THÔNG BÁO");
            }
            else
                return;
        }
        void print(AddOrderView parameter)
        {  
                KHACHHANG temp = (KHACHHANG)parameter.KH.SelectedItem;
                PrintOrderView printOrderView = new PrintOrderView();
                printOrderView.Height = 270 + 35 * LHT.Count;
                printOrderView.TenKH.Text = temp.HOTEN;
                printOrderView.dc.Text = temp.DCHI;
                printOrderView.sdt.Text = temp.SDT;
                printOrderView.ngay.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                printOrderView.sohd.Text = parameter.SoHD.Text;
                printOrderView.ListSP.ItemsSource = LHT;
                printOrderView.tt.Text = parameter.TT.Text;
                try
                {
                    PrintDialog printDialog = new PrintDialog();
                    if (printDialog.ShowDialog() == true)
                    {
                        printDialog.PrintVisual(printOrderView.PrintView, "BILL");
                    }
                }
                finally
                {

                }
                MessageBox.Show("In Hóa đơn thành công !", "THÔNG BÁO");
        }
    }
}

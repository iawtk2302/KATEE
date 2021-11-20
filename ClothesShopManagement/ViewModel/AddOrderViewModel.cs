using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace ClothesShopManagement.ViewModel
{
    public class HienThi
    {
        public string MaSp { get; set; }
        public string TenSP { get; set; }
        public int SL { get; set; }
        public int Tong { get; set; }
        public string Size { get; set; }
        public HienThi(string MaSp="", string TenSP="",string Size="", int SL=0, int Tong=0)
        {
            this.MaSp = MaSp;
            this.TenSP = TenSP;
            this.SL = SL;
            this.Tong = Tong;
            this.Size = Size;
        }
    }
    public class AddOrderViewModel:BaseViewModel
    {
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand Choose { get; set; }
        private List<KHACHHANG> _LKH;
        public List<KHACHHANG> LKH { get=>_LKH; set { _LKH = value;OnPropertyChanged(); } }
        private List<SANPHAM> _LSP;
        public List<SANPHAM> LSP { get => _LSP; set { _LSP = value; OnPropertyChanged(); } }
        private List<HienThi> _LHT;
        public List<HienThi> LHT { get => _LHT; set { _LHT = value; OnPropertyChanged(); } }
        private List<SANPHAM> _LSPSelected;
        public List<SANPHAM> LSPSelected { get=> _LSPSelected; set { _LSPSelected = value; OnPropertyChanged(); } }
        private ObservableCollection<CTHD> _LCTHD;
        public ObservableCollection<CTHD> LCTHD { get => _LCTHD; set { _LCTHD = value; OnPropertyChanged(); } }
        public ICommand AddSP { get; set; }
        public ICommand DeleteSP { get; set; }
        public ICommand SaveHD { get; set; }
        public int tongtien { get; set; }
        public AddOrderViewModel()
        {
            tongtien = 0;
            LSPSelected = new List<SANPHAM>();
            LHT = new List<HienThi>();
            LCTHD = new ObservableCollection<CTHD>();
            Closewd = new RelayCommand<AddOrderView>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<AddOrderView>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<AddOrderView>((p) => true, (p) => moveWindow(p));
            Loadwd = new RelayCommand<AddOrderView>((p) => true, (p) => _Loadwd(p));
            Choose= new RelayCommand<AddOrderView>((p) => true, (p) => _Choose(p));
            AddSP=new RelayCommand<AddOrderView>((p) => true, (p) => _AddSP(p));
            DeleteSP = new RelayCommand<AddOrderView>((p) => true, (p) => _DeleteSP(p));
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
            LSP = DataProvider.Ins.DB.SANPHAMs.ToList();
            paramater.KH.ItemsSource = LKH;
            paramater.SP.ItemsSource = LSP;
            paramater.MaND.Text = Const.ND.TENND;
            paramater.Ngay.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
            SANPHAM a = (SANPHAM)paramater.SP.SelectedItem;
            LSPSelected.Add(a);
            HienThi b = new HienThi(a.MASP,a.TENSP,a.SIZE,int.Parse(paramater.SL.Text), int.Parse(paramater.SL.Text)*a.GIA);
            CTHD cthd = new CTHD()
            {
                MASP = a.MASP,
                SL = int.Parse(paramater.SL.Text),
                DANHGIA = 4,
                SANPHAM = a,
                SOHD =int.Parse(paramater.SoHD.Text),
            };
            tongtien += int.Parse(paramater.SL.Text) * a.GIA;
            paramater.TT.Text = String.Format("{0:0,0}", tongtien)+" VND";
            LCTHD.Add(cthd);
            LHT.Add(b);
            paramater.ListViewSP.ItemsSource= LHT;
            paramater.ListViewSP.Items.Refresh();
            paramater.SP.SelectedItem = null;
            paramater.SL.Text = "";
        }
        void _DeleteSP(AddOrderView paramater)
        {
            MessageBoxResult h = System.Windows.MessageBox.Show("  Bạn có chắc muốn xóa sản phẩm.", "THÔNG BÁO", MessageBoxButton.YesNoCancel);
            if (h == MessageBoxResult.Yes)
            {
                HienThi a = (HienThi)paramater.ListViewSP.SelectedItem;
                tongtien -= a.Tong;
                paramater.TT.Text = String.Format("{0:0,0}", tongtien) + " VND";
                LHT.Remove(a);
                foreach (SANPHAM b in LSPSelected)
                {
                    if (b.MASP == a.MaSp)
                    {
                        LSPSelected.Remove(b);
                        break;
                    }
                }
                foreach (CTHD b in LCTHD)
                {
                    if (b.MASP == a.MaSp && b.SL == a.SL)
                    {
                        LCTHD.Remove(b);
                        break;
                    }
                }
                paramater.ListViewSP.Items.Refresh();
            }
            else
                return;
        }
        void _SaveHD(AddOrderView paramater)
        {
            MessageBoxResult h = System.Windows.MessageBox.Show("  Bạn muốn thanh toán.", "THÔNG BÁO", MessageBoxButton.YesNoCancel);
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
                    TRIGIA = tonggia
                };
                foreach(CTHD s in LCTHD)
                {
                    foreach(SANPHAM x in DataProvider.Ins.DB.SANPHAMs)
                    {
                        if(x.MASP==s.SANPHAM.MASP)
                        {
                            x.SL-=s.SL;
                        }    
                    }
                    DataProvider.Ins.DB.SaveChanges();
                }    
                DataProvider.Ins.DB.HOADONs.Add(temp);
                DataProvider.Ins.DB.SaveChanges();
                System.Windows.MessageBox.Show("Thanh toán thành công", "THÔNG BÁO");
            }
            else
                return;
        }
    }
}

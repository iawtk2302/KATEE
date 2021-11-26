using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ClothesShopManagement.ViewModel
{
    class AddImportViewModel : BaseViewModel
    {
        private List<SANPHAM> _LSP;
        public List<SANPHAM> LSP { get => _LSP; set { _LSP = value; OnPropertyChanged(); } }
        private ObservableCollection<Display> _LHT;
        public ObservableCollection<Display> LHT { get => _LHT; set { _LHT = value; OnPropertyChanged(); } }
        private ObservableCollection<SANPHAM> _LSPSelected;
        public ObservableCollection<SANPHAM> LSPSelected { get => _LSPSelected; set { _LSPSelected = value; OnPropertyChanged(); } }
        private ObservableCollection<CTPN> _LCTPN;
        public ObservableCollection<CTPN> LCTPN { get => _LCTPN; set { _LCTPN = value; OnPropertyChanged(); } }
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand AddSP { get; set; }
        public ICommand DeleteSP { get; set; }
        public ICommand SavePN { get; set; }
        public AddImportViewModel()
        {
            LSPSelected = new ObservableCollection<SANPHAM>();
            LHT = new ObservableCollection<Display>();
            LCTPN = new ObservableCollection<CTPN>();
            Closewd = new RelayCommand<AddImportView>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<AddImportView>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<AddImportView>((p) => true, (p) => moveWindow(p));
            Loadwd = new RelayCommand<AddImportView>((p) => true, (p) => _Loadwd(p));
            AddSP = new RelayCommand<AddImportView>((p) => true, (p) => _AddSP(p));
            DeleteSP = new RelayCommand<AddImportView>((p) => true, (p) => _DeleteSP(p));
            SavePN = new RelayCommand<AddImportView>((p) => true, (p) => _SavePN(p));
        }
        void moveWindow(AddImportView p)
        {
            p.DragMove();
        }
        void Close(AddImportView p)
        {
            p.Close();
        }
        void Minimize(AddImportView p)
        {
            p.WindowState = WindowState.Minimized;
        }
        void _Loadwd(AddImportView paramater)
        {
            LSP = DataProvider.Ins.DB.SANPHAMs.Where(p => p.SL >= 0).ToList();
            paramater.SP.ItemsSource = LSP;
            paramater.MaND.Text = Const.ND.MAND;
            paramater.TenND.Text = Const.ND.TENND;
            paramater.Ngay.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        }
        void _AddSP(AddImportView paramater)
        {
            if (paramater.MaPN.Text == "")
            {
                System.Windows.MessageBox.Show("Bạn chưa nhập mã phiếu nhập!", "THÔNG BÁO");
                return;
            }
            foreach (PHIEUNHAP s in DataProvider.Ins.DB.PHIEUNHAPs)
            {
                if (int.Parse(paramater.MaPN.Text) == s.MAPN)
                {
                    System.Windows.MessageBox.Show("Mã phiếu nhập đã tồn tại !", "THÔNG BÁO");
                    return;
                }
            }
            try
            {
                if (int.Parse(paramater.SL.Text) < 10)
                {
                    System.Windows.MessageBox.Show("Số lượng nhập không được nhỏ hơn 10!", "THÔNG BÁO");
                    return;
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Số lượng nhập không hợp lệ!", "THÔNG BÁO");
                return;
            }
            SANPHAM a = (SANPHAM)paramater.SP.SelectedItem;
            foreach (Display display in paramater.ListViewSP.Items)
            {
                if (display.MaSp == a.MASP)
                {
                    display.SL += int.Parse(paramater.SL.Text);
                    foreach (CTPN ct in LCTPN)
                    {
                        if (ct.MASP == display.MaSp)
                            ct.SL = display.SL;
                    }
                    goto There;
                }
            }
            Display b = new Display(a.MASP, a.TENSP, a.SIZE, int.Parse(paramater.SL.Text));
            CTPN ctpn = new CTPN()
            {
                MASP = a.MASP,
                SL = int.Parse(paramater.SL.Text),
                SANPHAM = a,
                MAPN = int.Parse(paramater.MaPN.Text),
            };
            LCTPN.Add(ctpn);
            LHT.Add(b);/*
            foreach (SANPHAM x in LSP)
            {
                if (x.MASP == a.MASP)
                    x.SL += int.Parse(paramater.SL.Text);
            }*/
            There:
            paramater.ListViewSP.ItemsSource = LHT;
            paramater.ListViewSP.Items.Refresh();
            paramater.SP.ItemsSource = LSP;
            paramater.SP.Items.Refresh();
            paramater.SP.SelectedItem = null;
            paramater.SL.Text = "";            
        }
        void _DeleteSP(AddImportView paramater)
        {
            if (paramater.ListViewSP.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Bạn chưa chọn sản phẩm !", "THÔNG BÁO");
                return;
            }
            MessageBoxResult h = System.Windows.MessageBox.Show("  Bạn có chắc muốn xóa sản phẩm.", "THÔNG BÁO", MessageBoxButton.YesNoCancel);
            if (h == MessageBoxResult.Yes)
            {
                Display a = (Display)paramater.ListViewSP.SelectedItem;
                LHT.Remove(a);
                foreach (SANPHAM b in LSPSelected)
                {
                    if (b.MASP == a.MaSp)
                    {
                        LSPSelected.Remove(b);
                        break;
                    }
                }
                foreach (CTPN b in LCTPN)
                {
                    if (b.MASP == a.MaSp && b.SL == a.SL)
                    {
                        LCTPN.Remove(b);
                        break;
                    }
                }
                paramater.ListViewSP.Items.Refresh();
            }
            else
                return;
        }
        void _SavePN(AddImportView paramater)
        {
            if (paramater.ListViewSP.Items.Count == 0)
            {
                System.Windows.MessageBox.Show("Thông tin phiếu nhập chưa đầy đủ !", "THÔNG BÁO");
                return;
            }
            MessageBoxResult h = System.Windows.MessageBox.Show("Xác nhận nhập hàng?", "THÔNG BÁO", MessageBoxButton.YesNoCancel);
            if (h == MessageBoxResult.Yes)
            {
                PHIEUNHAP temp = new PHIEUNHAP()
                {
                    MAPN = int.Parse(paramater.MaPN.Text),
                    MAND = Const.ND.MAND,
                    NGAYNHAP = DateTime.Now,
                    CTPNs = new ObservableCollection<CTPN>(LCTPN),
                };
                foreach (CTPN s in LCTPN)
                {
                    foreach (SANPHAM x in DataProvider.Ins.DB.SANPHAMs)
                    {
                        if (x.MASP == s.SANPHAM.MASP)
                        {
                            x.SL += s.SL;                            
                        }
                    }
                }
                DataProvider.Ins.DB.PHIEUNHAPs.Add(temp);
                DataProvider.Ins.DB.SaveChanges();
                System.Windows.MessageBox.Show("Nhập hàng thành công", "THÔNG BÁO");
                LHT = new ObservableCollection<Display>();
                paramater.MaPN.Clear();
                LCTPN = new ObservableCollection<CTPN>();
                paramater.ListViewSP.ItemsSource = LHT;
                LSP = DataProvider.Ins.DB.SANPHAMs.Where(p => p.SL >= 0).ToList();
                paramater.SP.Items.Refresh();
            }
            else
                return;
        }
    }
}

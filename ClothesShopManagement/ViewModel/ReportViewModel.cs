using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ClothesShopManagement.ViewModel
{
    public class Review
    {
        public string Type { get; set; }
        public int Num { get; set; }
        public Review(string Type = "", int Num = 0)
        {
            this.Type = Type;
            this.Num = Num;
        }
    }
    public class YData
    {
        private int _Time;
        public int Time { get => _Time; set { _Time = value; } }
        private long _DT;
        public long DT { get => _DT; set { _DT = value; } }
        public YData(int h = 0, long sp = 0)
        {
            Time = h; DT = sp;
        }
    }
    class ReportViewModel : BaseViewModel
    {
        public ICommand GetIdTab { get; set; }
        public ICommand SwitchTab { get; set; }
        private Visibility _SetMain;
        public Visibility SetMain { get => _SetMain; set { _SetMain = value; OnPropertyChanged(); } }
        private Visibility _Up;
        public Visibility Up { get => _Up; set { _Up = value; OnPropertyChanged(); } }
        private Visibility _Down;
        public Visibility Down { get => _Down; set { _Down = value; OnPropertyChanged(); } }
        public string Name;
        public List<Review> Reviews { get; set; }
        public List<YData> YDatas { get; set; }
        public long Tien { get; set; }
        public DateTime Ngay { get; set; }
        public string TenSP { get; set; }
        public string MaSP { get; set; }
        public int SL { get; set; }
        public int MaxSell { get; set; }
        public string BestSeller { get; set; }
        public string SPName { get; set; }
        public int MaxNV { get; set; }
        public string NVName { get; set; }
        public string NVBest { get; set; }
        public long ThisMonth { get; set; }
        public long LastMonth { get; set; }
        private ObservableCollection<string> _Select;
        public ObservableCollection<string> Select { get => _Select; set { _Select = value; OnPropertyChanged(); } }
        public ICommand LoadPie { get; set; }
        public ICommand LoadCol { get; set; }
        public ICommand LoadCbbx { get; set; }
        public ICommand LoadSP { get; set; }
        public ICommand LoadNV { get; set; }
        public ICommand LoadDT { get; set; }

        public ReportViewModel()
        {
            Select = new ObservableCollection<string> { "Theo năm", "Theo tháng" };
            LoadCbbx = new RelayCommand<ReportView>((p) => true, (p) => ColChart(p));
            SetMain = Visibility.Visible;
            GetIdTab = new RelayCommand<RadioButton>((p) => true, (p) => Name = p.Uid);
            SwitchTab = new RelayCommand<ReportView>((p) => true, (p) => switchtab(p));
            LoadPie = new RelayCommand<ReportView>((p) => true, (p) => PieChart(p));
            LoadSP = new RelayCommand<ReportView>((p) => true, (p) => SPCount(p));
            LoadNV = new RelayCommand<ReportView>((p) => true, (p) => NVCount(p));
            LoadDT = new RelayCommand<ReportView>((p) => true, (p) => DTTrend(p));
            //LoadCol = new RelayCommand<ReportView>((p) => true, (p) => ColChart(p));
        }
        public void DTTrend(ReportView p)
        {
            ThisMonth = DataProvider.Ins.DB.HOADONs.Where(x => x.NGHD.Month == DateTime.Now.Month).Sum(x => x.TRIGIA);
            LastMonth = DataProvider.Ins.DB.HOADONs.Where(x => x.NGHD.Month == DateTime.Now.Month - 1).Sum(x => x.TRIGIA);
            long temp = ThisMonth - LastMonth;
            if (temp >= 0)
            {
                p.DTTrend.Text = "+" + temp.ToString("#,### VNĐ");
                p.DTTrend.Foreground = new SolidColorBrush(Color.FromRgb(34, 139, 34));
                Up = Visibility.Visible;
                Down = Visibility.Collapsed;
            }
            else
            {
                p.DTTrend.Text = temp.ToString("#,### VNĐ");
                p.DTTrend.Foreground = new SolidColorBrush(Color.FromRgb(139, 0, 0));
                Up = Visibility.Collapsed;
                Down = Visibility.Visible;
            }
        }
        public void NVCount(ReportView p)
        {
            MaxNV = int.MinValue;
            foreach (HOADON hd in DataProvider.Ins.DB.HOADONs)
            {
                int temp = DataProvider.Ins.DB.HOADONs.Where(x => x.MAND == hd.MAND).Count();
                if (MaxNV < temp)
                {
                    MaxNV = temp;
                    NVBest = hd.MAND;
                }
            }
            NVName = DataProvider.Ins.DB.NGUOIDUNGs.Where(x => x.MAND == NVBest).Select(x => x.TENND).FirstOrDefault();
            p.NVBest.Text = NVBest;
            p.NVName.Text = string.Join(" ", NVName.Split().Reverse().Take(2).Reverse()); 
        }
        public void SPCount(ReportView p)
        {
            MaxSell = int.MinValue;
            var query = from a in DataProvider.Ins.DB.CTHDs
                        join b in DataProvider.Ins.DB.SANPHAMs on a.MASP equals b.MASP
                        where a.MASP == b.MASP
                        select new ReportViewModel()
                        {
                            SL = a.SL,
                            MaSP = a.MASP,
                            TenSP = b.TENSP
                        };
            foreach (ReportViewModel obj in query)
            {
                int temp = query.Where(x => x.TenSP == obj.TenSP).Sum(x => x.SL);
                if (MaxSell < temp)
                {
                    MaxSell = temp;
                    BestSeller = obj.MaSP;
                    SPName = obj.TenSP;
                }
            }
            p.MaxSP.Text = BestSeller;
            p.SPName.Text = SPName;
        }
        public void ColChart(ReportView p)
        {
            if (p.Combobox.SelectedIndex == 0)
            {
                var query = DataProvider.Ins.DB.HOADONs.Select(x => new ReportViewModel()
                {
                    Tien = x.TRIGIA,
                    Ngay = x.NGHD
                });
                YDatas = new List<YData>();
                for (int h = 1; h < 13; h++)
                {
                    long value = 0;
                    if (query.Where(x => x.Ngay.Month == h && x.Ngay.Year == DateTime.Now.Year).Select(x => x.Tien).Count() > 0)
                    {
                        value = query.Where(x => x.Ngay.Month == h && x.Ngay.Year == DateTime.Now.Year).Select(x => x.Tien).Sum();
                    }
                    YData result = new YData(h, value);
                    YDatas.Add(result);
                }                
            }
            else
            {
                var query = DataProvider.Ins.DB.HOADONs.Select(x => new ReportViewModel()
                {
                    Tien = x.TRIGIA,
                    Ngay = x.NGHD
                });
                YDatas = new List<YData>();
                for (int h = 1; h <= 31; h++)
                {
                    long value = 0;
                    if (query.Where(x => x.Ngay.Day == h && x.Ngay.Month == DateTime.Now.Month && x.Ngay.Year == DateTime.Now.Year).Select(x => x.Tien).Count() > 0)
                    {
                        value = query.Where(x => x.Ngay.Day == h && x.Ngay.Month == DateTime.Now.Month && x.Ngay.Year == DateTime.Now.Year).Select(x => x.Tien).Sum();
                    }
                    YData result = new YData(h, value);
                    YDatas.Add(result);
                }
            }
            p.ColChart.ItemsSource = YDatas;
        }
        void PieChart(ReportView p)
        {
            Reviews = new List<Review>();
            Review r1 = new Review()
            {
                Type = "Tích cực",
                Num = DataProvider.Ins.DB.HOADONs.Where(x => x.DANHGIA >= 3).Count()
            };
            Review r2 = new Review()
            {
                Type = "Tiêu cực",
                Num = DataProvider.Ins.DB.HOADONs.Where(x => x.DANHGIA <= 2).Count()
            };
            Reviews.Add(r1);
            Reviews.Add(r2);
            p.Pie.ItemsSource = Reviews;
            p.Pie.AdornmentsInfo = new Syncfusion.UI.Xaml.Charts.ChartAdornmentInfo()
            {
                ShowLabel = true,
                Margin = new Thickness(0)
            };
        }
        void switchtab(ReportView p)
        {

            int index = int.Parse(Name);
            switch (index)
            {
                case 0:
                    {
                        SetMain = Visibility.Visible;
                        break;
                    }
                case 1:
                    {
                        SetMain = Visibility.Hidden;
                        break;
                    }
                case 2:
                    {
                        SetMain = Visibility.Hidden;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}

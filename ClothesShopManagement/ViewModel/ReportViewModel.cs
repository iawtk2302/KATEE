using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

    class ReportViewModel : BaseViewModel
    {
        public ICommand GetIdTab { get; set; }
        public ICommand SwitchTab { get; set; }
        private Visibility _SetMain;
        public Visibility SetMain { get => _SetMain; set { _SetMain = value; OnPropertyChanged(); } }
        public string Name;
        public List<Review> Reviews { get; set; }
        public ICommand LoadPie { get; set; }

        public ReportViewModel()
        {
            SetMain = Visibility.Visible;
            GetIdTab = new RelayCommand<RadioButton>((p) => true, (p) => Name = p.Uid);
            SwitchTab = new RelayCommand<ReportView>((p) => true, (p) => switchtab(p));
            LoadPie = new RelayCommand<ReportView>((p) => true, (p) => PieChart(p));
        }
        void PieChart(ReportView p)
        {
            Reviews = new List<Review>();
            Review r1 = new Review()
            {
                Type = "Tích cực",
                Num = DataProvider.Ins.DB.CTHDs.Where(x => x.DANHGIA >= 3).Count()
            };
            Review r2 = new Review()
            {
                Type = "Tiêu cực",
                Num = DataProvider.Ins.DB.CTHDs.Where(x => x.DANHGIA == 1 || x.DANHGIA == 2).Count()
            };
            Reviews.Add(r1);
            Reviews.Add(r2);
            p.Pie.ItemsSource = Reviews;
            p.Pie.AdornmentsInfo = new Syncfusion.UI.Xaml.Charts.ChartAdornmentInfo()
            {
                ShowLabel = true
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

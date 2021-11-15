using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClothesShopManagement.ViewModel
{
    public class AddProductViewModel:BaseViewModel
    {
        private string _localLink = System.Reflection.Assembly.GetExecutingAssembly().Location.Remove(System.Reflection.Assembly.GetExecutingAssembly().Location.IndexOf(@"bin\Debug"));
        public ICommand AddImage { get; set; }
        private string _linkimage ;
        public string linkimage { get => _linkimage; set { _linkimage = value; OnPropertyChanged();}}
        public ICommand AddProduct { get; set; }
        public ICommand Loadwd { get; set; }
        public AddProductViewModel()
        {
            linkimage = "/Resource/Image/add.png";
            AddImage= new RelayCommand<Image>((p) => true, (p) => _AddImage(p));
            AddProduct = new RelayCommand<AddProductView>((p) => true, (p) => _AddProduct(p));
            Loadwd=new RelayCommand<AddProductView>((p) => true, (p) => _Loadwd(p));
        }
        void _Loadwd(AddProductView paramater)
        {
            linkimage = "/Resource/Image/add.png";
        }
        void _AddImage(Image img)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.png)|*.jpg; *.png";
            if (open.ShowDialog() == true)
            {
                linkimage = open.FileName;
            };
        }
        void _AddProduct(AddProductView paramater)
        {
            if(string.IsNullOrEmpty(paramater.MaSp.Text)|| string.IsNullOrEmpty(paramater.TenSp.Text) || string.IsNullOrEmpty(paramater.LoaiSp.Text) || string.IsNullOrEmpty(paramater.GiaSp.Text) || string.IsNullOrEmpty(paramater.SizeSp.Text) || string.IsNullOrEmpty(paramater.MotaSp.Text) || string.IsNullOrEmpty(paramater.SlSp.Text) || linkimage== "/Resource/Image/add.png")
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin.", "Thông Báo");
            }
            else
            {
                if(DataProvider.Ins.DB.SANPHAMs.Where(p=>p.MASP==paramater.MaSp.Text).Count()>0)
                {
                    MessageBox.Show("Mã sản phẩm đã tồn tại.", "Thông Báo");
                }
                else
                {
                    SANPHAM temp=new SANPHAM();
                    temp.MASP = paramater.MaSp.Text;
                    temp.TENSP = paramater.TenSp.Text;
                    temp.GIA=int.Parse(paramater.GiaSp.Text);
                    temp.LOAISP=paramater.LoaiSp.Text;
                    temp.SL = int.Parse(paramater.SlSp.Text);
                    temp.SIZE = paramater.SizeSp.Text;
                    temp.MOTA = paramater.MotaSp.Text;
                    temp.DVT = "Cái";
                    temp.HINHSP = "/Resource/ImgProduct/" + "product_" + paramater.MaSp.Text+((linkimage.Contains(".jpg")) ? ".jpg" : ".png").ToString();
                    DataProvider.Ins.DB.SANPHAMs.Add(temp);
                    DataProvider.Ins.DB.SaveChanges();
                    try
                    {
                        File.Copy(linkimage, _localLink + @"Resource\ImgProduct\" + "product_" + paramater.MaSp.Text + ((linkimage.Contains(".jpg")) ? ".jpg" : ".png").ToString(), true);
                    }
                    catch { }
                }          
            }    
        }
    }
}

using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ClothesShopManagement.ViewModel
{
    public class AddNDVM:BaseViewModel
    {
        private string _linkaddimage;
        public string linkaddimage { get => _linkaddimage; set { _linkaddimage = value; OnPropertyChanged(); } }
        public ICommand AddNDCommand { get; set; }
        public ICommand AddImage { get; set; }
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public AddNDVM()
        {
            linkaddimage =Const._localLink + "/Resource/Image/addava.png";
            AddNDCommand = new RelayCommand<AddNDView>((p) => true, (p) => _AddND(p));
            AddImage = new RelayCommand<ImageBrush>((p) => true, (p) => _AddImage(p));
            Closewd = new RelayCommand<AddNDView>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<AddNDView>((p) => true, (p) => Minimize(p));
        }
        void Close(AddNDView p)
        {
            linkaddimage = Const._localLink + "/Resource/Image/addava.png";
            p.Close();
        }
        void Minimize(AddNDView p)
        {
            p.WindowState = WindowState.Minimized;
        }
        void _AddImage(ImageBrush img)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.png)|*.jpg; *.png";
            
            if (open.ShowDialog()== true)
            {
                if(open.FileName!="")
                linkaddimage = open.FileName;
            };
            Uri fileUri = new Uri(linkaddimage);
            img.ImageSource = new BitmapImage(fileUri);
        }
        void _AddND(AddNDView addNDView)
        {
            MessageBoxResult h = System.Windows.MessageBox.Show("  Bạn muốn thêm người dùng ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel);
            if (h == MessageBoxResult.Yes)
            {
                if (String.IsNullOrEmpty(addNDView.MaND.Text) || String.IsNullOrEmpty(addNDView.TenND.Text) || String.IsNullOrEmpty(addNDView.SDT.Text) || String.IsNullOrEmpty(addNDView.GT.Text) || String.IsNullOrEmpty(addNDView.QTV.Text) || addNDView.NS.SelectedDate == null)
                {
                    MessageBox.Show("Bạn chưa nhập đầy đủ thông tin !", "THÔNG BÁO");
                    return;
                }
                NGUOIDUNG temp = new NGUOIDUNG();
                foreach (NGUOIDUNG a in DataProvider.Ins.DB.NGUOIDUNGs.Where(p=>p.TTND==true))
                {
                    if (addNDView.MaND.Text == a.MAND)
                    {
                        MessageBox.Show("Mã ND đã tồn tại !", "THÔNG BÁO");
                        return;
                    }
                }
                temp.MAND = addNDView.MaND.Text;
                temp.TENND = addNDView.TenND.Text;
                temp.SDT = addNDView.SDT.Text;
                temp.DIACHI = addNDView.DC.Text;
                temp.GIOITINH = addNDView.GT.Text;
                temp.NGSINH = (DateTime)addNDView.NS.SelectedDate;
                if (addNDView.QTV.Text == "Quản lý")
                    temp.QTV = true;
                else
                    temp.QTV = false;
                temp.TTND = true;
                temp.USERNAME = addNDView.MaND.Text;
                temp.PASS = LoginViewModel.MD5Hash(LoginViewModel.Base64Encode(addNDView.MaND.Text));
                if (linkaddimage == "/Resource/Image/addava.png")
                    temp.AVA = "/Resource/Image/addava.png";
                else
                    temp.AVA = "/Resource/Ava/" + addNDView.MaND.Text + ((linkaddimage.Contains(".jpg")) ? ".jpg" : ".png").ToString();
                DataProvider.Ins.DB.NGUOIDUNGs.Add(temp);
                try
                {
                    File.Copy(linkaddimage, Const._localLink + @"Resource\Ava\" + temp.MAND + ((linkaddimage.Contains(".jpg")) ? ".jpg" : ".png").ToString(), true);
                }
                catch { }
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Thêm người dùng thành công !", "THÔNG BÁO");
            }
        }               
    }
}

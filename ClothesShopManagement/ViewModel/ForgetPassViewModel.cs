using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ClothesShopManagement.ViewModel
{
    public class ForgetPassViewModel
    {
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand SendPass { get; set; }
        public ICommand movewd { get; set; }
        public ForgetPassViewModel()
        {
            Closewd = new RelayCommand<ForgetPassView>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<ForgetPassView>((p) => true, (p) => Minimize(p));
            SendPass = new RelayCommand<ForgetPassView>((p) => true, (p) => _SendPass(p));
            movewd = new RelayCommand<ForgetPassView>((p) => true, (p) => _movewd(p));
        }
        void Close(ForgetPassView p)
        {
            p.Close();
        }
        void _movewd(ForgetPassView p)
        {
            p.DragMove();
        }
        void Minimize(ForgetPassView p)
        {
            p.WindowState = WindowState.Minimized;
        }
        void _SendPass(ForgetPassView parameter)
        {
            int dem = DataProvider.Ins.DB.NGUOIDUNGs.Where(p => p.MAIL == parameter.mail.Text).Count();
            if(dem==0)
            {
                MessageBox.Show("Email này chưa được đăng lý !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }   
            Random rand = new Random();
            string newpass = rand.Next(100000, 999999).ToString();
            foreach(NGUOIDUNG temp in DataProvider.Ins.DB.NGUOIDUNGs)
            {
                if(temp.MAIL==parameter.mail.Text)
                {
                    temp.PASS = LoginViewModel.MD5Hash(LoginViewModel.Base64Encode(newpass));
                    break;
                }    
            }
            DataProvider.Ins.DB.SaveChanges();
            string nd = "Vui lòng nhập mật khẩu " + newpass + " để đăng nhập. Trân trọng !";
            MailMessage message=new MailMessage("clothesmanagement1412@gmail.com", parameter.mail.Text,"Lấy lại mật khẩu",nd);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("tài khoản mail của bạn","mật khẩu mail của bạn");
            smtpClient.Send(message);
            MessageBox.Show("Đã gửi mật khẩu vào Email đăng ký !", "Thông báo");
        }
    }
}

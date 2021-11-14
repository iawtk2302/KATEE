﻿using ClothesShopManagement.Model;
using ClothesShopManagement.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClothesShopManagement.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public static bool IsLogin { get; set; }
        private string _Username;
        public string Username { get => _Username; set { _Username = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
        public ICommand CloseLogin { get; set; }
        public ICommand MinimizeLogin { get; set; }
        public ICommand MoveLogin { get; set; }
        public ICommand Login { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public LoginViewModel()
        {
            IsLogin = false;
            Password = "";
            Username = "";
            CloseLogin = new RelayCommand<LoginWindow>((p) => true, (p) => Close());
            MinimizeLogin = new RelayCommand<LoginWindow>((p) => true, (p) => Minimize(p));
            MoveLogin = new RelayCommand<LoginWindow>((p) => true, (p) => Move(p));
            Login = new RelayCommand<LoginWindow>((p) => true, (p) => login(p));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) => { Password = p.Password; });
        }
        public void Close()
        {
            System.Windows.Application.Current.Shutdown();
        }
        public void Minimize(LoginWindow p)
        {
            p.WindowState = WindowState.Minimized;
        }
        public void Move(LoginWindow p)
        {
            p.DragMove();
        }
        public void login(LoginWindow p)
        {
            if (p == null) return;
            string PassEncode = MD5Hash(Base64Encode(Password));
            var accCount = DataProvider.Ins.DB.NGUOIDUNGs.Where(x => x.USERNAME == Username && x.PASS == PassEncode).Count();
            if (accCount > 0)
            {
                IsLogin = true;
                Const.TenDangNhap = Username;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                p.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Thông báo", MessageBoxButton.OK);
            }
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}

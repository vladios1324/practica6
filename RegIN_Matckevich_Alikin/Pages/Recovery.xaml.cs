using RegIN_Matckevich_Alikin.Classes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RegIN_Matckevich_Alikin.Pages
{
    /// <summary>
    /// Логика взаимодействия для Recovery.xaml
    /// </summary>
    public partial class Recovery : Page
    {
        string OldLogin;
        bool IsCapture = false;
        public Recovery()
        {
            InitializeComponent();
            MainWindow.mainWindow.UserLogin.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogin.HandlerInCorrectLogin += InCorrectLogin;
            Capture.HandlerCorrectCapture += CorrectCapture;
        }

        public void CorrectLogin()
        {
            if (OldLogin != TbLogin.Text)
            {
                UiHelper.SetNotification(LNameUser, "Hi, " + MainWindow.mainWindow.UserLogin.Name, Brushes.Black);

                try
                {
                    var imageSource = UiHelper.ToBitmap(MainWindow.mainWindow.UserLogin.Image);
                    UiHelper.Fade(IUser, () => IUser.Source = imageSource);
                }
                catch (Exception exp)
                {
                    Debug.WriteLine(exp.Message);
                }

                OldLogin = TbLogin.Text;
            }
        }

        public void InCorrectLogin()
        {
            if (LNameUser.Content != "")
            {
                LNameUser.Content = "";
                UiHelper.Fade(IUser, () =>
                    IUser.Source = new BitmapImage(new Uri("pack://application:,,,/Images/ic-user.png")));
            }

            if (TbLogin.Text.Length > 0)
                UiHelper.SetNotification(LNameUser, "Login is incorrect", Brushes.Red);
        }

        public void CorrectCapture()
        {
            Capture.IsEnabled = false;
            IsCapture = true;
            SendNewPassword();
        }

        private void SetLogin(object sender, RoutedEventArgs e)
        {
            if (e is KeyEventArgs key && key.Key != Key.Enter)
                return;

            MainWindow.mainWindow.UserLogin.GetUserLogin(TbLogin.Text);
        }

        public void SendNewPassword()
        {
            if(IsCapture)
            {
                if(MainWindow.mainWindow.UserLogin.Password != String.Empty)
                {
                    UiHelper.Fade(IUser, () => IUser.Source = new BitmapImage(new Uri("pack://application:,,,/Images/ic-email.png")));
                    UiHelper.SetNotification(LNameUser, "An email has been sent to your email.", Brushes.Black);
                    MainWindow.mainWindow.UserLogin.CreateNewPassword();
                }
            }
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}

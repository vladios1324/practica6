using RegIN_Matckevich_Alikin.Classes;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RegIN_Matckevich_Alikin.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        string OldLogin;
        int CountSetPassword;
        bool IsCapture = false;
        public Login()
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
        }

        private void SetPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetPassword();
        }

        public void SetPassword()
        {
            if(MainWindow.mainWindow.UserLogin.Password != String.Empty)
            {
                if (IsCapture)
                {
                    if (MainWindow.mainWindow.UserLogin.Password == TbPassword.Password)
                    {
                        MainWindow.mainWindow.OpenPage(new Pages.Confirmation(Confirmation.TypeConfirmation.Login));
                    }
                    else
                    {
                        if (CountSetPassword > 0)
                        {
                            UiHelper.SetNotification(LNameUser, $"Password is incorrect, {CountSetPassword} attempts left", Brushes.Red);
                            CountSetPassword--;
                        }
                        else
                        {
                            Thread TBlockAutorization = new Thread(BlockAutorization);
                            TBlockAutorization.Start();

                            SendMail.SendMessage("An attempt was made to log into your account.", MainWindow.mainWindow.UserLogin.Login);
                        }
                    }
                }
                else
                    UiHelper.SetNotification(LNameUser, $"Enter capture", Brushes.Red);
            }
        }

        public void BlockAutorization()
        {
            DateTime StartBlock = DateTime.Now.AddMinutes(3);

            Dispatcher.Invoke(() =>
            {
                TbLogin.IsEnabled = false;
                TbPassword.IsEnabled = false;
                Capture.IsEnabled = false;
            });

            for (int i = 0; i < 180; i++)
            {
                string timeLeft = StartBlock.Subtract(DateTime.Now).ToString(@"mm\:ss");

                Dispatcher.Invoke(() =>
                {
                    UiHelper.SetNotification(LNameUser, $"Reauthorization available in: {timeLeft}", Brushes.Red);
                });

                Thread.Sleep(1000);
            }

            Dispatcher.Invoke(() =>
            {
                UiHelper.SetNotification(LNameUser, "Hi, " + MainWindow.mainWindow.UserLogin.Name, Brushes.Black);
                TbLogin.IsEnabled = true;
                TbPassword.IsEnabled = true;
                Capture.IsEnabled = true;
                Capture.CreateCapture();
                IsCapture = false;
                CountSetPassword = 2;
            });
        }

        private void SetLogin(object sender, RoutedEventArgs e)
        {
            if (e is KeyEventArgs key && key.Key != Key.Enter)
                return;

            MainWindow.mainWindow.UserLogin.GetUserLogin(TbLogin.Text);

            if (TbPassword.Password.Length > 0)
                SetPassword();
        }

        private void RecoveryPassword(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Recovery());
        }

        private void OpenRegin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Regin());
        }
    }
}

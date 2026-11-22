using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RegIN_Matckevich_Alikin.Pages
{
    /// <summary>
    /// Логика взаимодействия для Confirmation.xaml
    /// </summary>
    public partial class Confirmation : Page
    {
        public enum TypeConfirmation
        {
            Login,
            Regin
        }

        TypeConfirmation ThisTypeConfirmation;
        private int Code = 0;
        public Confirmation(TypeConfirmation TypeConfirmation)
        {
            InitializeComponent();
            ThisTypeConfirmation = TypeConfirmation;
            SendMailCode();
        }

        public void SendMailCode()
        {
            Code = new Random().Next(100000, 999999);
            Classes.SendMail.SendMessage($"Login code: {Code}", MainWindow.mainWindow.UserLogin.Login);
            Thread TSendMailCode = new Thread(TimerSendMailCode);
            TSendMailCode.Start();
        }

        public void TimerSendMailCode()
        {
            for(int i = 0; i < 60; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    LTimer.Content = $"A second message can be sent after {(60 - i)} seconds";
                });
                Thread.Sleep(1000);
            }
            Dispatcher.Invoke(() =>
            {
                BSendMessage.IsEnabled = true;
                LTimer.Content = "";
            });
        }

        private void SendMail(object sender, RoutedEventArgs e)
        {
            SendMailCode();
        }

        private void SetCode(object sender, RoutedEventArgs e)
        {
            if (TbCode.Text != Code.ToString() || !TbCode.IsEnabled)
                return;

            TbCode.IsEnabled = false;
            if (ThisTypeConfirmation == TypeConfirmation.Login)
                MessageBox.Show("Авторизация пользователя успешно подтверждена.");
            else
            {
                MainWindow.mainWindow.UserLogin.SetUser();
                MessageBox.Show("Регистрация пользователя успешно подтверждена.");
            }
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}

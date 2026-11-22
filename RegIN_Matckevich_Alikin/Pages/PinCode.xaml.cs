using RegIN_Matckevich_Alikin.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegIN_Matckevich_Alikin.Pages
{
    /// <summary>
    /// Логика взаимодействия для PinCode.xaml
    /// </summary>
    public partial class PinCode : Page
    {
        public const int MaxAttempts = 3;
        public static int FailedAttempts = 0;
        public enum TypePin
        {
            Create,
            Login
        }
        TypePin ThisTypePin;
        public PinCode(TypePin typePin)
        {
            InitializeComponent();
            ThisTypePin = typePin;

            if (ThisTypePin == TypePin.Login)
            {
                LMessage.Content = "Введите пин код";
                BSave.Content = "Войти";
                BSkip.Content = "Назад";
            }
        }
        private void InputDigit(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }
        private void EnterPin(object sender, RoutedEventArgs e)
        {
            if (e is KeyEventArgs key && key.Key != Key.Enter)
            {
                return;
            }
            if (TbPin.Password.Length != 4)
            {
                UiHelper.SetNotification(LMessage, "Пин должен состоять из 4 цифр", Brushes.Red);
                return;
            }
            
            if(ThisTypePin == TypePin.Create)
            {
                MainWindow.mainWindow.UserLogin.SetPin(TbPin.Password);
                MessageBox.Show("PIN успешно установлен.");
                MainWindow.mainWindow.OpenPage(new Login());
            }
            else if(MainWindow.mainWindow.UserLogin.CheckPin(TbPin.Password))
            {
                FailedAttempts = 0;
                MessageBox.Show("PIN успешно введен.");
            }
            else
            {
                FailedAttempts++;
                if (FailedAttempts >= MaxAttempts)
                {
                    MessageBox.Show("Превышено количество попыток ввода PIN. Пожалуйста, войдите через логин и пароль.");
                    MainWindow.mainWindow.OpenPage(new Login());
                    return;
                }
                UiHelper.SetNotification(LMessage, "Неверный пин код", Brushes.Red);
                TbPin.Clear();
            }
        }
        private void Skip(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}

using RegIN_Matckevich_Alikin.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace RegIN_Matckevich_Alikin
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindow;
        public User UserLogin = new User();
        public MainWindow()
        {
            InitializeComponent();
            mainWindow = this;
            OpenPage(new Pages.Login());
        }

        public void OpenPage(Page page)
        {
            UiHelper.Fade(frame, () => frame.Navigate(page));
        }
    }
}

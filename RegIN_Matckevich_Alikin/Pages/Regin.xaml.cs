using Microsoft.Win32;
using RegIN_Matckevich_Alikin.Classes;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RegIN_Matckevich_Alikin.Pages
{
    /// <summary>
    /// Логика взаимодействия для Regin.xaml
    /// </summary>
    public partial class Regin : Page
    {
        OpenFileDialog FileDialogImage = new OpenFileDialog();
        bool BCorectLogin = false;
        bool BCorrectPassword = false;
        bool BCorrectConfirmPassword = false;
        bool BSetImages = false;
        static readonly Regex LoginRegex = new Regex(@"([a-zA-Z0-9._-]{4,}@[a-zA-Z0-9._-]{2,}\.[a-zA-Z0-9_-]{2,})");
        static readonly Regex PasswordRegex = new Regex(@"(?=.*[0-9])(?=.*[!@#$%^&?*\-_=])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&?*\-_=]{10,}");
        public Regin()
        {
            InitializeComponent();
            MainWindow.mainWindow.UserLogin.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogin.HandlerInCorrectLogin += InCorrectLogin;
            FileDialogImage.Filter = "PNG (*.png)|*.png|JPG (*.jpg)|*.jpg";
            FileDialogImage.RestoreDirectory = true;
            FileDialogImage.Title = "Choose a photo for your avatar";
        }

        private void CorrectLogin()
        {
            UiHelper.SetNotification(LNameUser, "Login already in use", Brushes.Red);
            BCorectLogin = false;
        }

        private void InCorrectLogin()
        {
            UiHelper.SetNotification(LNameUser, "", Brushes.Black);
        }



        private void SetLogin(object sender, RoutedEventArgs e)
        {
            if (e is KeyEventArgs key && key.Key != Key.Enter)
                return;

            BCorectLogin = LoginRegex.IsMatch(TbLogin.Text);
            if (BCorectLogin)
            {
                UiHelper.SetNotification(LNameUser, "", Brushes.Black);
                MainWindow.mainWindow.UserLogin.GetUserLogin(TbLogin.Text);
            }
            else
                UiHelper.SetNotification(LNameUser, "Invalid login", Brushes.Red);

            OnRegin();
        }

        private void SetPassword(object sender, RoutedEventArgs e)
        {
            if (e is KeyEventArgs key && key.Key != Key.Enter)
                return;

            SetPassword();
        }

        public void SetPassword()
        {
            BCorrectPassword = PasswordRegex.IsMatch(TbPassword.Password);
            if (BCorrectPassword)
            {
                UiHelper.SetNotification(LNameUser, "", Brushes.Black);
                if (TbConfirmPassword.Password.Length > 0)
                    ConfirmPassword(true);
                OnRegin();
            }
            else
                UiHelper.SetNotification(LNameUser, "Invalid password", Brushes.Red);
        }

        private void ConfirmPassword(object sender, RoutedEventArgs e)
        {
            if (e is KeyEventArgs key && key.Key != Key.Enter)
                return;

            ConfirmPassword();
        }

        public void ConfirmPassword(bool Pass = false)
        {
            BCorrectConfirmPassword = TbConfirmPassword.Password == TbPassword.Password;
            if (!BCorrectConfirmPassword)
                UiHelper.SetNotification(LNameUser, "Passwords do not match", Brushes.Red);
            else
            {
                UiHelper.SetNotification(LNameUser, "", Brushes.Black);
                if (!Pass)
                    SetPassword();
            }
        }

        void OnRegin()
        {
            if (!BCorectLogin)
                return;
            if (TbName.Text.Length == 0)
                return;
            if (!BCorrectPassword)
                return;
            if (!BCorrectConfirmPassword)
                return;

            MainWindow.mainWindow.UserLogin.Login = TbLogin.Text;
            MainWindow.mainWindow.UserLogin.Password = TbPassword.Password;
            MainWindow.mainWindow.UserLogin.Name = TbName.Text;
            if (BSetImages)
                MainWindow.mainWindow.UserLogin.Image = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\IUser.jpg");

            MainWindow.mainWindow.UserLogin.DateUpdate = DateTime.Now;
            MainWindow.mainWindow.UserLogin.DateCreate = DateTime.Now;
            MainWindow.mainWindow.OpenPage(new Confirmation(Confirmation.TypeConfirmation.Regin));
        }

        private void SetName(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsLetter(e.Text, 0));
        }

        private void SelectImage(object sender, MouseButtonEventArgs e)
        {
            if (FileDialogImage.ShowDialog() == true)
            {
                using (Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(FileDialogImage.FileName))
                {
                    int NewWidth = 0;
                    int NewHeight = 0;
                    if (image.Width > image.Height)
                    {
                        NewWidth = (int)(image.Width * (256f / image.Height));
                        NewHeight = 256;
                    }
                    else
                    {
                        NewWidth = 256;
                        NewHeight = (int)(image.Height * (256f / image.Width));
                    }
                    image.Resize(NewWidth, NewHeight);
                    image.Save("IUser.jpg");
                }

                using (Aspose.Imaging.RasterImage rasterImage = (Aspose.Imaging.RasterImage)Aspose.Imaging.Image.Load("IUser.jpg"))
                {
                    if (!rasterImage.IsCached)
                    {
                        rasterImage.CacheData();
                    }
                    int X = 0;
                    int Width = 256;
                    int Y = 0;
                    int Height = 256;

                    if (rasterImage.Width > rasterImage.Height)
                        X = (int)((rasterImage.Width - 256f) / 2);
                    else
                        Y = (int)((rasterImage.Height - 256f) / 2);

                    Aspose.Imaging.Rectangle rectangle = new Aspose.Imaging.Rectangle(X, Y, Width, Height);
                    rasterImage.Crop(rectangle);

                    rasterImage.Save("IUser.jpg");
                }

                UiHelper.Fade(IUser, () => IUser.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\IUser.jpg")));
                BSetImages = true;
            }
            else
                BSetImages = false;
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}

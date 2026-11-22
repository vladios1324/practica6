using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RegIN_Matckevich_Alikin.Elements
{
    /// <summary>
    /// Логика взаимодействия для ElementCapture.xaml
    /// </summary>
    public partial class ElementCapture : UserControl
    {
        public CorrectCapture HandlerCorrectCapture;
        public delegate void CorrectCapture();
        string StrCapture = "";
        const int ElementWidth = 280;
        const int ElementHeight = 50;
        readonly Random rnd = new Random();
        public ElementCapture()
        {
            InitializeComponent();
            CreateCapture();
        }

        public void CreateCapture()
        {
            InputeCapture.Text = "";
            Capture.Children.Clear();
            StrCapture = "";
            CreateBackground();
            CreateCode();
        }

        void CreateBackground()
        {

            for (int i = 0; i < 100; i++)
            {
                int back = rnd.Next(0, 10);
                Label LBackground = new Label()
                {
                    Content = back,
                    FontSize = rnd.Next(10, 16),
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromArgb(100, (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255))),
                    Margin = new Thickness(rnd.Next(0, ElementWidth - 20), rnd.Next(0, ElementHeight - 20), 0, 0)
                };
                Capture.Children.Add(LBackground);
            }
        }

        void CreateCode()
        {

            for (int i = 0; i < 4; i++)
            {
                int back = rnd.Next(0, 10);
                Label LCode = new Label()
                {
                    Content = back,
                    FontSize = 30,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255))),
                    Margin = new Thickness(ElementWidth / 2 - 60 + i * 30, rnd.Next(-10, 10), 0, 0)
                };
                StrCapture += back.ToString();
                Capture.Children.Add(LCode);
            }
        }

        public bool OnCapture()
        {
            return StrCapture == InputeCapture.Text;
        }

        private void EnterCapture(object sender, KeyEventArgs e)
        {
            if (InputeCapture.Text.Length == 4)
                if (!OnCapture())
                    CreateCapture();
                else if (HandlerCorrectCapture != null)
                    HandlerCorrectCapture.Invoke();
        }
    }
}
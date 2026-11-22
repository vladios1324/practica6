using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace RegIN_Matckevich_Alikin.Classes
{
    public static class UiHelper
    {
        public static void Fade(UIElement el, Action middle)
        {
            var hide = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.6));
            hide.Completed += (s, e) =>
            {
                middle();
                el.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1.2)));
            };
            el.BeginAnimation(UIElement.OpacityProperty, hide);
        }

        public static void SetNotification(Label label, string message, Brush color)
        {
            label.Content = message;
            label.Foreground = color;
        }


        public static BitmapImage ToBitmap(byte[] Data)
        {
            var img = new BitmapImage();
            using (MemoryStream ms = new MemoryStream(Data))
            {
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = ms;
                img.EndInit();
            }
            return img;
        }
    }
}

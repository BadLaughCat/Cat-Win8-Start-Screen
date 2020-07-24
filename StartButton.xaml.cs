using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace Cat_Win8_Start_Menu
{
    public partial class StartButton : Window
    {
        public StartButton()
        {
            InitializeComponent();
            this.Left = 0;
            this.Top = SystemParameters.PrimaryScreenHeight - 40;
            OnlyOne.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\start.png"));
        }

        private SolidColorBrush normal = new SolidColorBrush(Colors.Transparent);
        private SolidColorBrush hover = new SolidColorBrush(Color.FromArgb(51, 255, 255, 255));
        private SolidColorBrush press = new SolidColorBrush(Color.FromArgb(27, 255, 255, 255));

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Background = hover;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Background = normal;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Background = press;
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Background = hover;
        }
    }
}

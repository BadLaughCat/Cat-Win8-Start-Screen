using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Cat_Win8_Start_Menu
{
    public partial class GroupItem : UserControl
    {
        public GroupItem()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(dida);
        }

        private DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1) };
        private SolidColorBrush hover = new SolidColorBrush(Color.FromArgb(51, 255, 255, 255));
        private SolidColorBrush press = new SolidColorBrush(Color.FromArgb(27, 255, 255, 255));

        private void dida(object sender, EventArgs e)
        {
            var a = ((SolidColorBrush)this.Background).Color.A;
            if (a != 0)
            {
                a -= 3;
                this.Background = new SolidColorBrush(Color.FromArgb(a, 255, 255, 255));
            }
            else
                timer.Stop();
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            timer.Stop();
            this.Background = hover;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            timer.Start();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            this.Background = press;
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            this.Background = hover;
        }
    }
}

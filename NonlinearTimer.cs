using System;
using System.Windows.Threading;

namespace Cat_Win8_Start_Menu
{
    class NonlinearTimer
    {
        public DispatcherTimer AnimationTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1) };
        private double value1;
        private double value2;
        private double value3;
        private double toValue1;
        private double toValue2;
        private double toValue3;
        private double offset;
        
        public void InitializeTimer()
        {
            AnimationTimer.Tick += new EventHandler(dida);
        }

        public void InitializeTimerValues(double Value1, double Value2, double Value3, double ToValue1, double ToValue2, double ToValue3, double Offset)
        {
            value1 = Value1;
            value2 = Value2;
            value3 = Value3;
            toValue1 = ToValue1;
            toValue2 = ToValue2;
            toValue3 = ToValue3;
            offset = Offset;
        }

        private void dida(object sender, EventArgs e)
        {
            double speed1 = 0, speed2 = 0, speed3 = 0;
            speed1 += offset + toValue1 - value1;
            speed1 = Math.Round(speed1 * 0.1);
            value1 += speed1;

            speed2 += offset + toValue2 - value2;
            speed2 = Math.Round(speed2 * 0.1);
            value2 += speed2;

            speed3 += offset + toValue3 - value3;
            speed3 = Math.Round(speed3 * 0.1);
            value3 += speed3;
            if (speed1 == 0 && speed2 == 0 && speed3 == 0)
                AnimationTimer.Stop();
        }

        public void OutputValues(ref double InputValue1, ref double InputValue2, ref double InputValue3)
        {
            InputValue1 = value1;
            InputValue2 = value2;
            InputValue3 = value3;
        }
    }
}

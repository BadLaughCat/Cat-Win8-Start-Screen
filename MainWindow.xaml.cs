using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace Cat_Win8_Start_Menu
{
    public partial class MainWindow : Window
    {
        private double TTBLeft, PLLeft, BothOpacity;
        private NonlinearTimer animation = new NonlinearTimer();
        private double animationWeight = 512;
        private StartButton sb = new StartButton();

        public MainWindow()
        {
            InitializeComponent();
            sb.MouseLeftButtonUp += sbmup;
            sb.Quit.Click += Quit_Click;
            sb.Show();
            this.Left = 0;
            this.Top = 0;
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            ProgramList.MaxHeight = this.Height - 221;
            animation.InitializeTimer();
            animation.AnimationTimer.Tick += new EventHandler(dida);
            TitleTB.Opacity = ProgramList.Opacity = this.Opacity = 0;
            TitleTB.Margin = new Thickness(120 + animationWeight, 47, 0, 0);
            ProgramList.Margin = new Thickness(118 + animationWeight, 154, 118, 50);
            BeginAnimation_Intro();
            BringPrograms();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            sb.Close();
            this.Close();
        }

        private void sbmup(object sender, MouseButtonEventArgs e)
        {
            if (this.Opacity == 0)
                BeginAnimation_Intro();
            else if (this.Opacity == 1)
                BeginAnimation_Out();
        }

        private void BeginAnimation_Intro()
        {
            animation.InitializeTimerValues(120 + animationWeight, 118 + animationWeight, 0, 120, 118, 100, 5);
            SViewer.ScrollToHome();
            animation.AnimationTimer.Start();
        }

        private void BeginAnimation_Out()
        {
            animation.InitializeTimerValues(120, 118, 100, 120 + animationWeight, 118 + animationWeight, 0, -5);
            animation.AnimationTimer.Start();
        }

        private void dida(object sender, EventArgs e)
        {
            animation.OutputValues(ref TTBLeft, ref PLLeft, ref BothOpacity);
            TitleTB.Opacity = ProgramList.Opacity = this.Opacity = BothOpacity / 100;
            TitleTB.Margin = new Thickness(TTBLeft, 47, 0, 0);
            ProgramList.Margin = new Thickness(PLLeft, 154, 118, 50);
        }

        private void BringPrograms()
        {
            var startDirs = new string[] { Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu) };
            SearchFiles(startDirs[0] + @"\Programs", startDirs[1] + @"\Programs");
            SearchDirs(startDirs[0] + @"\Programs", startDirs[1] + @"\Programs");
            foreach (UIElement ui in ProgramList.Children)
            {
                if (ui.ToString() == "Cat_Win8_Start_Menu.GroupTitle")
                    if (ProgramList.Children[ProgramList.Children.IndexOf(ui) + 1].ToString() == "Cat_Win8_Start_Menu.GroupTitle")
                        ui.Visibility = Visibility.Collapsed;
            }
        }

        private void SearchFiles(string path1, string path2)
        {
            var gta = new GroupTitle();
            gta.VerticalAlignment = VerticalAlignment.Top;
            gta.Height = 64;
            gta.Info.Text = "#";
            ProgramList.Children.Add(gta);
            var lista = MixStringList(Directory.GetFiles(path1), Directory.GetFiles(path2));
            var lista_real = new List<string>();
            foreach (string suba in lista)
            {
                switch (FileHelper.GetSaftyFileName(suba, false).Substring(0, 1).ToLower())
                {
                    case "a":
                    case "b":
                    case "c":
                    case "d":
                    case "e":
                    case "f":
                    case "g":
                    case "h":
                    case "i":
                    case "j":
                    case "k":
                    case "l":
                    case "m":
                    case "n":
                    case "o":
                    case "p":
                    case "q":
                    case "r":
                    case "s":
                    case "t":
                    case "u":
                    case "v":
                    case "w":
                    case "x":
                    case "y":
                    case "z":
                        break;
                    default:
                        lista_real.Add(suba);
                        break;
                }
            }
            BringFiles(lista_real.ToArray());

            var letters = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            foreach (string subLetter in letters)
            {
                var gt = new GroupTitle();
                gt.VerticalAlignment = VerticalAlignment.Top;
                gt.Height = 64;
                gt.Info.Text = subLetter.ToUpper();
                ProgramList.Children.Add(gt);

                BringFiles(MixStringList(Directory.GetFiles(path1, subLetter + "*"), Directory.GetFiles(path2, subLetter + "*")).ToArray());
            }
        }

        private void SearchDirs(string path1, string path2)
        {
            var allDirs = MixStringList(Directory.GetDirectories(path1), Directory.GetDirectories(path2));
            foreach (string subDir in allDirs)
            {
                var gt = new GroupTitle();
                gt.VerticalAlignment = VerticalAlignment.Top;
                gt.Height = 64;
                gt.Info.Text = FileHelper.GetSaftyFileName(subDir, true);
                if (Directory.GetFiles(subDir).Length != 0)
                    ProgramList.Children.Add(gt);

                BringFiles(Directory.GetFiles(subDir));
            }
        }

        private void BringFiles(string[] files)
        {
            foreach (string subFile in files)
            {
                var gi = new GroupItem();
                gi.VerticalAlignment = VerticalAlignment.Top;
                gi.Height = 56;
                gi.Tag = subFile;
                gi.Icon.Source = FileHelper.GetFileIcon(subFile);
                gi.Info.Text = FileHelper.GetSaftyFileName(subFile, false);
                gi.MouseLeftButtonUp += new MouseButtonEventHandler(ItemLMUp);
                if (gi.Info.Text != "Desktop" && gi.Info.Text != "desktop")
                    if (subFile.Substring(subFile.Length - 4) == ".lnk")
                        if (File.Exists(FileHelper.GetShortcutTargetPath(subFile)))
                            ProgramList.Children.Add(gi);
            }
        }

        private static List<string> MixStringList(IEnumerable<string> list1, IEnumerable<string> list2)
        {
            var allDirs = new List<string>();
            allDirs = allDirs.Concat(list1).ToList();
            allDirs = allDirs.Concat(list2).ToList();
            allDirs.Sort();
            return allDirs;
        }

        private void ItemLMUp(object sender, MouseButtonEventArgs e)
        {
            FileHelper.RunEXE(((GroupItem)sender).Tag.ToString());
            BeginAnimation_Out();
        }
    }
}

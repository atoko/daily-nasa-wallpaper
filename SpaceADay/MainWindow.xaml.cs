using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using Microsoft.Win32;

namespace SpaceADay
{
	public partial class MainWindow : Window
	{
        private RegistryKey startupKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public MainWindow()
		{
			InitializeComponent();
            carousel.Context.PropertyChanged += Context_PropertyChanged;
		}
        #region Events
        private void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void Context_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Display")
            {
                btnWallpaper.IsEnabled = (sender as FileContext).PathToImage != null;
            }
        }
        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            dpDate.SelectedDate = DateTime.Now;
            StartTimer();
            chkStartup.IsChecked = startupKey.GetValue("SpaceToday") != null;
        }
        private void btnWallpaper_Click(object sender, RoutedEventArgs e)
        {
            string path = Path.GetFullPath(carousel.Context.PathToImage);
            Wallpaper.Set(path, GetSelectedStyle());
        }
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            carousel.Context.SaveLatestPicture(dpDate.SelectedDate.Value);
        }
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        #endregion

        #region Polling
        private void StartTimer()
        {
            var daily = new TimeSpan(8, 0, 0);
            var poll = new System.Timers.Timer(daily.TotalMilliseconds);
            poll.Elapsed += Poll_Elapsed;
            poll.Start();
        }
        private void Poll_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var path = Path.GetFullPath(carousel.Context.SaveLatestPicture(DateTime.Now));
            Wallpaper.Set(path, GetSelectedStyle());
        }
        #endregion

        #region Settings
        private void chkStartup_Checked(object sender, RoutedEventArgs e)
		{
			if (((CheckBox)sender).IsChecked == true)
			{
                startupKey.SetValue("SpaceToday", System.Reflection.Assembly.GetEntryAssembly().Location);
			}
			else
			{
                startupKey.DeleteValue("SpaceToday");
			}
		}
        #endregion
        
        private Wallpaper.Style GetSelectedStyle()
        {
            return (Wallpaper.Style)Enum.Parse(typeof(Wallpaper.Style), cbWallpaperStyle.SelectedItem.ToString());
        }
    }
}

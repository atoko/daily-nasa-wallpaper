using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceADay
{
	public class ApodCard : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private NASA.ApodResponse _current = new NASA.ApodResponse();
		private ImageSource _display;
		private string _path;

		public void NotifyPropertyChanged(string property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}

		public NASA.ApodResponse Current
		{
			get { return _current; }
			set { _current = value; PathToImage = _current.fileName; NotifyPropertyChanged("Current"); NotifyPropertyChanged("CurrentTitle"); NotifyPropertyChanged("CurrentImage"); }
		}

		public string CurrentTitle
		{
			get { return (Current != null) ? Current.title : ""; }
		}
		public string CurrentDescription
		{
			get { return (Current != null) ? Current.explanation : ""; }
		}
		public ImageSource CurrentImage
		{
			get { return _display; }
		}
		public string PathToImage
		{
			get
			{
				return _path;
			}
			set
			{
				using (var stream = (File.Open(value, FileMode.Open)))
				{
					BitmapImage biImg = new BitmapImage();
					MemoryStream ms = new MemoryStream(ImageUtils.ReadAllBytes(stream));
					biImg.BeginInit();
					biImg.StreamSource = ms;
					biImg.EndInit();

					_path = value;
					_display = biImg as ImageSource;
				}
				if (this.PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Display")); }
			}
		}
	}
	public partial class MainWindow : Window
	{

		public Visibility Toggle
		{
			get { return carousel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; }
		}
        public MainWindow()
		{
			InitializeComponent();
            carousel.Context.PropertyChanged += Context_PropertyChanged;
			this.DataContext = this;
		}
		#region Events
		private void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void Context_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
			switch (e.PropertyName)
			{
				case "Selected":
					btnView.IsEnabled = (sender as FileContext).Selected.Count > 0;
					break;
				case "Display":
					btnWallpaper.IsEnabled = (sender as FileContext).PathToImage != null;
					break;
			}
        }
        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            dpDate.SelectedDate = DateTime.Now;
            StartTimer();
            chkStartup.IsChecked = RegistryConfig.OnStartup(null);
			cbWallpaperStyle.SelectedItem = RegistryConfig.Get<string>("wpStyle");
		}
        private void btnWallpaper_Click(object sender, RoutedEventArgs e)
        {
            string path = Path.GetFullPath(carousel.Context.PathToImage);
            Wallpaper.Set(path, GetSelectedStyle());
        }
		private void btnView_Click(object sender, RoutedEventArgs e)
		{
			if (carousel.Context.Selected.Count > 0)
			{
				card.DataContext = carousel.Context.Selected.Select((datum) => { var ac = new ApodCard { Current = datum }; return ac; }).ToList();
			}
			carousel.list.SelectedItem = null;
		}
		private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            carousel.Context.PathToImage = FileContext.SavePicture(dpDate.SelectedDate.Value, carousel.Context);
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
            var daily = new TimeSpan(0, 0, 8);
            var poll = new System.Timers.Timer(daily.TotalMilliseconds);
            poll.Elapsed += Poll_Elapsed;
            poll.Start();
        }
        private void Poll_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
			this.Dispatcher.Invoke(() => {
				//var path = Path.GetFullPath(carousel.Context.SavePicture(DateTime.Now));

				if (false)
					Wallpaper.Set("", GetSelectedStyle());
			});
        }
        #endregion

        #region Settings
        private void chkStartup_Checked(object sender, RoutedEventArgs e)
		{
			RegistryConfig.OnStartup(((CheckBox)sender).IsChecked.GetValueOrDefault(false));
		}

		private void cbWallpaperStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var wpStyle = (sender as ComboBox).SelectedItem;
			if (wpStyle != null)
			{
				RegistryConfig.Set("wpStyle", wpStyle.ToString());
			}
		}
		#endregion

		private Wallpaper.Style GetSelectedStyle()
        {
            return (Wallpaper.Style)Enum.Parse(typeof(Wallpaper.Style), cbWallpaperStyle.SelectedItem.ToString());
        }

		private void card_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (card.DataContext.GetType() == typeof(List<ApodCard>))
			cardSelected.DataContext = (card.DataContext as List<ApodCard>).FirstOrDefault();
		}
	}
}

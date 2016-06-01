using SpaceADay.NASA;
using System.Windows.Controls;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using System;

namespace SpaceADay
{
	public partial class Carousel : UserControl
	{
		public FileContext Context
		{
			set { this.DataContext = value; }
			get { return (this.DataContext as FileContext); }
		}
		public Carousel()
		{
			InitializeComponent();
		}

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListView list = sender as ListView;
			if (list.SelectedItem != null)
			{
				Context.PathToImage = (list.SelectedItem as ApodResponse).fileName.ToString();
			}
			Context.Selected = list.SelectedItems.Cast<ApodResponse>().ToList();
		}
		private void btnLoad_Click(object sender, RoutedEventArgs e)
		{
			progress.Maximum = calendar.SelectedDates.Count;
			progress.Value = 0;
			int p = 0;
			Task.Run((Action)(() => {
				var context = new FileContext();
				foreach (var date in calendar.SelectedDates)
				{
					FileContext.SavePicture(date, context);
					this.Dispatcher.Invoke((Action)(() =>
					{
						p++;
						progress.Value = p;
					}));
				}
				this.Dispatcher.Invoke((Action)(() =>
				{
					Context = context;
				}));
			}));
		}
	}
}

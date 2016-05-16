using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SpaceADay
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			TaskbarIcon tbi = new TaskbarIcon();
			tbi.ToolTipText = "NASA APoD Wallpaper";
			tbi.TrayMouseDoubleClick += tbi_TrayMouseDoubleClick;
			tbi.TrayLeftMouseUp += tbi_TrayMouseDoubleClick;

			this.MainWindow = new MainWindow();

			this.MainWindow.Show();
		}

		void tbi_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
		{
			this.MainWindow.Show();
		}
	}
}

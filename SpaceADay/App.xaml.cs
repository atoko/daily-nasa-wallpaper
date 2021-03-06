﻿using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.IO;

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
			tbi.ToolTipText = "Space Today";
			tbi.TrayMouseDoubleClick += tbi_TrayMouseDoubleClick;
			tbi.TrayLeftMouseUp += tbi_TrayMouseDoubleClick;

			var cm = new ContextMenu();
			tbi.ContextMenu = cm;

			System.Windows.Controls.TextBlock exitButton = new System.Windows.Controls.TextBlock
			{
				Text = "Exit",
				Width = 100
			};
			cm.Items.Add(exitButton);
			(exitButton).MouseUp += (s, ev) => { this.Shutdown(); };


			Directory.CreateDirectory("metadata");
			this.MainWindow = new MainWindow();
			this.MainWindow.Show();
		}

		void tbi_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
		{
			this.MainWindow.Show();
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using Microsoft.Win32;

namespace SpaceADay
{
	public partial class MainWindow : Window
	{
		public enum Style : int
		{
			Tiled,
			Centered,
			Stretched
		}

		public MainWindow()
		{
			InitializeComponent();
			carousel.Context.ReadIndexImagesFromFolder("images");
			this.ShowInTaskbar = false;
			dpDate.SelectedDate = DateTime.Now;
		}
		private void OnClosing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
		}
		private void SaveLatestPicture()
		{
			Directory.CreateDirectory("images");
			DateTime currentDate = dpDate.SelectedDate.Value;
			ApodResponse data = RequestApod(currentDate);

			while (data.type != "image")
			{
				currentDate = currentDate.AddDays(-1);
				data = RequestApod(currentDate);
			}

			var imageUri = data.hdurl != null ? data.hdurl : data.url;
			var imagePath = SaveImageToFile(RequestImage(imageUri), "images/" + currentDate.ToString("yyyy-MM-dd") + ".png");
			carousel.Context.ReadIndexImagesFromFolder("images");
		}
		public static string SaveImageToFile(ImageSource request, string filePath)
		{
			string path = "";
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				BitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(request as BitmapSource));
				encoder.Save(fileStream);
				path = fileStream.Name;
			}

			return path;
		}
		private ImageSource RequestImage(string url)
		{
			var request = System.Net.HttpWebRequest.CreateHttp(url);
	        BitmapImage biImg = new BitmapImage();

			try
			{
				using (var stream = new BinaryReader((request.GetResponse().GetResponseStream())))
				{
					
					MemoryStream ms = new MemoryStream(stream.ReadBytes(90000000));
					biImg.BeginInit();
					biImg.StreamSource = ms;
					biImg.EndInit();

					return (biImg as ImageSource);
				}
			}
			catch
			{
				return (new BitmapImage() as ImageSource);
			}
		}
		struct ApodResponse 
		{
			public string url;
			public string hdurl;
			public string type;

		}
		private ApodResponse RequestApod(DateTime date)
		{
			var url = String.Format("https://api.nasa.gov/planetary/apod?api_key={0}&date={1}",
				"Wobddthfi9ht5UyGRo9qHMBN8RtyuE6wWaFQusTQ",
				date.ToString("yyyy-MM-dd"));
			var request = System.Net.HttpWebRequest.CreateHttp(url);

			try
			{
				using (var stream = new StreamReader(request.GetResponse().GetResponseStream()))
				{
					var obj = (JObject)JsonConvert.DeserializeObject(stream.ReadToEnd());

					var hdsrc = obj.GetValue("hdurl");
					var src = obj.GetValue("url");
					var type = obj.GetValue("media_type");

					return new ApodResponse
					{
						type = type != null ? type.Value<string>() : null,
						url = src != null ? src.Value<string>() : null,
						hdurl = hdsrc != null ? hdsrc.Value<string>() : null
					};
				}
			}
			catch
			{
				return new ApodResponse
				{
					type = "invalid"
				};
			}
		}

		private void SaveLatestPicture(object sender, RoutedEventArgs e)
		{
			SaveLatestPicture();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			string path = System.IO.Path.GetFullPath(carousel.Context.PathToImage);
			Wallpaper.Set(path, (Style)Enum.Parse(typeof(Style), cbWallpaperStyle.SelectedItem.ToString()));
		}

		private	RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
		private void StackPanel_Loaded(object sender, RoutedEventArgs e)
		{
			chkStartup.IsChecked = registryKey.GetValue("SpaceToday") != null;
		}

		private void chkStartup_Checked(object sender, RoutedEventArgs e)
		{
			if (((CheckBox)sender).IsChecked == true)
			{
				registryKey.SetValue("SpaceToday", System.Reflection.Assembly.GetEntryAssembly().Location);
			}
			else
			{
				registryKey.DeleteValue("SpaceToday");
			}
		}
	}
}

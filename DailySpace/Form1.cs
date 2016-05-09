using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailySpace
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		public sealed class Wallpaper
		{
			Wallpaper() { }

			const int SPI_SETDESKWALLPAPER = 20;
			const int SPIF_UPDATEINIFILE = 0x01;
			const int SPIF_SENDWININICHANGE = 0x02;

			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

			public enum Style : int
			{
				Tiled,
				Centered,
				Stretched
			}

			public static void Set(string path, Style style)
			{
				RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
				if (style == Style.Stretched)
				{
					key.SetValue(@"WallpaperStyle", 2.ToString());
					key.SetValue(@"TileWallpaper", 0.ToString());
				}

				if (style == Style.Centered)
				{
					key.SetValue(@"WallpaperStyle", 1.ToString());
					key.SetValue(@"TileWallpaper", 0.ToString());
				}

				if (style == Style.Tiled)
				{
					key.SetValue(@"WallpaperStyle", 1.ToString());
					key.SetValue(@"TileWallpaper", 1.ToString());
				}

				SystemParametersInfo(SPI_SETDESKWALLPAPER,
					0,
					path,
					SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
			}
		}

		public partial class MainWindow : Window
		{
			public MainWindow()
			{
				InitializeComponent();

				Directory.CreateDirectory("images");
				DateTime currentDate = DateTime.Now;
				ApodResponse data = RequestApod(currentDate);

				while (data.type != "image")
				{
					currentDate = currentDate.AddDays(-1);
					data = RequestApod(currentDate);
				}

				var imageUri = data.hdurl != null ? data.hdurl : data.url;
				var imagePath = SaveImageToFile(RequestImage(imageUri), "images/" + currentDate.ToString("yyyy-MM-dd") + ".png");

				Wallpaper.Set(imagePath, Wallpaper.Style.Centered);
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
		}
	}
}

using SpaceADay.NASA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceADay
{
	public class FileContext : INotifyPropertyChanged
	{
		private HashSet<ApodResponse> _files = new HashSet<ApodResponse> {};
		private List<ApodResponse> _selected;
		private ImageSource _display;
		private string _path;

		public FileContext()
		{
			using (File.AppendText("metadata/_.dot")) { }
			ReadIndexImagesFromFolder("images");
		}

		public List<ApodResponse> IndexFiles
		{
			get { return _files.ToList(); }
			set { _files = new HashSet<ApodResponse>(value); if (this.PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("IndexFiles")); } }
		}
		public List<ApodResponse> Selected
		{
			get { return _selected; }
			set { _selected = value; if (this.PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Selected")); } }
		}
		public ImageSource Display
		{
			get { return _display; }
		}
		public string PathToImage
		{
			get {
				return _path;
			}
			set {
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

		public void ReadIndexImagesFromFolder(string data)
        {
			if ((data = (File.ReadAllText("metadata/_.dot"))).Length > 0)
			{
				IndexFiles = ApodResponse.Deserialize<List<ApodResponse>>(data);
			}
		}
        public static string SavePicture(DateTime date, FileContext that)
        {
            Directory.CreateDirectory("images");
            NASA.ApodResponse data;

            do
            {
                data = NASA.ApodClient.RequestApod(date);
                date = date.AddDays(-1);
            }
            while (data.media_type != "image");


            var imageUri = data.hdurl != null ? data.hdurl : data.url;
            var imageData = ImageUtils.RequestImage(imageUri);
			var imageName = data.title;
			foreach (var invalidChar in Path.GetInvalidFileNameChars())
			{
				imageName = new string(imageName.Where( c => c != invalidChar).ToArray());
			}
			imageName = String.Format("images/{1}_{0}.png", imageName.Substring(0, data.title.Length > 15 ? 15 : data.title.Length), data.date);
            var imagePath = ImageUtils.SaveImageToFile(imageData, imageName);
			data.fileName = imagePath;

			if (that != null)
			{
				that._files.Add(data);
				File.WriteAllText("metadata/_.dot", ApodResponse.Serialize(that._files).ToString());
				that.ReadIndexImagesFromFolder("images");
			}
			return imagePath;
        }
        public event PropertyChangedEventHandler PropertyChanged;
	}
	public class RedLetterDayConverter : IValueConverter {
		static Dictionary<DateTime, string> dict = new Dictionary<DateTime, string>();
		static RedLetterDayConverter() {
			dict.Add(new DateTime(2009, 3, 17), "St. Patrick's Day"); dict.Add(new DateTime(2009, 3, 20), "First day of spring"); dict.Add(new DateTime(2009, 4, 1), "April Fools"); dict.Add(new DateTime(2009, 4, 22), "Earth Day"); dict.Add(new DateTime(2009, 5, 1), "May Day"); dict.Add(new DateTime(2009, 5, 10), "Mother's Day"); dict.Add(new DateTime(2009, 6, 21), "First Day of Summer");
		}
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			string text = null;
			if (((DateTime)value).Day == DateTime.Today.Day) text = "Dah";
			return text;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}

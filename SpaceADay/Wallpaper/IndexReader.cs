using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceADay
{
	public class IndexReader : INotifyPropertyChanged
	{
		private IEnumerable<string> _files = new List<string> { "uninitialized", "folder" };
		public IEnumerable<string> IndexFiles
		{
			get { return _files; }
			set { _files = value; if (this.PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("IndexFiles")); } }
		}

		private ImageSource _display;
		private string _path;
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
				using (var stream = new BinaryReader((File.Open(value, FileMode.Open))))
				{
					BitmapImage biImg = new BitmapImage();
					MemoryStream ms = new MemoryStream(stream.ReadBytes(90000000));
					biImg.BeginInit();
					biImg.StreamSource = ms;
					biImg.EndInit();

					_path = value;
					_display = biImg as ImageSource;
				}
				if (this.PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Display")); } 
			}
		}

		public void ReadIndexImagesFromFolder(string folder)
        {
			IndexFiles = Directory.EnumerateFiles(folder);
        }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

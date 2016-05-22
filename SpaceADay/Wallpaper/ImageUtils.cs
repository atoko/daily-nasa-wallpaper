using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceADay
{
    class ImageUtils
    {
        public static ImageSource RequestImage(string url)
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
    }
}

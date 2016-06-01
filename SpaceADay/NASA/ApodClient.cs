using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
namespace SpaceADay.NASA
{
    public class ApodResponse
    {
        public string url = "NA";
        public string hdurl;
        public string media_type;
        public string title = "Welcome";
        public string explanation;
        public string date;
        public string copyright;
		public string fileName;
		public string fileThumb;

		public ApodResponse()
		{
			title = "Welcome to Space Today!";
			explanation = "Select a date range and click load.";
			//explanation = "Don’t trust anyone, cause you only live once. Aliquam imperdiet, ligula vehicula sodales lobortis, dui arcu ultricies libero, vitae tempor eros libero sed neque. Pop a molly, I’m sweatin’, consequat feugiat eros.  How you like your eggs, fried or fertilized? Mi ullamcorper molestie vehicula, nulla est hendrerit ante, eget tempor augue felis ut velit. Sed ut tortor nibh. Phasellus et erat a nisl molestie tempor et at mi. I’m ballin’ hard, I need a jersey on, sollicitudin eget auctor quis, aliquet vitae nulla.";
			date = DateTime.Now.ToShortDateString();
			copyright = "deez nuts";
		}

		public override string ToString()
		{
			return title;
		}
		public override bool Equals(object obj)
		{
			ApodResponse q = obj as ApodResponse;
			return q != null && q.url == this.url;
		}

		public override int GetHashCode()
		{
			return this.url.GetHashCode();
		}
		public static StringWriter Serialize(object o)
		{
			var xs = new XmlSerializer(o.GetType());
			var xml = new StringWriter();
			xs.Serialize(xml, o);

			return xml;
		}
		public static T Deserialize<T>(string xml)
		{
			var xs = new XmlSerializer(typeof(T));
			return (T)xs.Deserialize(new StringReader(xml));
		}
	}
    public class ApodClient
    {
        public static ApodResponse RequestApod(DateTime date)
        {
            var url = string.Format("https://api.nasa.gov/planetary/apod?api_key={0}&date={1}",
                "Wobddthfi9ht5UyGRo9qHMBN8RtyuE6wWaFQusTQ",
                date.ToString("yyyy-MM-dd"));
            var request = System.Net.WebRequest.CreateHttp(url);

            try
            {
                using (var stream = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    var obj = JsonConvert.DeserializeObject<ApodResponse>(stream.ReadToEnd());
                    return obj;
                }
            }
            catch
            {
                return new ApodResponse
                {
                    media_type = "invalid"
                };
            }
        }
    }
}

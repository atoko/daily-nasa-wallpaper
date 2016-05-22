using Newtonsoft.Json;
using System;
using System.IO;

namespace SpaceADay.NASA
{
    public struct ApodResponse
    {
        public string url;
        public string hdurl;
        public string media_type;
        public string title;
        public string explanation;
        public string date;
        public string copyright;
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

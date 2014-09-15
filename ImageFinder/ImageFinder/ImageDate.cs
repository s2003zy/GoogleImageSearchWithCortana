using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Data.Json;

namespace ImageFinder
{
    public class ImageDate
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public int Width{get;set;}
        public int Height{get;set;}

        public ImageDate (string title, string url, int width, int height)
        {
            this.Title = title;
            this.Url = url;
            this.Width = width;
            this.Height = height;
        }
        public override string ToString()
        {
            return this.Title;
        }
    }


    public sealed class ImageDataSource
    {

        private static ImageDataSource _imageDataSource = new ImageDataSource();

        private ObservableCollection<ImageDate> _images = new ObservableCollection<ImageDate>();

        public ObservableCollection<ImageDate> Images
        {
            get { return this._images; }
        }

        public static async Task<IEnumerable<ImageDate>> GetGroupsAsync(string content)
        {
            await _imageDataSource.GetImageDataAsync(content);

            return _imageDataSource.Images;
        }

        public  async Task GetImageDataAsync(string content)
        {
            if (_images.Count != 0)
                _images.Clear();
            string v = "1.0";
            string q = content;
            HttpWebRequest request = HttpWebRequest.CreateHttp("https://ajax.googleapis.com/ajax/services/search/images?v="+v+"&q="+q);

            string jasonText;
            var response = await request.GetResponseAsync();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jasonText = sr.ReadToEnd();
            }

            JsonObject jsonObject = JsonObject.Parse(jasonText);
            JsonObject responseDataObject = jsonObject["responseData"].GetObject();
            JsonArray imageArray = responseDataObject["results"].GetArray();
            foreach(JsonValue image in imageArray)
            {
                JsonObject imageObjcet = image.GetObject();
                ImageDate tmpImage = new ImageDate( imageObjcet["title"].GetString(),
                                         imageObjcet["url"].GetString(),
                                         int.Parse(imageObjcet["width"].GetString()),
                                         int.Parse(imageObjcet["height"].GetString())
                                         );
                _images.Add(tmpImage);

            }
      
            //Debug.WriteLine(jasonText);
        }
    }
}

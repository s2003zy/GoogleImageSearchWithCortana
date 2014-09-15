using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
//using Google.API.Search;
using Windows.UI.Xaml.Media.Imaging;
using ImageFinder.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ImageFinder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShowImage : Page
    {
        private readonly NavigationHelper navigationHelper;

        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ShowImage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

           
        }
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                textBox.Text = e.Parameter.ToString();
                if (textBox.Text != null && textBox.Text != "")
                {
                    getImage();
                }
            }
        }
        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            getImage();
        }


        private async void getImage()
        {

            try
            {
                listView.Items.Clear();

                var imageData = await ImageDataSource.GetGroupsAsync(textBox.Text);
                foreach (ImageDate image in imageData)
                {
                    Uri uri = new Uri(image.Url);
                    Image im = new Image();
                    im.Source = new BitmapImage(uri);
                    listView.Items.Add(im);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("exc:" + ex.Message);
            }
        }

    }
}

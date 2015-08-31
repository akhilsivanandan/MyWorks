using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using ViewModel;
using Windows.UI.Xaml.Controls;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace YouTubeSearch
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private List<ViewModelClass> viewModelClassObject;

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public List<ViewModelClass> ViewModelClassObject
        {
            get
            {
                return viewModelClassObject ?? (viewModelClassObject = new List<ViewModelClass>());
            }
        }

        private async void button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NoSearchText.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            NoResults.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            var searchText = textBox.Text.Replace(' ', '+');
            if (searchText == "")
            {
                NoSearchText.Visibility = Windows.UI.Xaml.Visibility.Visible;
                textBlock1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                return;
            }

            WebRequest webRequestHelper = WebRequest.CreateHttp(string.Format("https://www.googleapis.com/youtube/v3/search?part=snippet&q={0}&type=video&key=AIzaSyA8M6liIdtVZCOibC6LC2sW8dmoJwiKEvA", searchText));
            var results = await webRequestHelper.GetResponseAsync();
            Stream dataStream = results.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            JToken result = JObject.Parse(responseFromServer);
            var values = result["items"];
            if (values != null)
            {
                foreach (var item in values)
                {
                    JObject id = item["id"] as JObject;
                    var vId = id["videoId"].ToString();
                    JObject itemResult = item["snippet"] as JObject;
                    if (itemResult != null)
                    {
                        var vm = new ViewModelClass();
                        vm.Title = itemResult["title"].ToString();
                        vm.Description = itemResult["description"].ToString();
                        vm.VideoId = vId;
                        JObject thumbNails = itemResult["thumbnails"] as JObject;
                        JObject image = thumbNails["medium"] as JObject;
                        vm.Image = image["url"].ToString();
                        ViewModelClassObject.Add(vm);
                    }
                }
            }
            else
            {
                NoResults.Visibility = Windows.UI.Xaml.Visibility.Visible;
                textBlock1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                return;
            }

            listview.ItemsSource = ViewModelClassObject;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void resultButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string url = string.Format("https://www.youtube.com/watch?v={0}", ((sender as Button).DataContext as ViewModelClass).VideoId);
            watchNow.Navigate(new System.Uri(url, System.UriKind.Absolute));
        }
    }
}

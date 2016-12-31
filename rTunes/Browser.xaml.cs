using System.Reflection;
using System.Web;
using System.Windows;
using System.Windows.Controls;

namespace rTunes
{
    /// <summary>
    /// Interaction logic for LyricsFetcher.xaml
    /// </summary>
    public partial class Browser : Window
    {
        public Browser()
        {
            InitializeComponent();

            browser.LoadCompleted += Browser_LoadCompleted;
            browser.Navigated += Browser_Navigated;
        }

        private void Browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // Magic to supress JavaScript errors
            var Info = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (Info != null)
            {
                var ComObj = Info.GetValue(browser);
                if (ComObj != null)
                {
                    ComObj.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, ComObj, new object[] { true });
                }
            }
        }

        private void Browser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //HTMLDocument doc = (HTMLDocument)browser.Document;
            //Debug.WriteLine(doc.doctype.ToString());
        }

        public void NavigateTo(string url)
        {
            browser.Navigate(url);
        }

        public void SearchFor(string name, string artist)
        {
            var strSearch = "http://" + $"www.google.de/search?q=lyrics+" + HttpUtility.UrlEncode(name) + "+" + HttpUtility.UrlEncode(artist);
            browser.Navigate(strSearch);
        }

        private void textBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var strSearch = textBox.Text;
            strSearch.Replace(" ", "+");
            if (e.Key == System.Windows.Input.Key.Return)
            {
                browser.Navigate("http://" + $"www.google.de/search?q=lyrics+{strSearch}");
            }
        }
    }
}

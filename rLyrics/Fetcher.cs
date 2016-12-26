using HtmlAgilityPack;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;


namespace rLyrics
{
    public class Fetcher
    {
        public void SearchFor(string name, string artist)
        {
            var strSearch = "http://" + $"www.google.de/search?q=lyrics+" + HttpUtility.UrlEncode(name) + "+" + HttpUtility.UrlEncode(artist);

            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += DownloadStringCompleted;

            wc.DownloadStringAsync(new System.Uri(strSearch));
        }

        private void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string html = e.Result;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodes = doc.DocumentNode.SelectNodes("//div[@class='g']//h3[@class='r']/a/@href");

            var urls = nodes == null
                ? new List<string>()
                : nodes.Select((n, i) => n.Attributes[0].Value).ToList();

            var myRegEx = new Regex(@"http.*?(?=&amp)");

            foreach (var s in urls)
            {
                var match = myRegEx.Match(s);
                Debug.WriteLine(match.Value);
            }
        }

    }
}

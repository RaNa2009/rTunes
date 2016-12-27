using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;


namespace rLyrics
{
    public class rLyricsEventArgs : EventArgs
    {
        public string Lyrics { get; private set; }
        public rLyricsEventArgs(string lyrics)
        {
            this.Lyrics = lyrics;
        }
    }


    public class Fetcher
    {
        public event EventHandler<rLyricsEventArgs> NewLyrics;

        public void SearchFor(string name, string artist)
        {
            var strSearch = "http://" + $"www.google.de/search?q=lyrics+" + HttpUtility.UrlEncode(name) + "+" + HttpUtility.UrlEncode(artist);

            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += DownloadStringCompleted;
            wc.DownloadStringAsync(new System.Uri(strSearch));
        }

        public async Task<int> SearchForAsync(string name, string artist,  IProgress<string> progress)
        {
            WebClient wc = new WebClient();
            var strSearch = "http://" + $"www.google.de/search?q=lyrics+" + HttpUtility.UrlEncode(name) + "+" + HttpUtility.UrlEncode(artist);
            string html = await wc.DownloadStringTaskAsync(new System.Uri(strSearch));
            progress.Report("Received Google search results...");

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
                progress.Report(match.Value);

                if (match.Value.Contains("azlyrics"))
                {
                    Debug.WriteLine("Searching azlyrics...");
                    WebClient wc2 = new WebClient();
                    wc2.DownloadStringCompleted += LyricsDownloadStringCompleted;
                    wc2.Encoding = Encoding.UTF8;
                    wc2.DownloadStringAsync(new System.Uri(match.Value));
                    continue;
                }

            }
            return 0;
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

                if (match.Value.Contains("azlyrics"))
                {
                    Debug.WriteLine("Searching azlyrics...");
                    WebClient wc2 = new WebClient();
                    wc2.DownloadStringCompleted += LyricsDownloadStringCompleted;
                    wc2.Encoding = Encoding.UTF8;
                    wc2.DownloadStringAsync(new System.Uri(match.Value));
                    continue;
                }
            }
        }

        private void LyricsDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string html = e.Result;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            RemoveComments(doc);

            // azlyrics
            var nodes = doc.DocumentNode.SelectNodes("//div[not(@*)]");
            var test = nodes[0].InnerText;

            Debug.WriteLine(test);

            NewLyrics?.Invoke(this, new rLyricsEventArgs(test));
        }

        private void RemoveComments(HtmlDocument doc)
        {
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//comment()");
            if (nodes != null)
            {
                foreach (HtmlNode node in from cmt in nodes
                                          where (cmt != null
                                                 && cmt.InnerText != null
                                                 && !cmt.InnerText.ToUpper().StartsWith("DOCTYPE"))
                                                 && cmt.ParentNode != null
                                          select cmt)
                {
                    node.ParentNode.RemoveChild(node);
                }
            }
        }
    }
}

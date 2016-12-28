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
        private readonly string[] LyricsSites = { "golyr.de", "azlyrics.com" };

        public event EventHandler<rLyricsEventArgs> NewLyrics;

        public async Task<int> SearchForAsync(string name, string artist,  IProgress<string> progress)
        {
            HtmlDocument docResults = new HtmlDocument();
            HtmlDocument docLyrics = new HtmlDocument();
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;

            progress.Report($"Googling for {name}");

            // Asynchronously fetch Google search results via HTTP
            var strSearch = "http://" + $"www.google.de/search?q=lyrics+" + HttpUtility.UrlEncode(name) + "+" + HttpUtility.UrlEncode(artist);
            string html = await wc.DownloadStringTaskAsync(new Uri(strSearch));

            progress.Report("Received Google search results...");

            // Parse Google HTML result into list of URLs
            docResults.LoadHtml(html);

            // XPath to fetch raw google result urls (this will break if Google changes its results page!)
            var nodes = docResults.DocumentNode.SelectNodes("//div[@class='g']//h3[@class='r']/a/@href");

            var rawGoogleUrls = nodes == null ? new List<string>()
                                              : nodes.Select((n, i) => n.Attributes[0].Value);

            var cleanUrls = rawGoogleUrls.Select(url => new Regex(@"http.*?(?=&amp)").Match(url).Value).ToList();

            // Check each results for known lyrics sites
            foreach (var url in cleanUrls)
            {
                progress.Report($"Prcocessing Google Result: {url}");
                foreach (var site in LyricsSites)
                {
                    if (url.Contains(site))
                    {
                        // Known lyrics site found, try to get lyrics
                        progress.Report($"Trying to get lyrics from {site}...");

                        html = await wc.DownloadStringTaskAsync(new Uri(url));
                        docLyrics.LoadHtml(html);
                        ParseLyrics(docLyrics, site);
                    }
                }
            }
            return 0;
        }

        private void ParseLyrics(HtmlDocument doc, string site)
        {
            string result = "";

            RemoveComments(doc);

            if (site == LyricsSites[0])         // golyr
            {
                var nodes = doc.DocumentNode.SelectNodes("//div[@id='lyrics']");
                result = nodes[0].InnerText;
            }
            else if (site == LyricsSites[1])    // azlyrics
            {
                var nodes = doc.DocumentNode.SelectNodes("//div[not(@*)]");
                result = nodes[0].InnerText;
            }
            else if (site == LyricsSites[2])
            {
                var nodes = doc.DocumentNode.SelectNodes("");
                result = nodes[0].InnerText;
            }

            result.Trim();

            NewLyrics?.Invoke(this, new rLyricsEventArgs(result));
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

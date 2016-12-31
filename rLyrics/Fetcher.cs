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
    
    public partial class Fetcher
    {
        public event EventHandler<rLyricsEventArgs> NewLyrics;

        public async Task<bool> SearchForAsync(string name, string artist,  IProgress<string> progress)
        {
            HtmlDocument docResults = new HtmlDocument();
            HtmlDocument docLyrics = new HtmlDocument();
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;

            progress.Report($"Ask Google about '{name}' by '{artist}'");

            // Asynchronously fetch Google search results via HTTP
            var strSearch = "http://www.google.de/search?q=lyrics+" + HttpUtility.UrlEncode(name) + "+" + HttpUtility.UrlEncode(artist);
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
                //foreach (var site in new string[] { "metrolyrics.com" })
                foreach (var site in LyricsSites)
                {
                    if (url.Contains(site.Key))
                    {
                        // Known lyrics site found, try to get lyrics
                        progress.Report($"Trying to get lyrics from '{site}'");

                        if (site.Key == "songlyrics.com")
                        {
                            wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                        }
                        else
                        {
                            wc.Headers.Clear();
                        }

                        html = await wc.DownloadStringTaskAsync(new Uri(url));

                        progress.Report("Parsing result from lyrics site.");
                        docLyrics.LoadHtml(html);

                        if (ParseLyrics(docLyrics, site.Key, progress))
                            return true;
                    }
                }
            }
            return false;
        }

        private List<MyDelegate<HtmlDocument, string, bool>> test = new List<MyDelegate<HtmlDocument, string, bool>>();
        private bool ParseLyrics(HtmlDocument doc, string site, IProgress<string> progress)
        {
            bool Found = false;
            string result = "";

            RemoveComments(doc);

            Found = LyricsSites[site](doc, out result);

            result = HttpUtility.HtmlDecode(result);
            result = result.Trim();
            result = result.Replace("\n\n\n\n", "\n");

            if (Found)
            {
                progress.Report("Lyrics fetched.");
                NewLyrics?.Invoke(this, new rLyricsEventArgs(result));
            }
            return Found;
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

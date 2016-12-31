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
    public partial class Fetcher
    {
        // Hiermit ein Dictionary string <--> MyDelegate aufbauen und die Funktionen für die verschiedenen Seiten eintragen
        delegate V MyDelegate<T, U, V>(T input, out U output);

        private readonly Dictionary<string, MyDelegate<HtmlDocument, string, bool>> LyricsSites =
            new Dictionary<string, MyDelegate<HtmlDocument, string, bool>>
            {
                { "golyr.de", FetchGolyr },
                { "azlyrics.com", FetchAzlyrics },
                { "songtexte.com", FetchSongtexte},
                { "metrolyrics.com", FetchMetrolyrics},
                { "genius.com", FetchGenius},
                { "lololyrics.com",FetchLololyrics },
                { "songlyrics.com", FetchTemplate }
            };

        static bool FetchGolyr(HtmlDocument doc, out string result)
        {
            result = "";
            var nodes = doc.DocumentNode.SelectNodes("//div[@id='lyrics']");
            if (nodes == null)
                return false;

            var captionNode = nodes[0].SelectSingleNode("h2");      // first child in div is h2 with song caption
            nodes[0].RemoveChild(captionNode);                      // --> remove it
            result = nodes[0].InnerText;
            return true;
        }
        static bool FetchAzlyrics(HtmlDocument doc, out string result)
        {
            result = "";
            var nodes = doc.DocumentNode.SelectNodes("//div[not(@*)]");
            if (nodes == null)
                return false;

            result = nodes[0].InnerText;
            return true;
        }
        static bool FetchSongtexte(HtmlDocument doc, out string result)
        {
            result = "";
            var nodes = doc.DocumentNode.SelectNodes("//div[@id='lyrics']");
            if (nodes == null)
                return false;

            result = nodes[0].InnerText;
            if (result.Contains("Leider kein Songtext vorhanden"))
                return false;

            return true;
        }
        static bool FetchMetrolyrics(HtmlDocument doc, out string result)
        {
            result = "";
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='lyrics-body']//p[@class='verse']");

            if (nodes != null && nodes[0].InnerText.Length > 1)
            {
                // we get the lyrics in verses, add an empty line between each
                StringBuilder sb = new StringBuilder();
                foreach (var node in nodes)
                {
                    sb.AppendLine(node.InnerText);
                    sb.AppendLine();
                }

                result = sb.ToString();
                return true;
            }
            return false;
        }
        static bool FetchGenius(HtmlDocument doc, out string result)
        {
            result = "";
            var nodes = doc.DocumentNode.SelectNodes("//body//div[@class='song_body-lyrics']//p");
            if (nodes == null)
                return false;

            result = nodes[0].InnerText;
            return true;
        }
        static bool FetchLololyrics(HtmlDocument doc, out string result)
        {
            result = "";
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='lyrics_txt']");
            if (nodes == null)
                return false;

            result = nodes[0].InnerText;
            return true;
        }
        static bool FetchTemplate(HtmlDocument doc, out string result)
        {
            result = "";
            return false;
        }

        //else if (site == LyricsSites[7])    // 7: songlyrics.com
        //{
        //    // TODO does not work
        //    // wc empfängt anderes HTML als der Browser.. keine Lyrics drin.. ausgetrickst :-(
        //    var nodes = doc.DocumentNode.SelectNodes("//*[@id='songLyricsDiv-outer']");
        //    if (nodes == null)
        //        return false;

        //    result = nodes[0].InnerText;
        //    Found = true;
        //}

    }
}

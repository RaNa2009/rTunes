using iTunesLib;
using rTunes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace rTunes
{
    public static class RalfiTunes
    {
        static iTunesApp iTunes = new iTunesApp();

        /// <summary>
        /// ctor
        /// </summary>
        static RalfiTunes()
        {
            iTunes.OnPlayerPlayEvent += new _IiTunesEvents_OnPlayerPlayEventEventHandler(OnPlay);
        }

        /// <summary>
        /// Called when iTunes Event "Play" occurs (i.e. new song, next song)
        /// </summary>
        /// <param name="iTrack"></param>
        private static void OnPlay(object iTrack)
        {
            UpdateTrack((IITTrack)iTrack);
        }

        private static void UpdateTrack(IITTrack track)
        {
            if (track != null)
            {
                MainWindow.foo.Name = track.Name;
                MainWindow.foo.Artist = track.Artist;
                MainWindow.foo.Album = track.Album;
                MainWindow.foo.Genre = track.Genre;

                var artworkList = new List<Artwork>();
                var artworkCollection = track.Artwork;
                for (int i=1; i<=artworkCollection.Count; i++)
                {
                    var artwork = new Artwork();
                    artwork.Description = artworkCollection[i].Description;
                    switch (artworkCollection[i].Format)
                    {
                        case ITArtworkFormat.ITArtworkFormatUnknown: artwork.Format = "Unknown"; break;
                        case ITArtworkFormat.ITArtworkFormatBMP: artwork.Format = "bmp"; break;
                        case ITArtworkFormat.ITArtworkFormatJPEG: artwork.Format = "jpg"; break;
                        case ITArtworkFormat.ITArtworkFormatPNG: artwork.Format = "png"; break;
                    }
                    artwork.IsDownloadedArtwork = artworkCollection[i].IsDownloadedArtwork;
                    artworkList.Add(artwork);
                }

                string coverPath;
                if (artworkCollection.Count > 0)
                {
                    coverPath = System.IO.Path.Combine(Environment.CurrentDirectory, $"Art.{artworkList[0].Format}");
                    try
                    {
                        System.IO.File.Delete(coverPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception on Delete Format [{artworkList[0].Format}] for [{track.Name}]");
                    }
                    try
                    {
                        artworkCollection[1].SaveArtworkToFile(coverPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception on SaveArtworkToFile Format [{artworkList[0].Format}] for [{track.Name}]");
                    }
                    MainWindow.foo.Cover = coverPath;
                }

                IITFileOrCDTrack fileTrack = track as IITFileOrCDTrack;
                if (fileTrack != null)
                {
                    MainWindow.foo.Lyrics = fileTrack.Lyrics;
                }
            }
        }

        public static void UpdateCurrentTrack()
        {
            UpdateTrack(iTunes.CurrentTrack);
        }

        public static void Prev()
        {
            iTunes.PreviousTrack();
        }

        public static void PlayPause()
        {
            if (iTunes.PlayerState == ITPlayerState.ITPlayerStatePlaying)
                iTunes.Pause();
            else
                iTunes.Play();
        }

        public static void Next()
        {
            iTunes.NextTrack();
        }
    }
}

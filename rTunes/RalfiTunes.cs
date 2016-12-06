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

        static RalfiTunes()
        {
            iTunes.OnPlayerPlayEvent += new _IiTunesEvents_OnPlayerPlayEventEventHandler(app_OnPlayerPlayEvent);
        }

        static void app_OnPlayerPlayEvent(object iTrack)
        {
            IITTrack currentTrack = (IITTrack)iTrack;
            string trackName = currentTrack.Name;
            string artist = currentTrack.Artist;
            string album = currentTrack.Album;

            IITFileOrCDTrack fileTrack = currentTrack as IITFileOrCDTrack;
            string lyrics = fileTrack.Lyrics;

            MainWindow.main.Track = $"{trackName}\n{artist}";
            MainWindow.main.Lyrics = lyrics;
        }

        public static string FooGetCurrentTrack()
        {
            var currentTrack = iTunes.CurrentTrack;
            string trackName = currentTrack.Name;
            string artist = currentTrack.Artist;
            return $"{trackName}\n{artist}";
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

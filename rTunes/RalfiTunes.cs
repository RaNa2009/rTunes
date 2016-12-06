using iTunesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rTunes
{
    public static class RalfiTunes
    {
        static iTunesApp iTunes = new iTunesApp();

        public static void foo()
        {
            foreach (IITTrack track in iTunes.LibraryPlaylist.Tracks)
            {
                //textBox1.Text += track.Artist + " - " + track.Name + Environment.NewLine;
                Console.WriteLine(track.Name);
            }
        }
    }
}

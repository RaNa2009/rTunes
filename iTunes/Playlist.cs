using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunesWrapper
{
    public class Playlist
    {
        public int TrackCount;
        public string Name { get; set; } = "N/A";
        public double Size { get; set; }
        public int Duration { get; set; }
        public List<Track> Tracks { get; set; }
    }
}

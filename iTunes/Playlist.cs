using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunesWrapper
{
    public class Playlist
    {
        public string Name { get; set; }
        public double Size { get; set; }
        public List<Track> Tracks { get; set; }
        public int TrackCount { get { return Tracks != null ? Tracks.Count : 0; } }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace rTunes.ViewModels
{
    public class Track : INotifyPropertyChanged
    {
        private string _name;
        private string _artist;
        private string _album;
        private string _lyrics;
        private string _genre;
        private string _coverPath;

        public Track() { }

        public string Name { get { return _name; } set { _name = value; Changed(); } }
        public string Artist { get { return _artist; } set { _artist = value; Changed(); } }
        public string Album { get { return _album; } set { _album = value; Changed(); } }
        public string Lyrics { get { return _lyrics; } set { _lyrics = value; Changed(); } }
        public string Genre { get { return _genre; } set { _genre = value; Changed(); } }
        public string Cover { get { return _coverPath; } set { _coverPath = value; Changed(); } }

        #region PropertyNotifcations
        public event PropertyChangedEventHandler PropertyChanged;

        private void Changed([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion  
    }

    public class Artwork
    {
        public string Description { get; set; }
        public string Format { get; set; }
        public bool IsDownloadedArtwork { get; set; }
    }
}

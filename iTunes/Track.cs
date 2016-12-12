using iTunesLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace iTunesWrapper
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

        public Track(IITTrack track)
        {
            _name = track.Name;
            _artist = track.Artist;
            _album = track.Album;
            _genre = track.Genre;

            var artworkList = new List<Artwork>();
            var artworkCollection = track.Artwork;
            for (int i = 1; i <= artworkCollection.Count; i++)
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
                    artworkCollection[1].SaveArtworkToFile(coverPath);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Exception on SaveArtworkToFile Format [{artworkList[0].Format}] for [{track.Name}]");
                }
                _coverPath = coverPath;
            }

            IITFileOrCDTrack fileTrack = track as IITFileOrCDTrack;
            if (fileTrack != null)
            {
                _lyrics = fileTrack.Lyrics;
            }
        }

        public override string ToString()
        {
            return $"{_name}, {_artist}, {_album}, {_genre}";
        }
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

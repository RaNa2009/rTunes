using iTunesLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace iTunesWrapper
{
    public class Track
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Lyrics { get; set; }
        public string Genre { get; set; }
        public string Cover { get; set; }
        public int PlayOrderIndex { get; set; }
        public int SampleRate { get; set; }
        public int BitRate { get; set; }
        public Track() { }

        public Track(IITTrack track, bool SaveArtworkToDisk = true)
        {
            Name = track.Name;
            Artist = track.Artist;
            Album = track.Album;
            Genre = track.Genre;
            PlayOrderIndex = track.PlayOrderIndex;
            BitRate = track.BitRate;
            SampleRate = track.SampleRate;

            if (SaveArtworkToDisk)
            {
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
                    Cover = coverPath;
                }
            }

            IITFileOrCDTrack fileTrack = track as IITFileOrCDTrack;
            if (fileTrack != null)
            {
                Lyrics = fileTrack.Lyrics;
            }
        }

        public override string ToString()
        {
            return $"{Name}, {Artist}, {Album}, {Genre}";
        }
    }

    public class Artwork
    {
        public string Description { get; set; }
        public string Format { get; set; }
        public bool IsDownloadedArtwork { get; set; }
    }
}

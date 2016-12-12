using iTunesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunesWrapper
{
    public class iTunesEventArgs : EventArgs
    {
        public string Title { get; private set; }
        public iTunesEventArgs(string title)
        {
            this.Title = title;
        }
    }


    public class iTunes
    {
        private static iTunes _instance = null;
        private iTunesApp _iTunes;

        private iTunes()
        {
            _iTunes = new iTunesApp();
            _iTunes.OnPlayerPlayEvent += new _IiTunesEvents_OnPlayerPlayEventEventHandler(PlayerPlayEvent);
            _iTunes.OnPlayerStopEvent += new _IiTunesEvents_OnPlayerStopEventEventHandler(PlayerStopEvent);
        }

        public static iTunes Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new iTunes();
                }
                return _instance;
            }
        }

        public void iTunesRelease()
        {
            // how to reset COM interop?!
        }

        public event EventHandler<iTunesEventArgs> Play;
        public event EventHandler<iTunesEventArgs> Stop;

        /// <summary>
        /// Called when iTunes Event "Play" occurs (i.e. new song, next song)
        /// </summary>
        /// <param name="iTrack"></param>
        private void PlayerPlayEvent(object iTrack)
        {
            var track = iTrack as IITTrack;
            Play?.Invoke(this, new iTunesEventArgs(track?.Name));
        }
        private void PlayerStopEvent(object iTrack)
        {
            var track = iTrack as IITTrack;
            Stop?.Invoke(this, new iTunesEventArgs(track?.Name));
        }

        public void Prev()
        {
            _iTunes.PreviousTrack();
        }

        public void PlayPause()
        {
            if (_iTunes.PlayerState == ITPlayerState.ITPlayerStatePlaying)
                _iTunes.Pause();
            else
                _iTunes.Play();
        }

        public void Next()
        {
            _iTunes.NextTrack();
        }

        public Track GetCurrentTrack()
        {
            IITTrack currentTrack = _iTunes.CurrentTrack;
            Track foo = new Track(currentTrack);
            return foo;
        }

        public Playlist GetCurrentPlaylist()
        {
            var pl = _iTunes.CurrentPlaylist;

            if (pl != null)
            {
                var tracks = pl.Tracks;
                for (int i = 1; i <= tracks.Count; i++)
                {
                    Console.WriteLine($"#{i} : {tracks[i].Name}");
                }

                return new Playlist
                {
                    Name = pl.Name,
                    Size = pl.Size,
                    Tracks = new List<Track>()
                };
            }
            return new Playlist();
        }
    }
}

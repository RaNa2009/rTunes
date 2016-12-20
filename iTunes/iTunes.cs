using iTunesLib;
using System;
using System.Collections.Generic;

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

        public void Plus20Secs()
        {
            _iTunes.PlayerPosition = _iTunes.PlayerPosition + 20;
        }
        public Track GetCurrentTrack()
        {
            IITTrack currentTrack = _iTunes.CurrentTrack;
            if (currentTrack != null)
            {
                Track foo = new Track(currentTrack);
                return foo;
            }
            return null;
        }
        public Playlist GetCurrentPlaylist()
        {
            var pl = _iTunes.CurrentPlaylist;

            if (pl != null)
            {
                var tracks = new List<Track>();
                for (int i = 1; i <= pl.Tracks.Count; i++)
                {
                    tracks.Add(new iTunesWrapper.Track(pl.Tracks[i], false));
                }

                return new Playlist
                {
                    Name = pl.Name,
                    Size = pl.Size,
                    Tracks = tracks
                };
            }
            return new Playlist();
        }
    }
}

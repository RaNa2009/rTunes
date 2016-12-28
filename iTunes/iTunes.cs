using iTunesLib;
using Microsoft.VisualStudio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

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
            _iTunes = new iTunesAppClass();
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
        public int GetPosition()
        {
            int result = 0;
            try
            {
                result = _iTunes.PlayerPosition;
            }
            catch (COMException ex)
            {
                // If an unexpected error occurred, e.g. no track is being played, we get E_FAIL
                if (ex.HResult != VSConstants.E_FAIL)
                    Debug.WriteLine($"Unexpected COM error: {ex.Message} [HRESULT = 0x{ex.HResult,8:X}]");
            }
            return result;
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

                int high, low;
                _iTunes.GetITObjectPersistentIDs(currentTrack, out high, out low);
                foo.HighID = high;
                foo.LowID = low;

                return foo;
            }
            return null;
        }

        public void SaveLyrics(Track track, string lyrics)
        {
            IITTrack foo = _iTunes.LibraryPlaylist.Tracks.ItemByPersistentID[track.HighID, track.LowID];
            var bar = foo as IITFileOrCDTrack;
            bar.Lyrics = lyrics;
        }
        public Playlist GetCurrentPlaylist()
        {
            var pl = _iTunes.CurrentPlaylist;

            if (pl != null)
            {
                //var tracks = new List<Track>();
                //for (int i = 1; i <= Math.Min(pl.Tracks.Count, 20); i++)
                //{
                //    tracks.Add(new Track(pl.Tracks[i], false));
                //}

                return new Playlist
                {
                    Name = pl.Name,
                    TrackCount = pl.Tracks.Count,
                    Size = pl.Size,
                    Duration = pl.Duration
                };
            }
            return new Playlist();
        }
    }
}

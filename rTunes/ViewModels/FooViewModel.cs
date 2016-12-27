﻿using iTunesWrapper;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Timers;
using System;

namespace rTunes
{
    public class FooViewModel : NotifyBase, IDisposable
    {
        private bool disposed;

        private Timer PositionTimer;

        public static iTunes iTunesPlayer = iTunes.Instance;

        private int _position;
        public int CurrentPosition {
            get { return _position; }
            set { _position = value;  Changed(); }
        }

        private Track _currentTrack;
        public Track CurrentTrack {
            get { return _currentTrack; }
            set { _currentTrack = value; Changed(); }
        }

        private string _lyrics;
        public string Lyrics
        {
            get { return _lyrics; }
            set { _lyrics = value; Changed(); }
        }

        public FooViewModel()
        {
            iTunesPlayer.Play += PlayHandler;
            iTunesPlayer.Stop += StopHandler;

            App.MyLyrics.NewLyrics += NewLyricsHandler;

            PositionTimer = new Timer(500);
            PositionTimer.Elapsed += new ElapsedEventHandler(PollPosition);
            PositionTimer.Enabled = true;

            CurrentTrack = iTunesPlayer.GetCurrentTrack();
            Lyrics = CurrentTrack?.Lyrics;
        }

        private void PollPosition(object source, ElapsedEventArgs e)
        {
            iTunesPlayer.GetPosition();
            if (CurrentTrack != null)
            {
                CurrentPosition = iTunesPlayer.GetPosition();
            }
        }
        private void PlayHandler(object sender, iTunesEventArgs args)
        {
            CurrentTrack = iTunesPlayer.GetCurrentTrack();
            Lyrics = CurrentTrack.Lyrics;
            Debug.WriteLine($"[{CurrentTrack.Name}, {CurrentTrack.Artist}] [Index {CurrentTrack.PlayOrderIndex}] [BitRate {CurrentTrack.BitRate}] [SampleRate {CurrentTrack.SampleRate}]");
        }
        private void StopHandler(object sender, iTunesEventArgs args)
        {
            Debug.WriteLine("Stopping: " + args.Title);
        }
        private void NewLyricsHandler(object sender, rLyrics.rLyricsEventArgs e)
        {
            Debug.WriteLine("Received new lyrics!");
            Lyrics = e.Lyrics;

        }

        public async void Foobar()
        {
            int dummy = await App.MyLyrics.SearchForAsync(CurrentTrack.Name, CurrentTrack.Artist, new Progress<string>(msg => Debug.WriteLine(msg)));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    PositionTimer.Enabled = false;
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #region Commands
        RelayCommand _playpauseCommand; public ICommand PlayPauseCommand
        {
            get
            {
                if (_playpauseCommand == null)
                {
                    _playpauseCommand = new RelayCommand(param => iTunesPlayer.PlayPause(), param => true);
                }
                return _playpauseCommand;
            }
        }
        RelayCommand _nextCommand; public ICommand NextCommand
        {
            get
            {
                if (_nextCommand == null)
                {
                    _nextCommand = new RelayCommand(param => iTunesPlayer.Next(), param => true);
                }
                return _nextCommand;
            }
        }
        RelayCommand _prevCommand; public ICommand PrevCommand
        {
            get
            {
                if (_prevCommand == null)
                {
                    _prevCommand = new RelayCommand(param => iTunesPlayer.Prev(), param => true);
                }
                return _prevCommand;
            }
        }
        RelayCommand _plus20SecsCommand; public ICommand Plus20SecsCommand
        {
            get
            {
                if (_plus20SecsCommand == null)
                {
                    _plus20SecsCommand = new RelayCommand(param => iTunesPlayer.Plus20Secs(), param => true);
                }
                return _plus20SecsCommand;
            }
        }
        RelayCommand _testCommand; public ICommand TestCommand
        {
            get
            {
                if (_testCommand == null)
                {
                    _testCommand = new RelayCommand(param =>
                    {
                        //var pl = iTunesPlayer.GetCurrentPlaylist();
                        //Debug.WriteLine(pl.TrackCount);

                        //App.MyLyrics.SearchFor(CurrentTrack.Name, CurrentTrack.Artist);
                        Foobar();

                    }, param => true);
                }
                return _testCommand;
            }
        }
        RelayCommand _saveLyricsCommand; public ICommand SaveLyricsCommand
        {
            get
            {
                if (_saveLyricsCommand == null)
                {
                    _saveLyricsCommand = new RelayCommand(param => {
                        iTunesPlayer.SaveLyrics(CurrentTrack, Lyrics);
                    }, param => true);
                }
                return _saveLyricsCommand;
            }
        }

        #endregion
    }
}

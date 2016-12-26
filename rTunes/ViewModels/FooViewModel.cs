﻿using iTunesWrapper;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Timers;

namespace rTunes
{
    public class FooViewModel : NotifyBase
    {
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

        public FooViewModel()
        {
            iTunesPlayer.Play += PlayHandler;
            iTunesPlayer.Stop += StopHandler;

            Timer PositionTimer = new Timer(500);
            PositionTimer.Elapsed += new ElapsedEventHandler(PollPosition);
            PositionTimer.Enabled = true;

            CurrentTrack = iTunesPlayer.GetCurrentTrack();
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
            Debug.WriteLine($"[{CurrentTrack.Name}, {CurrentTrack.Artist}] [Index {CurrentTrack.PlayOrderIndex}] [BitRate {CurrentTrack.BitRate}] [SampleRate {CurrentTrack.SampleRate}]");
        }
        private void StopHandler(object sender, iTunesEventArgs args)
        {
            Debug.WriteLine("Stopping: " + args.Title);
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

                        //App.Lyrics.SearchFor(CurrentTrack.Name, CurrentTrack.Artist);
                        App.MyLyrics.SearchFor(CurrentTrack.Name, CurrentTrack.Artist);


                    }, param => true);
                }
                return _testCommand;
            }
        }
        #endregion
    }
}

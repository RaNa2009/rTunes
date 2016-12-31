using iTunesWrapper;
using System.Diagnostics;
using System.Windows.Input;
using System.Timers;
using System;

namespace rTunes
{
    public class MainViewModel : NotifyBase, IDisposable
    {
        private Common.ILogger _log;
        private bool disposed;
        
        private Timer PositionTimer;

        private static iTunes iTunesPlayer = iTunes.Instance;

        #region DependencyProperties
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
        #endregion

        public MainViewModel(Common.ILogger log)
        {
            _log = log;

            iTunesPlayer.Play += PlayHandler;
            iTunesPlayer.Stop += StopHandler;
            iTunesPlayer.Log += LogHandler;

            App.MyLyrics.NewLyrics += NewLyricsHandler;

            PositionTimer = new Timer(500);
            PositionTimer.Elapsed += new ElapsedEventHandler(PostionHandler);

            CurrentTrack = iTunesPlayer.GetCurrentTrack();
            Lyrics = CurrentTrack?.Lyrics;
        }
        public async void FetchLyrics()
        {
            bool result = await App.MyLyrics.SearchForAsync(CurrentTrack.Name, CurrentTrack.Artist,
                new Progress<string>(msg => _log.Log(msg, "Fetcher")));

            if (!result)
                Log("Could not fetch lyrics.");
        }

        private void Log(string msg)
        {
            _log.Log(msg, GetType().Name);
        }

        #region Eventhandler
        private void PostionHandler(object source, ElapsedEventArgs e)
        {
            iTunesPlayer.GetPosition();
            if (CurrentTrack != null)
            {
                CurrentPosition = iTunesPlayer.GetPosition();
            }
        }
        private void PlayHandler(object sender, iTunesEventArgs args)
        {
            PositionTimer.Enabled = true;
            CurrentTrack = iTunesPlayer.GetCurrentTrack();
            Lyrics = CurrentTrack.Lyrics;
            Log($"[{CurrentTrack.Name}, {CurrentTrack.Artist}] [Index {CurrentTrack.PlayOrderIndex}] [BitRate {CurrentTrack.BitRate}] [SampleRate {CurrentTrack.SampleRate}]");
        }
        private void StopHandler(object sender, iTunesEventArgs args)
        {
            PositionTimer.Enabled = false;
            Log("Stopping: " + args.Title);
        }
        private void LogHandler(object sender, iTunesEventArgs args)
        {
            _log.Log(args.Title, "iTunesWrapper");
        }
        private void NewLyricsHandler(object sender, rLyrics.rLyricsEventArgs e)
        {
            Log("NewLyricsHandler was called");
            Lyrics = e.Lyrics;
            if (e.Lyrics.Length > 0)
                iTunesPlayer.SaveLyrics(CurrentTrack, Lyrics);
        }
        #endregion

        #region Lifecycle
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("Stopping iTunes Timer");
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
        #endregion

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
                    //(RoutedCommnad)_nextCommand.InputGestures.Add(new KeyGesture(Key.Right));
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
                    }, param => true);
                }
                return _testCommand;
            }
        }
        RelayCommand _fetchLyricsCommand; public ICommand FetchLyricsCommand
        {
            get
            {
                if (_fetchLyricsCommand == null)
                {
                    _fetchLyricsCommand = new RelayCommand(param => {
                        FetchLyrics();
                    }, param => true);
                }
                return _fetchLyricsCommand;
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

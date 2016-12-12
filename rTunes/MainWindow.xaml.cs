using System.Windows;
using iTunesWrapper;
using System.Diagnostics;

namespace rTunes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static iTunes iTunesPlayer = iTunes.Instance;
        private Track _track;

        private void PlayHandler(object sender, iTunesEventArgs args)
        {
            Debug.WriteLine(args.Title);
            _track = iTunesPlayer.GetCurrentTrack();
        }

        public MainWindow()
        {
            InitializeComponent();
            iTunesPlayer.Play += PlayHandler;
            //_track = iTunesPlayer.GetCurrentTrack();
        }


        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            iTunesPlayer.Prev();
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            iTunesPlayer.PlayPause();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            iTunesPlayer.Next();
        }
    }
}

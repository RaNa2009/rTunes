using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace rTunes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow main;
        internal string Track
        {
            set { Dispatcher.Invoke(new Action(() => { lblTrack.Content = value; })); }
        }
        internal string Lyrics
        {
            set { Dispatcher.Invoke(new Action(() => { tbLyrics.Text = value; })); }
        }


        public MainWindow()
        {
            InitializeComponent();
            main = this;
            lblTrack.Content = RalfiTunes.FooGetCurrentTrack();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            RalfiTunes.Prev();
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            RalfiTunes.PlayPause();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            RalfiTunes.Next();
        }
    }
}

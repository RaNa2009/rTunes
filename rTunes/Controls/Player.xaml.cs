using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace rTunes
{
    public partial class PlayerControl : UserControl
    {
        public PlayerControl()
        {
            InitializeComponent();
            PlayerGrid.DataContext = this;
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(int), typeof(PlayerControl), new PropertyMetadata(null));
        public int Position
        {
            get { return (int)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty MaxPositionProperty =
            DependencyProperty.Register("MaxPosition", typeof(int), typeof(PlayerControl), new PropertyMetadata(null));
        public int MaxPosition
        {
            get { return (int)GetValue(MaxPositionProperty); }
            set { SetValue(MaxPositionProperty, value); }
        }

        public static readonly DependencyProperty PlayPauseProperty =
            DependencyProperty.Register("PlayPause", typeof(ICommand), typeof(PlayerControl), new UIPropertyMetadata(null));
        public ICommand PlayPause
        {
            get { return (ICommand)GetValue(PlayPauseProperty); }
            set { SetValue(PlayPauseProperty, value); }
        }
        public static readonly DependencyProperty PrevProperty =
            DependencyProperty.Register("Prev", typeof(ICommand), typeof(PlayerControl), new UIPropertyMetadata(null));
        public ICommand Prev
        {
            get { return (ICommand)GetValue(PrevProperty); }
            set { SetValue(PrevProperty, value); }
        }
        public static readonly DependencyProperty NextProperty =
            DependencyProperty.Register("Next", typeof(ICommand), typeof(PlayerControl), new UIPropertyMetadata(null));
        public ICommand Next
        {
            get { return (ICommand)GetValue(NextProperty); }
            set { SetValue(NextProperty, value); }
        }
        public static readonly DependencyProperty Plus20SecsProperty =
            DependencyProperty.Register("Plus20Secs", typeof(ICommand), typeof(PlayerControl), new UIPropertyMetadata(null));
        public ICommand Plus20Secs
        {
            get { return (ICommand)GetValue(Plus20SecsProperty); }
            set { SetValue(Plus20SecsProperty, value); }
        }
    }
}

using System.Collections.ObjectModel;
using System.Windows;

namespace rTunes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Common.ILogger log)
        {
            DataContext = new MainViewModel(log);
            InitializeComponent();
        }
    }
}

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
using System.Windows.Shapes;

namespace rTunes.Common
{
    /// <summary>
    /// Interaction logic for LoggerWindow.xaml
    /// </summary>
    public partial class LoggerWindow : Window
    {
        private Paragraph _para;

        public LoggerWindow()
        {
            InitializeComponent();
            _para = new Paragraph();
            textBox.Document = new FlowDocument(_para);
        }

        public void WriteLog(string message, string sender = "")
        {
            SolidColorBrush brush = Brushes.Black;

            int color = sender.Length > 0 ? sender[0] : 0;
            if (color > 'M') brush = Brushes.Red;
            if (color > 'F') brush = Brushes.Blue;
            if (color > 'a') brush = Brushes.Green;

            Dispatcher.Invoke(new Action( () => {
                _para.Inlines.Add(new Bold(new Run($"{sender} ")) { Foreground = brush });
                _para.Inlines.Add(message);
                _para.Inlines.Add(new LineBreak());
            }));
        }
    }
}

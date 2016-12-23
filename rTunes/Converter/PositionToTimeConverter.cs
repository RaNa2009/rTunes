using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace rTunes
{
    public class PositionToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = null;
            var position = new TimeSpan(0,0,0,(int)value);

            if (position != null)
            {
                result = position.ToString(@"m\:ss");
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

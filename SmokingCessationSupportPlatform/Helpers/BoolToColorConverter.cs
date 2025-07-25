using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Markup;

namespace SmokingCessationSupportPlatform.Helpers
{
    public class BoolToColorConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isUnread = value is bool b && b;
            return isUnread ? new SolidColorBrush(Color.FromRgb(232, 245, 253)) : new SolidColorBrush(Color.FromRgb(245, 245, 245));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}

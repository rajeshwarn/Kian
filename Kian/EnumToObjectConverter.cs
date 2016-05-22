using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Kian
{
    [ContentProperty("Items")]
    public class EnumToObjectConverter : IValueConverter
    {
        public ResourceDictionary Items { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string key = Enum.GetName(value.GetType(), value);
            return Items[key];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("This converter only works for one way binding");
        }
    }
}
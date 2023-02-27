using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ProductsClient.Classes
{
    public class MultiplicationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            float result = 1;
            values.ToList().ForEach(x => result *= float.Parse(x.ToString()));

            return result.ToString("C2", new CultureInfo("ru"));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { Binding.DoNothing, false };
        }
    }
}

using ProductsClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProductsClient.Classes
{
    public class MinusConverter : IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<SaleProduct> sales = value as ObservableCollection<SaleProduct>;

            float result = 0;
            sales.ToList().ForEach(x => result += float.Parse(x.Count.ToString()) * float.Parse(x.Product.SellCost.ToString()));

            return result.ToString("C2", new CultureInfo("ru"));
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            List<ArrivalProduct> arrivals = new List<ArrivalProduct>();
            values.ToList().ForEach(x => arrivals.Add(x as ArrivalProduct));
            List<SaleProduct> sales = parameter as List<SaleProduct>;
            sales.ToList().ForEach(x =>
            {
                arrivals.ToList().Find(y => y.Id.Equals(x.Product.Id)).Count -= x.Count;
            });

            return arrivals;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { Binding.DoNothing };
        }
    }
}
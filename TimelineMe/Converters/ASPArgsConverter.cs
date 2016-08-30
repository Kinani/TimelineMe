using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace TimelineMe.Converters
{
    public class ASPArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            
            var args = value as AutoSuggestBoxQuerySubmittedEventArgs;
            
            return args;

            //if (value != null)
            //{
            //    var args = (AutoSuggestBoxQuerySubmittedEventArgs)value;

            //    return args;
            //}
            //else
            //    return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

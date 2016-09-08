using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace TimelineMe.Converters
{
    public class ToggleToggledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ToggleSwitch tgleSw = value as ToggleSwitch;
            if (tgleSw != null)
            {
                if (tgleSw.IsOn == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

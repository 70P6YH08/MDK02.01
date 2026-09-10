using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Task3.Services
{
    public class PasswordConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            PasswordBox[] passwordBoxes = new PasswordBox[2];
            passwordBoxes[0] = values[0] as PasswordBox;
            passwordBoxes[1] = values[1] as PasswordBox;
            return passwordBoxes;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

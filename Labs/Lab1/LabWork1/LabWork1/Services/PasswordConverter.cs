using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace LabWork1.Services
{
    public class PasswordConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return false;

            PasswordBox[] pwdBoxes = new PasswordBox[values.Length];
            for (int i = 0; i < values.Length; i++)
                pwdBoxes[i] = values[i] as PasswordBox;
            return pwdBoxes;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

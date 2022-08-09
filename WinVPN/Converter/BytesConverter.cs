using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WinVPN.Converter
{
    [ValueConversion(typeof(long), typeof(long))]
    internal class BytesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long val = (long)value;

            return SizeFormat(val);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public string SizeFormat(double size)
        {
            if(size <= 0)
            {
                return "0";
            }
            int i = 0;
            for (; size >= 1024; i++)
            {
                size /= 1024;
            }
            string[] units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
            double val = Math.Floor(size * 100d) / 100;
            return string.Format("{0}{1}", val, units[i]);
        }
    }
}

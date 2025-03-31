using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AlghorithmsApp.Resources
{
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Преобразование double в строку для отображения
            if (value is double doubleValue)
            {
                return doubleValue.ToString(CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Преобразование строки в double с учетом разных форматов чисел
            if (value is string stringValue)
            {
                // Пробуем разные форматы чисел
                if (double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                {
                    return result;
                }

                // Замена запятых на точки для русской культуры
                if (double.TryParse(stringValue.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                {
                    return result;
                }

                // Замена точек на запятые для европейского формата
                if (double.TryParse(stringValue.Replace('.', ','), NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                {
                    return result;
                }
            }
            return 0.0; // Значение по умолчанию при ошибке
        }
    }
}


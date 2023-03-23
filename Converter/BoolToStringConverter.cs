using System;
using System.Globalization;
using System.Windows.Data;

namespace Aurora.Wpf.Converter
{
    /// <summary>
    /// ValueConverter class to translate bool values into 2 diffenrent strings
    /// </summary>
    public class BoolToStringConverter : IValueConverter
    {
        /// <summary>
        /// separator char to separate the two string values
        /// </summary>
        public char Separator { get; set; } = ';';

        /// <summary>
        /// Convert the bool value into string values
        /// </summary>
        /// <param name="value">boolean to convert</param>
        /// <param name="targetType">type of target element</param>
        /// <param name="parameter">containing the 2 strings separated by the given char</param>
        /// <param name="culture">CultureIfo to use to display the texts</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            var strings = ((string)parameter).Split(Separator);
            var trueString = strings[0];
            var falseString = strings[1];

            var boolValue = (bool)value;
            return boolValue ? trueString : falseString;
        }
        /// <summary>
        /// COnvert the string back into a boolean value
        /// </summary>
        /// <param name="value">boolean to convert</param>
        /// <param name="targetType">type of target element</param>
        /// <param name="parameter">containing the 2 strings separated by the given char</param>
        /// <param name="culture">CultureIfo to use to display the texts</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            var strings = ((string)parameter).Split(Separator);
            var trueString = strings[0];
            var stringValue = (string)value;
            return stringValue == trueString;
        }
    }
}

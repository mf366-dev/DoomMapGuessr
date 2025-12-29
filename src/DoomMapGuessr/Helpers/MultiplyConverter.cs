using System;
using System.Globalization;

using Avalonia.Data.Converters;


namespace DoomMapGuessr.Helpers
{

	public class MultiplyConverter : IValueConverter
	{

		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
			value is double d && parameter is string param && Double.TryParse(param, out double multiplier)
				? d * multiplier
				: value;

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
			value is double d && parameter is string param && Double.TryParse(param, out double multiplier) && multiplier != 0
				? d / multiplier
				: value;

	}

}

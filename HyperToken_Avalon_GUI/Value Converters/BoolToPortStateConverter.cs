using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using Terminal.Interface.Enums;

namespace HyperToken_Avalon_GUI.Value_Converters
{
	public class BoolToPortStateConverter : ConverterMarkupExtension<BoolToPortStateConverter>
	{
		public override object Convert(object value, Type targetType, object parameter,
			CultureInfo culture)
		{
			var state = (PortState)value;
			if (state == PortState.Open)
				return true;
			return false;
		}

		public override object ConvertBack(object value, Type targetType, object parameter,
			CultureInfo culture)
		{
			var state = (bool)value;
			if (state)
				return PortState.Open;
			return PortState.Closed;

		}
	}
}
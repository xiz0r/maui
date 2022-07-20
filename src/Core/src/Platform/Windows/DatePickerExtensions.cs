﻿using System;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml.Controls;
using WBrush = Microsoft.UI.Xaml.Media.Brush;

namespace Microsoft.Maui.Platform
{
	public static class DatePickerExtensions
	{
		public static void UpdateDate(this CalendarDatePicker platformDatePicker, IDatePicker datePicker)
		{
			var date = datePicker.Date;
			platformDatePicker.UpdateDate(date);

			platformDatePicker.UpdateTextColor(datePicker);
		}

		public static void UpdateDate(this CalendarDatePicker platformDatePicker, DateTime dateTime)
		{
			platformDatePicker.Date = dateTime;
		}

		public static void UpdateMinimumDate(this CalendarDatePicker platformDatePicker, IDatePicker datePicker)
		{
			platformDatePicker.MinDate = datePicker.MinimumDate;
		}

		public static void UpdateMaximumDate(this CalendarDatePicker platformDatePicker, IDatePicker datePicker)
		{
			platformDatePicker.MaxDate = datePicker.MaximumDate;
		}

		public static void UpdateCharacterSpacing(this CalendarDatePicker platformDatePicker, IDatePicker datePicker)
		{
			platformDatePicker.CharacterSpacing = datePicker.CharacterSpacing.ToEm();
		}

		public static void UpdateFont(this CalendarDatePicker platformDatePicker, IDatePicker datePicker, IFontManager fontManager) =>
			platformDatePicker.UpdateFont(datePicker.Font, fontManager);

		public static void UpdateTextColor(this CalendarDatePicker platformDatePicker, IDatePicker datePicker)
		{
			Color textColor = datePicker.TextColor;

			WBrush? platformBrush = textColor?.ToPlatform();

			if (platformBrush == null)
			{
				platformDatePicker.Resources.RemoveKeys(TextColorResourceKeys);
				platformDatePicker.ClearValue(DatePicker.ForegroundProperty);
			}
			else
			{
				platformDatePicker.Resources.SetValueForAllKey(TextColorResourceKeys, platformBrush);
				platformDatePicker.Foreground = platformBrush;
			}
		}

		// ResourceKeys controlling the foreground color of the CalendarDatePicker.
		// https://docs.microsoft.com/en-us/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.controls.calendardatepicker?view=windows-app-sdk-1.1
		static readonly string[] TextColorResourceKeys =
		{
			"CalendarDatePickerTextForeground",
			"CalendarDatePickerTextForegroundDisabled",
			"CalendarDatePickerTextForegroundSelected"
		};
	}
}
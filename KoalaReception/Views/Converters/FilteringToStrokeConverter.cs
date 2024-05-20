using System.Globalization;


namespace KoalaReception.Views.Converters
{
    public class FilterToStrokeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool OnFiltered && OnFiltered)
            {
                return Color.FromRgb(230, 0, 230);
            }
            return Color.FromRgba(255, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

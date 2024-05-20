using KoalaReception.Models.DTO;
using System.Globalization;


namespace KoalaReception.Views.Converters
{
    public class TableStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TableStatusEnum status)
            {
                switch (status)
                {
                    case TableStatusEnum.Available:
                        return Color.FromRgb(0, 255, 0);
                    case TableStatusEnum.Reserved:
                        return Color.FromRgb(211, 211, 211);
                    case TableStatusEnum.Selected:
                        return Color.FromRgb(255, 255, 0);
                    default:
                        return Color.FromRgb(255, 0, 0);
                }
            }
            return Color.FromRgb(255, 255, 255);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Windows.Data;
using Microsoft.Web.LibraryManager.Contracts;
using Microsoft.Web.LibraryManager.Vsix.Resources;

namespace Microsoft.Web.LibraryManager.Vsix.UI
{
    internal class HintTextConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IProvider provider = value as IProvider;

            if (value == null || provider == null)
            {
                return string.Empty;
            }

            string id = provider.Id;

            if (string.Equals(id, "cdnjs"))
            {
                return Text.CdnjsLibraryIdHintText;
            }           
            else if (string.Equals(id, "filesystem"))
            {
                return Text.FileSystemLibraryIdHintText;
            }
                
            else
            {
                return string.Empty;
            }
                
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

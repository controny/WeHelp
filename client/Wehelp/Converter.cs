using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

/*
 * 把字符串type转化为type对应的图片
 * 把对应的头像转化为string
 * 
 * By 许琦
 */

namespace Wehelp
{
    class Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var content = "ms-appx:///Assets/ItemType/" + (String)value + ".png";
            return new BitmapImage(new Uri(content));
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    class Converter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new BitmapImage(new Uri((String)value));
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

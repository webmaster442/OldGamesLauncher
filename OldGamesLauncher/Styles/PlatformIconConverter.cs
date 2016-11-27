using AppLib.WPF.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace OldGamesLauncher.Styles
{
    class PlatformIconConverter : IValueConverter
    {
        static Color[] _colors;
        static Dictionary<string, ImageSource> _cache;

        static PlatformIconConverter()
        {
            _colors = new Color[]
            {
                Color.FromRgb(26, 188, 156),
                Color.FromRgb(22, 160, 133),
                Color.FromRgb(46, 204, 113),
                Color.FromRgb(39, 174, 96),
                Color.FromRgb(52, 152, 219),
                Color.FromRgb(41, 128, 185),
                Color.FromRgb(155, 89, 182),
                Color.FromRgb(142, 68, 173),
                Color.FromRgb(52, 73, 94),
                Color.FromRgb(44, 62, 80),
                Color.FromRgb(241, 196, 15),
                Color.FromRgb(243, 156, 18),
                Color.FromRgb(230, 126, 34),
                Color.FromRgb(211, 84, 0),
                Color.FromRgb(231, 76, 60),
                Color.FromRgb(192, 57, 43),
                Color.FromRgb(189, 195, 199),
                Color.FromRgb(149, 165, 166),
                Color.FromRgb(127, 140, 141)
            };
            _colors.OrderBy(i => i.R * i.G * i.B);
            _cache = new Dictionary<string, ImageSource>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            var size = 128;
            if (string.IsNullOrEmpty(str)) return null;

            string tbtext = "";
            var words = str.Split(' ');
            if (words.Length > 2)
                tbtext = string.Format("{0}{1}{2}", words[0][0], words[1][0], words[2][0]);
            else if (words.Length > 1)
                tbtext = string.Format("{0}{1}", words[0][0], words[1][0]);
            else
                tbtext = words[0].Length > 2 ? words[0].Substring(0, 3) : words[0].Substring(0, words[0].Length);

            if (_cache.ContainsKey(tbtext))
            {
                return _cache[tbtext];
            }
            else
            {
                Border b = new Border();
                b.Width = size;
                b.Height = size;
                TextBlock t = new TextBlock();
                t.FontWeight = FontWeights.Bold;
                Viewbox strecher = new Viewbox();
                strecher.Child = t;
                b.Child = strecher;
                t.Text = tbtext.ToUpper();
                var index = Math.Abs(tbtext.ToUpper().GetHashCode()) % _colors.Length;
                b.Background = new SolidColorBrush(_colors[index]);
                t.Foreground = new SolidColorBrush(Colors.White);
                var img = b.Render();
                _cache.Add(tbtext, img);
                return img;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

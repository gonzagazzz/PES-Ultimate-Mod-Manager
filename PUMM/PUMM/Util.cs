﻿using PUMM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PUMM
{
    class Util
    {
        /* Loads local image to show in XAML */
        public static ImageSource LoadThumbnail(string filepath)
        {
            var image = new BitmapImage();
            using (var stream = new FileStream(filepath, FileMode.Open))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }
            return image;
        }
    }
}

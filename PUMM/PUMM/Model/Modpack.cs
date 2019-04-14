using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PUMM.Model
{
    public class Modpack : INotifyPropertyChanged
    {
        private string name;
        private string filepath = @"C:\Users\GonzagaZZZ\Desktop\199376822.jpeg";

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("name");
                }
            }
        }

        public ImageSource thumbnail
        {
            get { return LoadThumbnail(filepath); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

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

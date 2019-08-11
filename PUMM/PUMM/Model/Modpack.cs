using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PUMM.Model
{
    public class Modpack : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string filepath;

        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChanged("name");
                }
            }
        }

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

        public string ImagePath
        {
            get { return filepath; }
            set
            {
                if (filepath != value)
                {
                    filepath = value;
                    RaisePropertyChanged("filepath");
                    RaisePropertyChanged("Thumbnail");
                }
            }
        }

        public ImageSource Thumbnail
        {
            get { return Util.LoadThumbnail(filepath); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}

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
        private int version;
        private int mods;

        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChanged("id");
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

        public int Version
        {
            get { return version; }
            set
            {
                if (version != value)
                {
                    version = value;
                    RaisePropertyChanged("version");
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

        public int Mods
        {
            get { return mods; }
            set
            {
                if (mods != value)
                {
                    mods = value;
                    RaisePropertyChanged("mods");
                }
            }
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

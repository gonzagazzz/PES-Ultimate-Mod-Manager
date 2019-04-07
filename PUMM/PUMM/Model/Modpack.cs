using System.ComponentModel;

namespace PUMM.Model
{
    public class Modpack : INotifyPropertyChanged
    {
        private string name;

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

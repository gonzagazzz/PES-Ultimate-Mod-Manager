using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PUMM.Model
{
    class Mod : INotifyPropertyChanged
    {

        private string filename;
        private bool selected;

        public string Filename
        {
            get { return filename; }
            set
            {
                if (filename != value)
                {
                    filename = value;
                    RaisePropertyChanged("filename");
                }
            }
        }

        public bool Selected
        {
            get { return selected; }
            set
            {
                if (selected != value)
                {
                    selected = value;
                    RaisePropertyChanged("selected");
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

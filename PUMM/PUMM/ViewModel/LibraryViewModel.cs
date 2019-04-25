using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PUMM.Model;

namespace PUMM.ViewModel
{
    class LibraryViewModel : BindableBase
    {
        DbProvider db;

        public LibraryViewModel(DbProvider db)
        {
            this.db = db;

            Modpacks = db.retrieveModpacks();

            Print = new MyICommand<string>(PrintForDebug);
            UpdateItems = new MyICommand<string>(PanelLoaded);
        }

        public MyICommand<string> Print { get; set; }
        public MyICommand<string> UpdateItems { get; set; }

        private double _ItemWidth;
        public double ItemWidth {
            get { return _ItemWidth; }
            set { SetProperty<double>(ref _ItemWidth, value); }
        }

        public double PanelWidth { get; set; }

        public ObservableCollection<Modpack> Modpacks
        {
            get;
            set;
        }

        private void PrintForDebug(string s)
        {
            MessageBox.Show(System.AppDomain.CurrentDomain.BaseDirectory);
        }

        private void PanelLoaded(string s)
        {
            if(PanelWidth < 900)
            {
                ItemWidth = (PanelWidth - 14 * 3 - SystemParameters.VerticalScrollBarWidth) / 3;
            } else
            {
                ItemWidth = (PanelWidth - 14 * 4 - SystemParameters.VerticalScrollBarWidth) / 4;
            }
        }

    }
}

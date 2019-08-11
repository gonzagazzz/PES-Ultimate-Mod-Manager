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
using PUMM.ViewModel;

namespace PUMM.ViewModel
{
    class LibraryViewModel : BindableBase
    {
        private DbProvider db;
        private ObservableCollection<Modpack> modpacks;
        private MainWindowViewModel main;

        public LibraryViewModel(DbProvider db, MainWindowViewModel main)
        {
            this.db = db;
            Modpacks = db.retrieveModpacks();
            this.main = main;

            //Print = new MyICommand<string>(printForDebug);
            AdjustItems = new MyICommand<string>(adjustItemsWidth);
            SetAsActive = new MyICommand<Modpack>(setActive);
            DeleteModpack = new MyICommand<Modpack>(deleteModpack);
        }

        private double itemWidth;
        public double ItemWidth {
            get { return itemWidth; }
            set { SetProperty<double>(ref itemWidth, value); }
        }

        public double PanelWidth { get; set; }

        public ObservableCollection<Modpack> Modpacks
        {
            get { return modpacks; }
            set {  SetProperty(ref modpacks, value); }
        }

        //public MyICommand<string> Print { get; set; }
        public MyICommand<string> AdjustItems { get; set; }
        public MyICommand<Modpack> SetAsActive { get; set; }
        public MyICommand<Modpack> DeleteModpack { get; set; }

        /*
        private void printForDebug(string s)
        {
            MessageBox.Show(System.AppDomain.CurrentDomain.BaseDirectory);
        }
        */

        private void adjustItemsWidth(string s)
        {
            if(PanelWidth < 900)
                ItemWidth = (PanelWidth - 14 * 3 - SystemParameters.VerticalScrollBarWidth) / 3;
            else
                ItemWidth = (PanelWidth - 14 * 4 - SystemParameters.VerticalScrollBarWidth) / 4;
        }

        private void setActive(Modpack modpack)
        {
            main.Active = modpack;
        }

        private void deleteModpack(Modpack modpack)
        {
            Modpacks.Remove(modpack);
            db.deleteModpack(modpack.Id);
        }

    }
}

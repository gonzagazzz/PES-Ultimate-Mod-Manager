using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PUMM.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace PUMM.ViewModel
{
    class ModsViewModel : BindableBase
    {
        private DbProvider db;
        private ObservableCollection<Mod> mods;
        private MainWindowViewModel main;

        public ModsViewModel(DbProvider db, MainWindowViewModel main)
        {
            this.db = db;
            mods = new ObservableCollection<Mod>();
            this.main = main;

            GenerateBinary = new MyICommand<string>(generateDpFileList);
            SaveModpack = new MyICommand<string>(saveModpack);

            /* Creates every mod inside download folder */
            string[] files = DpFileListUtil.getCpks(@"E:\Games\Pro Evolution Soccer 2019\download");
            foreach(string filename in files)
            {
                Mods.Add(new Mod { Filename = filename, Selected = db.modpackHasMod(main.Active, filename) });
            }

        }

        public ObservableCollection<Mod> Mods
        {
            get{ return mods; }
            set { SetProperty(ref mods, value); }
        }

        public MyICommand<string> GenerateBinary { get; private set; }
        public MyICommand<string> SaveModpack { get; private set; }

        private void generateDpFileList(string s)
        {

        }

        private void saveModpack(string s)
        {
            if(db.addModsToModpack(main.Active, Mods))
                MessageBox.Show(main.Active.Name + " mods saved successfully", "Mods saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}

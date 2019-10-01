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
        private ObservableCollection<string> checkedMods;
        private List<string> remainingMods;

        public ModsViewModel(DbProvider db, MainWindowViewModel main)
        {
            this.db = db;
            mods = new ObservableCollection<Mod>();
            this.main = main;

            // Stores mods by order of selection
            checkedMods = new ObservableCollection<string>();
            remainingMods = new List<string>();

            GenerateBinary = new MyICommand<string>(generateDpFileList);
            SaveModpack = new MyICommand<string>(saveModpack);
            SelectionChanged = new MyICommand<Mod>(selectionChanged);

            if (main.DownloadPath != "Browse PES download folder...")
            {
                /* Creates every mod inside download folder */
                string[] files = DpFileListUtil.getCpks(main.DownloadPath);
                foreach (string filename in files)
                {
                    bool check = db.modpackHasMod(main.Active, filename);
                    // Adds CPK to mods' ListView
                    Mods.Add(new Mod { Filename = filename, Selected = check });
                    // If there is an active modpack, gets list of mods that are not checked
                    if (main.Active != null && !check)
                        remainingMods.Add(Mods.Last().Filename);
                }
                // If there is an active modpack, gets sorted list of mods
                if(main.Active != null)
                    CheckedMods = new ObservableCollection<string>(main.Active.Mods);

            }
        }

        public ObservableCollection<Mod> Mods
        {
            get{ return mods; }
            set { SetProperty(ref mods, value); }
        }

        public ObservableCollection<string> CheckedMods
        {
            get { return checkedMods; }
            set { SetProperty(ref checkedMods, value); }
        }

        public MyICommand<string> GenerateBinary { get; private set; }
        public MyICommand<string> SaveModpack { get; private set; }
        public MyICommand<Mod> SelectionChanged { get; private set; }

        private void generateDpFileList(string s)
        {
            if (main.DownloadPath == "Browse PES download folder...")
            {
                string[] error = Messages.error("PathNotSet", new string[] { main.Version.ToString() });
                MessageBox.Show(error[0], error[1], MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (main.Active == null) // Generate from mods list
            {
                if(CheckedMods.Count == 0)
                {
                    string[] error = Messages.error("EmptyModsList", null);
                    MessageBox.Show(error[0], error[1], MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DpFileListUtil.generate(DpFileListUtil.arrangeDLC(CheckedMods.ToList()), main.DownloadPath + @"\DpFileList.bin");
                string[] success = Messages.success("GeneratedFromModsList", null);
                MessageBox.Show(success[0], success[1], MessageBoxButton.OK, MessageBoxImage.Information);
            } else // Generate from active Modpack
            {
                DpFileListUtil.generate(main.Active.Mods, main.DownloadPath + @"\DpFileList.bin");
                string[] success = Messages.success("GeneratedFromModpack", new string[] { main.Active.Name });
                MessageBox.Show(success[0], success[1], MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void saveModpack(string s)
        {
            if (main.Active == null)
            {
                string[] error = Messages.error("NoActiveModpack", null);
                MessageBox.Show(error[0], error[1], MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            db.addModsToModpack(main.Active, checkedMods, remainingMods);
            // Updates active modpack list of mods and sorts it
            main.Active.Mods = db.getMods(main.Active);

            string[] success = Messages.success("ModsSaved", new string[] { main.Active.Name });
            MessageBox.Show(success[0], success[1], MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /* Sets order of mods (to generate by order of selection) */
        public void selectionChanged(Mod mod)
        {
            if (mod.Selected)
            {
                checkedMods.Add(mod.Filename);
                remainingMods.Remove(mod.Filename);
                CheckedMods = new ObservableCollection<string>(DpFileListUtil.arrangeDLC(CheckedMods.ToList()));
            } else {
                remainingMods.Add(mod.Filename);
                checkedMods.Remove(mod.Filename);
            }
        }

    }
}

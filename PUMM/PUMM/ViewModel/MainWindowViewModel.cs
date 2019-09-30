using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PUMM.Model;
using Microsoft.Win32;
using System.Windows;
using System.Collections.ObjectModel;

namespace PUMM.ViewModel
{
    class MainWindowViewModel : BindableBase
    {
        private string title;
        private HomeViewModel home;
        private DbProvider db;
        private Modpack active;
        private int version;
        private string download;
        private string potentialName;

        public MainWindowViewModel()
        {
            db = new DbProvider();
            // Loads last PES version used
            string v = db.getSetting("pes_version");
            Version = String.IsNullOrEmpty(v) ? 2019 : Int32.Parse(v);

            // Sets title when window load
            Title = "PES Ultimate Mod Manager - PES " + Version;

            /* Initializes needed ViewModels */
            home = new HomeViewModel(db, this);

            // Gets current PES version download path and updates Home page hyperlink
            string download = db.getSetting("pes" + version + "_path");
            if (String.IsNullOrEmpty(download))
                home.DownloadPath = "Browse PES download folder...";
            else
                home.DownloadPath = download;

            CurrentViewModel = home;
            NavCommand = new MyICommand<string>(OnNav);
            BrowseThumbnail = new MyICommand<string>(updateThumbnail);
            SetName = new MyICommand<string>(updateName);
            SaveSession = new MyICommand<string>(onExit);
            ClearActive = new MyICommand<string>(clearActiveModpack);
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private BindableBase currentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }

        public Modpack Active
        {
            get { return active; }
            set { SetProperty(ref active, value); }
        }

        public int Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }

        public string DownloadPath
        {
            get { return download; }
            set { SetProperty(ref download, value); }
        }

        public string PotentialName
        {
            get { return potentialName; }
            set { SetProperty(ref potentialName, value); }
        }

        public MyICommand<string> NavCommand { get; private set; }
        public MyICommand<string> BrowseThumbnail { get; private set; }
        public MyICommand<string> SetName { get; private set; }
        public MyICommand<string> SaveSession { get; private set; }
        public MyICommand<string> ClearActive { get; private set; }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "home":
                    CurrentViewModel = home;
                    break;
                case "library":
                    // Fixes reloading Library hides every modpacks
                    if (CurrentViewModel.GetType() != typeof(LibraryViewModel))
                        CurrentViewModel = new LibraryViewModel(db, this);
                    break;
                case "new_modpack":
                    CurrentViewModel = new NewModpackViewModel(db, this);
                    break;
                case "mods":
                    CurrentViewModel = new ModsViewModel(db, this);
                    break;
                default:
                    break;
            }
        }

        private void updateThumbnail(string s)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select modpack's thumbnail...";
            dialog.Filter = "Image Files|*.png; *.jpg; *.jpeg; *.bmp";
            if (dialog.ShowDialog() == true)
            {
                // Updates active modpack
                Active.ImagePath = dialog.FileName;
                // Updates library's modpack thumbnail (Fix changing pages with active modpack only updates active modpack's thumbnail)
                if (CurrentViewModel.GetType() == typeof(LibraryViewModel))
                {
                    LibraryViewModel current = (LibraryViewModel)CurrentViewModel;
                    foreach(Modpack modpack in current.Modpacks)
                    {
                        if (modpack.Id == Active.Id)
                            modpack.ImagePath = Active.ImagePath;
                    }
                }
                // Updates database
                db.updateModpack(Active.Id, "thumbnail", dialog.FileName);
            }
        }

        private void updateName(string s)
        {
            Modpack modpack = db.getModpack(PotentialName);
            if (modpack == null) // Potential name doesn't already exist
            {
                // Updates active modpack
                Active.Name = PotentialName;
                // Updates library's modpack name (Fix changing pages with active modpack only updates active modpack's name)
                if (CurrentViewModel.GetType() == typeof(LibraryViewModel))
                {
                    LibraryViewModel current = (LibraryViewModel)CurrentViewModel;
                    foreach (Modpack modpck in current.Modpacks)
                    {
                        if (modpck.Id == Active.Id)
                            modpck.Name = Active.Name;
                    }
                }
                // Update database (Active modpack's name is set on textbox text change)
                db.updateModpack(Active.Id, "name", PotentialName);
            } else if (modpack.Id != Active.Id) // Potential name equals name of another modpack
            {
                PotentialName = Active.Name;
                string[] error = Messages.error("ModpackAlreadyExists", new string[] { PotentialName });
                MessageBox.Show(error[0], error[1], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void onExit(string s)
        {
            // Save last PES version used
            db.newSetting("pes_version", Version.ToString());
        }

        private void clearActiveModpack(string s)
        {
            Active = null;
            // Resets textbox with modpack's name to hide Close button
            PotentialName = string.Empty;

            // Clear Mods page if is where user is
            if (CurrentViewModel.GetType() == typeof(ModsViewModel))
            {
                ModsViewModel current = (ModsViewModel)CurrentViewModel;

                foreach (Mod mod in current.Mods)
                    mod.Selected = false;

                current.CheckedMods = new ObservableCollection<string>();
            }
        }
    }
}

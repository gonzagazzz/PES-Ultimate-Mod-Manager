using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PUMM.Model;
using Microsoft.Win32;
using System.Windows;

namespace PUMM.ViewModel
{
    class MainWindowViewModel : BindableBase
    {
        private HomeViewModel home;
        private DbProvider db;
        private Modpack active;
        private int version;
        string download;

        public MainWindowViewModel()
        {
            db = new DbProvider();
            version = 2019; // MUST CHANGE TO LAST SESSION VERSION

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
            set {
                SetProperty(ref download, value);
            }
        }

        public MyICommand<string> NavCommand { get; private set; }
        public MyICommand<string> BrowseThumbnail { get; private set; }
        public MyICommand<string> SetName { get; private set; }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "home":
                    CurrentViewModel = home;
                    break;
                case "library":
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
                // Updates database
                db.updateModpack(Active.Id, "thumbnail", dialog.FileName);
            }
        }

        private void updateName(string s)
        {
            // Update database (Active modpack's name is set on textbox text change)
            db.updateModpack(Active.Id, "name", Active.Name);
        }
        
    }
}

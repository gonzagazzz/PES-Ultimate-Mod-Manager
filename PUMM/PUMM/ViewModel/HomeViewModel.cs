using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUMM.ViewModel
{
    class HomeViewModel : BindableBase
    {

        private MainWindowViewModel main;
        private DbProvider db;
        private string download = "Browse PES download folder...";
        private bool[] versionSelection;

        public HomeViewModel(DbProvider db, MainWindowViewModel main)
        {
            this.db = db;
            this.main = main;
            versionSelection = new bool[2];

            // Updates checked and unchecked PES versions
            updateCheckedVersions();

            SetVersion = new MyICommand<string>(updateVersion);
            BrowseDownload = new MyICommand<string>(updateDownloadPath);
        }

        public string DownloadPath
        {
            get { return download; }
            set {
                SetProperty(ref download, value);
                main.DownloadPath = download;
            }
        }

        public bool[] VersionSelection
        {
            get { return versionSelection; }
        }

        public MyICommand<string> SetVersion { get; private set; }
        public MyICommand<string> BrowseDownload { get; private set; }

        private void updateVersion(string version)
        {
            main.Version = Int32.Parse(version);
            // Updates checked and unchecked PES versions
            updateCheckedVersions();

            // Updates current version download folder
            string download = db.getSetting("pes" + main.Version + "_path");
            if (String.IsNullOrEmpty(download))
                DownloadPath = "Browse PES download folder...";
            else
                DownloadPath = download;
        }

        private void updateDownloadPath(string s)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                DownloadPath = dialog.SelectedPath;
                db.newSetting("pes" + main.Version + "_path", dialog.SelectedPath);
            }
        }

        private void updateCheckedVersions()
        {
            switch (main.Version)
            {
                case 2019:
                    VersionSelection[0] = true;
                    VersionSelection[1] = false;
                    break;
                case 2020:
                    VersionSelection[0] = false;
                    VersionSelection[1] = true;
                    break;
            }
        }
    }
}

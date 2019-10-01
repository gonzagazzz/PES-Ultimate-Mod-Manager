using PUMM.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            GenerateBinary = new MyICommand<string>(generateDpFileList);
            OpenExplorer = new MyICommand<string>(openInExplorer);
            RemovePath = new MyICommand<string>(removePath);
            OpenBrowser = new MyICommand<string>(openBrowser);
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
        public MyICommand<string> GenerateBinary { get; private set; }
        public MyICommand<string> OpenExplorer { get; private set; }
        public MyICommand<string> RemovePath { get; private set; }
        public MyICommand<string> OpenBrowser { get; private set; }

        private void updateVersion(string version)
        {
            main.Version = Int32.Parse(version);
            // Sets title accordingly
            main.Title = "PES Ultimate Mod Manager - PES " + version;
            // Resets active modpack
            main.Active = null;
            // Resets textbox with modpack's name to hide Close button
            main.PotentialName = string.Empty;
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

        private void generateDpFileList(string s)
        {
            if(DownloadPath == "Browse PES download folder...")
            {
                string[] error = Messages.error("PathNotSet", new string[] { main.Version.ToString() });
                MessageBox.Show(error[0], error[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(main.Active == null)
            {
                string[] error = Messages.error("NoActiveModpack", null);
                MessageBox.Show(error[0], error[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(main.Active.Mods.Count == 0)
            {
                string[] error = Messages.error("EmptyModpack", new string[] { main.Active.Name });
                MessageBox.Show(error[0], error[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DpFileListUtil.generate(main.Active.Mods, DownloadPath + @"\DpFileList.bin");
            string[] success = Messages.success("GeneratedFromModpack", new string[] { main.Active.Name });
            MessageBox.Show(success[0], success[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void openInExplorer(string s)
        {
            Process.Start(db.getSetting("pes" + main.Version + "_path"));
        }

        private void removePath(string s)
        {
            db.removeSetting("pes" + main.Version + "_path");
            DownloadPath = "Browse PES download folder...";
        }

        private void openBrowser(string s)
        {
            Process.Start("http://www.google.pt");
        }
    }
}

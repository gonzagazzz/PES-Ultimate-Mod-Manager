using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            Modpacks = db.retrieveModpacks(main.Version);
            this.main = main;
            
            AdjustItems = new MyICommand<string>(adjustItemsWidth);
            SetAsActive = new MyICommand<Modpack>(setActive);
            DeleteModpack = new MyICommand<Modpack>(deleteModpack);
            Import = new MyICommand<string>(importModpack);
            Export = new MyICommand<string>(exportModpack);
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
        
        public MyICommand<string> AdjustItems { get; set; }
        public MyICommand<Modpack> SetAsActive { get; set; }
        public MyICommand<Modpack> DeleteModpack { get; set; }
        public MyICommand<string> Import { get; private set; }
        public MyICommand<string> Export { get; private set; }

        private void adjustItemsWidth(string s)
        {
            if(PanelWidth < 900)
                ItemWidth = (PanelWidth - 14 * 3 - System.Windows.SystemParameters.VerticalScrollBarWidth) / 3;
            else
                ItemWidth = (PanelWidth - 14 * 4 - System.Windows.SystemParameters.VerticalScrollBarWidth) / 4;
        }

        private void setActive(Modpack modpack)
        {
            // Updates current active modpack and its mods
            main.Active = modpack;
            // Updates active modpack list of mods and sorts it
            main.Active.Mods = db.getMods(main.Active);
            // Updates name for editing
            main.PotentialName = modpack.Name;
        }

        private void deleteModpack(Modpack modpack)
        {
            string[] question = Messages.question("DeleteModpackConfirmation", new string[] { modpack.Name });
            DialogResult dialogResult = MessageBox.Show(question[0], question[1], MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                if (main.Active != null && main.Active.Id == modpack.Id)
                {
                    main.Active = null;
                    main.PotentialName = string.Empty;
                }
                
                Modpacks.Remove(modpack);
                db.deleteModpack(modpack.Id);
            }
        }

        private void importModpack(string s)
        {
            Modpack modpack = ExcelProvider.import(db);
            if (modpack != null)
            {
                Modpacks.Add(modpack);
                string[] success = Messages.success("ModpackImported", new string[] { modpack.Name });
                MessageBox.Show(success[0], success[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void exportModpack(string s)
        {
            if (main.Active == null)
            {
                string[] error = Messages.error("NoActiveModpack", null);
                MessageBox.Show(error[0], error[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ExcelProvider.export(main.Active);
            string[] success = Messages.success("ModpackExported", new string[] { main.Active.Name });
            MessageBox.Show(success[0], success[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}

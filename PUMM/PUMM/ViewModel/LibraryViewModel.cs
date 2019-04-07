using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PUMM.Model;

namespace PUMM.ViewModel
{
    class LibraryViewModel : BindableBase
    {
        public LibraryViewModel()
        {
            LoadModpacks();
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

        public void LoadModpacks()
        {
            ObservableCollection<Modpack> modpacks = new ObservableCollection<Modpack>();
            
            modpacks.Add(new Modpack { Name = "Real Madrid Full Mods 2019" });
            modpacks.Add(new Modpack { Name = "Super Dragões Pack" });
            modpacks.Add(new Modpack { Name = "SLB Loladas" }); modpacks.Add(new Modpack { Name = "Real Madrid Full Mods 2019" });
            modpacks.Add(new Modpack { Name = "Super Dragões Pack" });
            modpacks.Add(new Modpack { Name = "SLB Loladas" }); modpacks.Add(new Modpack { Name = "Real Madrid Full Mods 2019" });
            modpacks.Add(new Modpack { Name = "Super Dragões Pack" });
            modpacks.Add(new Modpack { Name = "SLB Loladas" }); modpacks.Add(new Modpack { Name = "Real Madrid Full Mods 2019" });
            modpacks.Add(new Modpack { Name = "Super Dragões Pack" });
            modpacks.Add(new Modpack { Name = "SLB Loladas" }); modpacks.Add(new Modpack { Name = "Real Madrid Full Mods 2019" });
            modpacks.Add(new Modpack { Name = "Super Dragões Pack" });
            modpacks.Add(new Modpack { Name = "SLB Loladas" }); modpacks.Add(new Modpack { Name = "Real Madrid Full Mods 2019" });
            modpacks.Add(new Modpack { Name = "Super Dragões Pack" });
            modpacks.Add(new Modpack { Name = "SLB Loladas" }); modpacks.Add(new Modpack { Name = "Real Madrid Full Mods 2019" });
            modpacks.Add(new Modpack { Name = "Super Dragões Pack" });
            modpacks.Add(new Modpack { Name = "SLB Loladas" }); modpacks.Add(new Modpack { Name = "Real Madrid Full Mods 2019" });
            modpacks.Add(new Modpack { Name = "Super Dragões Pack" });
            modpacks.Add(new Modpack { Name = "SLB Loladas" });

            Modpacks = modpacks;
            
        }

        private void PrintForDebug(string s)
        {
            MessageBox.Show(System.AppDomain.CurrentDomain.BaseDirectory);
            //MessageBox.Show("[Debug] Panel Width: " + PanelWidth);
            //MessageBox.Show("[Debug] Item Width: " + ItemWidth);
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

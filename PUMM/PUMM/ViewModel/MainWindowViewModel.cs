using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PUMM.ViewModel
{
    class MainWindowViewModel : BindableBase
    {

        private HomeViewModel home;
        private LibraryViewModel library;

        public MainWindowViewModel()
        {
            if(!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "registry.sqlite"))
            {
                
            }
            home = new HomeViewModel();
            library = new LibraryViewModel();

            CurrentViewModel = home;
            NavCommand = new MyICommand<string>(OnNav);
        }

        private BindableBase _CurrentViewModel;

        public BindableBase CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value); }
        }

        public MyICommand<string> NavCommand { get; private set; }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "home":
                    CurrentViewModel = home;
                    break;
                case "library":
                    CurrentViewModel = library;
                    break;
                default:
                    break;
            }
        }
    }
}

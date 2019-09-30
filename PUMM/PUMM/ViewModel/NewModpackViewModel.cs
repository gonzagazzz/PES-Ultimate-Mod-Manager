using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PUMM.ViewModel
{
    class NewModpackViewModel : BindableBase
    {
        private DbProvider db;
        private string name;
        private ImageSource thumbnail;
        private MainWindowViewModel main;

        public NewModpackViewModel(DbProvider db, MainWindowViewModel main)
        {
            this.db = db;
            this.main = main;
            Thumbnail = new BitmapImage(new Uri("pack://application:,,,/PUMM;component/Resources/select_thumbnail.png", UriKind.Absolute));

            AddModpack = new MyICommand<string>(addModpack);
            BrowseThumbnail = new MyICommand<string>(selectThumbnail);
        }

        public string Name {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public ImageSource Thumbnail
        {
            get { return thumbnail; }
            set { SetProperty(ref thumbnail, value); }
        }

        public MyICommand<string> AddModpack { get; set; }
        public MyICommand<string> BrowseThumbnail { get; set; }

        private string thumbnailPath; // original thumbnail's path
        private string thumbnailName; // thumbnail's name with extension
        public void selectThumbnail(string s)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select modpack's thumbnail...";
            dialog.Filter = "Image Files|*.png; *.jpg; *.jpeg; *.bmp";
            if(dialog.ShowDialog() == true)
            {
                thumbnailPath = dialog.FileName;
                thumbnailName = dialog.SafeFileName;
                Thumbnail = Util.LoadThumbnail(dialog.FileName);
            }
        }

        public void addModpack(string s)
        {
            if(String.IsNullOrEmpty(Name))
            {
                string[] error = Messages.error("EmptyModpackName", null);
                MessageBox.Show(error[0], error[1], MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrEmpty(thumbnailPath))
            {
                string[] error = Messages.error("EmptyModpackThumbnail", null);
                MessageBox.Show(error[0], error[1], MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (db.getModpack(Name) != null)
            {
                string[] error = Messages.error("ModpackAlreadyExists", new string[] { Name });
                MessageBox.Show(error[0], error[1], MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // creates folder to store modpacks' thumbnails
            FileInfo path = new FileInfo(System.AppDomain.CurrentDomain.BaseDirectory + @"thumbnails\");
            path.Directory.Create();
            // creates a copy of the selected image in 'thumbnails' folder
            File.Copy(thumbnailPath, path.Directory + @"\" + thumbnailName, true);
            // creates modpack entry in database
            int id = db.addModpack(Name, main.Version, path.Directory + @"\" + thumbnailName);

            string[] success = Messages.success("ModpackAdded", new string[] { Name });
            MessageBox.Show(success[0], success[1], MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}

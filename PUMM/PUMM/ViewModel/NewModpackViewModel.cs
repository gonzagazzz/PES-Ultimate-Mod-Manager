using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PUMM.ViewModel
{
    class NewModpackViewModel : BindableBase
    {
        private DbProvider db;
        private string name;
        private ImageSource thumbnail;

        public NewModpackViewModel(DbProvider db)
        {
            this.db = db;
            Thumbnail = Util.LoadThumbnail(@"C:\Users\GonzagaZZZ\Desktop\modpack example\199376822.jpeg"); // default modpack thumbnail

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
            if(!String.IsNullOrEmpty(thumbnailPath))
            {
                // creates folder to store modpacks' thumbnails
                FileInfo path = new FileInfo(System.AppDomain.CurrentDomain.BaseDirectory + @"thumbnails\");
                path.Directory.Create();
                // creates a copy of the selected image in 'thumbnails' folder
                File.Copy(thumbnailPath, path.Directory + @"\" + thumbnailName, true);
                // creates modpack entry in database
                db.addModpack(Name, path.Directory + @"\" + thumbnailName);
                MessageBox.Show(Name + " successfully added", "Modpack Added");
            }
        }

    }
}

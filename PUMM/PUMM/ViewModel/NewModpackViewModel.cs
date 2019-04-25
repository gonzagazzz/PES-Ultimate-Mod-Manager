using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PUMM.ViewModel
{
    class NewModpackViewModel : BindableBase
    {
        DbProvider db;

        public NewModpackViewModel(DbProvider db)
        {
            this.db = db;

            AddModpack = new MyICommand<string>(addModpack);
            SelectThumbnail = new MyICommand<string>(selectThumbnail);
        }

        public MyICommand<string> AddModpack { get; set; }
        public MyICommand<string> SelectThumbnail { get; set; }

        public string Name { get; set; }

        private string thumbnailPath, thumbnailName;
        public void selectThumbnail(string s)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == true)
            {
                thumbnailPath = dialog.FileName;
                thumbnailName = dialog.SafeFileName;
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

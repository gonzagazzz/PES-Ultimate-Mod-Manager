using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IronXL;
using System.Data;
using PUMM.Model;
using System.Windows.Forms;

namespace PUMM
{
    class ExcelProvider
    {

        public static void export(Modpack modpack)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XLSX File|*.xlsx";
            dialog.FileName = modpack.Name;
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                // Creates Excel file
                WorkBook registry = WorkBook.Create(ExcelFileFormat.XLSX);
                registry.Metadata.Author = "PUMM";
                WorkSheet data = registry.CreateWorkSheet("modpack");

                data["A1"].Value = modpack.Name;
                data["B1"].Value = modpack.Version;
                data["C1"].Value = modpack.ImagePath;

                for(int i=0; i<modpack.Mods.Count; i++)
                    data["A" + (2 + i)].Value = modpack.Mods[i];

                registry.SaveAs(dialog.FileName);
            }
        }

        public static Modpack import(DbProvider db)
        {
            Modpack modpack = null;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XLSX File|*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                WorkBook registry = WorkBook.Load(dialog.FileName);
                WorkSheet data = registry.WorkSheets.First();

                // Adds modpack to database
                db.addModpack(data["A1"].Value.ToString(), Int32.Parse(data["B1"].Value.ToString()), data["C1"].Value.ToString());
                modpack = db.getModpack(data["A1"].Value.ToString());

                // Inserts mods in modpack
                ObservableCollection<string> mods = new ObservableCollection<string>();
                foreach (var cell in data["A2:A" + data.Rows.Count])
                {
                    mods.Add(cell.Text);
                }
                db.addModsToModpack(modpack, mods, null);
            }

            return modpack;
        }
    }
}

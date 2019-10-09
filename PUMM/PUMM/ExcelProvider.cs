using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using PUMM.Model;
using System.Windows.Forms;
using ExcelLibrary.SpreadSheet;

namespace PUMM
{
    class ExcelProvider
    {
        public static bool export(Modpack modpack)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XLS File|*.xls";
            dialog.FileName = modpack.Name;
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                // Export XLS with database information
                Workbook registry = new Workbook();
                Worksheet data = new Worksheet("Modpack");

                data.Cells[0, 0] = new Cell(modpack.Name);
                data.Cells[0, 1] = new Cell(modpack.Version);
                data.Cells[0, 2] = new Cell(modpack.ImagePath);

                for (int i = 0; i < modpack.Mods.Count; i++)
                    data.Cells[(i + 1), 0] = new Cell(modpack.Mods[i]);

                registry.Worksheets.Add(data);
                registry.Save(dialog.FileName);
                return true;
            }
            return false;
        }

        public static Modpack import(DbProvider db)
        {
            Modpack modpack = null;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XLS File|*.xls";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Workbook registry = Workbook.Load(dialog.FileName);
                Worksheet data = registry.Worksheets.First();

                // This should be decoupled from here
                if (db.getModpack(data.Cells[0, 0].Value.ToString()) != null)
                {
                    string[] error = Messages.error("ModpackAlreadyExists", new string[] { data.Cells[0, 0].Value.ToString() });
                    MessageBox.Show(error[0], error[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                // Adds modpack to database
                db.addModpack(data.Cells[0, 0].Value.ToString(), Int32.Parse(data.Cells[0, 1].Value.ToString()), data.Cells[0, 2].Value.ToString());
                modpack = db.getModpack(data.Cells[0, 0].Value.ToString());

                // Inserts mods in modpack
                ObservableCollection<string> mods = new ObservableCollection<string>();
                for(int i=1; i<=data.Cells.LastRowIndex; i++)
                {
                    mods.Add(data.Cells[i, 0].StringValue);
                }
                db.addModsToModpack(modpack, mods, null);
                
            }

            return modpack;
        }

        /* IronXL code
        public static bool export(Modpack modpack)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XLSX File|*.xlsx";
            dialog.FileName = modpack.Name;
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                WorkBook registry = WorkBook.Create(ExcelFileFormat.XLSX);
                registry.Metadata.Author = "PUMM";
                WorkSheet data = registry.CreateWorkSheet("modpack");

                data["A1"].Value = modpack.Name;
                data["B1"].Value = modpack.Version;
                data["C1"].Value = modpack.ImagePath;

                for (int i = 0; i < modpack.Mods.Count; i++)
                    data["A" + (2 + i)].Value = modpack.Mods[i];

                registry.SaveAs(dialog.FileName);
                return true;
            }
            return false;
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

                // This should be decoupled from here
                if (db.getModpack(data["A1"].Value.ToString()) != null)
                {
                    string[] error = Messages.error("ModpackAlreadyExists", new string[] { data["A1"].Value.ToString() });
                    MessageBox.Show(error[0], error[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

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
        */
    }
}

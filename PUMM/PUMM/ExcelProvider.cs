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

namespace PUMM
{
    class ExcelProvider
    {

        WorkBook registry;

        public ExcelProvider()
        {
            if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "pumm.xlsx"))
            {
                registry = WorkBook.Create(ExcelFileFormat.XLSX);
                registry.Metadata.Author = "PUMM";
                registry.CreateWorkSheet("mods");
                registry.SaveAs("pumm.xlsx");
            } else
            {
                registry = WorkBook.Load("pumm.xlsx");
            }
        }

        public void addModsToModpack(Modpack modpack, ObservableCollection<Mod> mods)
        {
            foreach(Mod mod in mods)
            {

                if(mod.Selected)
                {
                    if(getMod(modpack.Name) == null)
                    {

                    }
                } else
                {
                    if(getMod(modpack.Name) != null)
                    {

                    }
                }
            }
        }

        public string[] getMod(string name)
        {
            WorkSheet mods = registry.WorkSheets.First();
            int rows = 0;
            foreach (var cell in mods["B1:B" + mods.Rows.Count])
            {
                rows++;   
                if(cell.Text == name)
                    return new string[] { (string)mods["B" + rows].Value, (string)mods["C" + rows].Value, (string)mods["D" + rows].Value };
            }
            return null;
        }
    }
}

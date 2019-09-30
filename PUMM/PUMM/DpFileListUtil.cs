using PUMM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace PUMM
{
    class DpFileListUtil
    {
        /* Retrieves all CPK files names inside download folder */
        public static string[] getCpks(string downloadPath)
        {
            return Directory.EnumerateFiles(downloadPath, "*.*")
                 .Select(p => Path.GetFileName(p))
                 .Where(s => s.EndsWith(".cpk", StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        /* Sorts array of strings to keep DLCs always first (and in order) */
        public static List<string> arrangeDLC(List<string> mods)
        {
            List<string> dlcs = new List<string>();
            List<string> remaining = new List<string>();

            Regex regex = new Regex("dt80_[0-9][0-9][0-9]E_x64.cpk");
            foreach(string mod in mods)
            {
                if (regex.Matches(mod).Count > 0)
                    dlcs.Add(mod);
                else
                    remaining.Add(mod);
            }

            string[] sorted = dlcs.ToArray();
            Array.Sort(sorted, StringComparer.InvariantCulture);

            dlcs = sorted.ToList();
            dlcs.AddRange(remaining);
            return dlcs;
        }
        
        public static void generate(List<string> mods, int version, string binPath)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(binPath, FileMode.Create));
            switch (version) {
                case 2019:
                    int decrementing = mods.Count;
                    foreach(string mod in mods)
                    {
                        // First 16 bytes
                        bw.Write('d');
                        for (int i = 0; i < 3; i++) bw.Write(string.Empty);
                        bw.Write(Convert.ToByte(decrementing));
                        decrementing--;
                        for (int i = 0; i < 3; i++) bw.Write(string.Empty);
                        bw.Write('d');
                        for (int i = 0; i < 3; i++) bw.Write(string.Empty);
                        bw.Write('t');
                        bw.Write('\'');
                        for (int i = 0; i < 2; i++) bw.Write(string.Empty);
                        // CPK name
                        bw.Write(Encoding.UTF8.GetBytes(mod));
                        // Remaining bytes
                        for (int i = 0; i < (32 - mod.Length); i++) bw.Write(string.Empty);
                    }
                    break;
            }
            bw.Close();
        }
    }
}

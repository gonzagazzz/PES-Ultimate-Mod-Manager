using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

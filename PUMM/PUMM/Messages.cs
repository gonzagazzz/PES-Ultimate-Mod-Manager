using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PUMM
{
    class Messages
    {
        public static string[] error(string type, string[] args)
        {
            switch(type)
            {
                case "PathNotSet":
                    return new string[] { "PES " + args[0] + " download path was not set. Please browse download folder in Home page.", "Download path not set" };
                case "NoActiveModpack":
                    return new string[] { "There is no active modpack. Please set desired modpack as active in Library page.", "No modpack active" };
                case "EmptyModpack":
                    return new string[] { "Modpack '" + args[0] + "' does not have any mods. Please select mods in Mods page and click 'Save Modpack' to keep changes.", "Modpack is empty" };
                case "EmptyModsList":
                    return new string[] { "Mods list is empty. Please select mods in Mods page and click 'Generate DpFileList.bin'.", "Mods list empty" };
                case "EmptyModpackName":
                    return new string[] { "Modpack's name field is empty. Please fill it with desired name.", "Empty name field" };
                case "EmptyModpackThumbnail":
                    return new string[] { "Modpack's thumbnail field is empty. Please browse desired thumbnail.", "Empty thumbnail field" };
                case "ModpackAlreadyExists":
                    return new string[] { "Modpack '" + args[0] + "' already exists. Please choose a different name.", "Modpack already exists" };
            }
            return null;
        }

        public static string[] success(string type, string[] args)
        {
            switch (type)
            {
                case "GeneratedFromModpack":
                    return new string[] { "Successfully generated DpFileList.bin of modpack '" + args[0] + "'.", "DpFileList.bin Generated" };
                case "GeneratedFromModsList":
                    return new string[] { "Successfully generated DpFileList.bin.", "DpFileList.bin Generated" };
                case "ModpackExported":
                    return new string[] { "Modpack '" + args[0] + "' successfully exported.", "Modpack exported" };
                case "ModpackImported":
                    return new string[] { "Modpack '" + args[0] + "' successfully imported.", "Modpack imported" };
                case "ModpackAdded":
                    return new string[] { "Modpack '" + args[0] + "' successfully added.", "Modpack added" };
                case "ModsSaved":
                    return new string[] { "Modpack's '" + args[0] + "' mods saved successfully.", "Mods saved" };
            }
            return null;
        }

        public static string[] question(string type, string[] args)
        {
            switch (type)
            {
                case "DeleteModpackConfirmation":
                    return new string[] { "Are you sure you want to delete modpack '" + args[0] + "'?", "Confirm modpack deletion" };
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PUMM.Model;

namespace PUMM
{
    class DbProvider
    {
        SQLiteConnection db;

        public DbProvider()
        {
            /* create database if doesn't exist */
            if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "pumm.sqlite"))
            {
                SQLiteConnection.CreateFile("pumm.sqlite");
                db = new SQLiteConnection("Data Source=pumm.sqlite;Version=3;journal_mode=WAL;");
                migrate();
            } else
            {
                db = new SQLiteConnection("Data Source=pumm.sqlite;Version=3;journal_mode=WAL;");
            }
            
        }

        /* Creates required tables */
        public void migrate()
        {
            db.Open();
            string query = "CREATE TABLE settings (" +
                "'property' TEXT NOT NULL," +
                "'value' TEXT," +
                "PRIMARY KEY('property')" +
                ")";
            SQLiteCommand command = new SQLiteCommand(query, db);
            command.ExecuteNonQuery();

            query = "CREATE TABLE modpack (" +
                "'id' INTEGER NOT NULL," +
                "'name' TEXT," +
                "'version' INTEGER NOT NULL," +
                "'thumbnail' TEXT," +
                "'is_active' INTEGER NOT NULL," +
                "PRIMARY KEY('id')" +
                ")";
            command = new SQLiteCommand(query, db);
            command.ExecuteNonQuery();

            query = "CREATE TABLE mod (" +
                "'id' INTEGER NOT NULL," +
                "'filename' TEXT NOT NULL," +
                "'modpack_id' INTEGER NOT NULL," +
                "PRIMARY KEY('id')" +
                ")";
            command = new SQLiteCommand(query, db);
            command.ExecuteNonQuery();
            
            db.Close();
        }

        /* Adds new modpack to database */
        public int addModpack(string name, int version, string thumbnail)
        {
            string query = "INSERT INTO modpack (name, version, thumbnail, is_active) VALUES ('" + name + "', " + version + ", '" + thumbnail + "', 0)";

            db.Open();
            SQLiteCommand command = new SQLiteCommand(query, db);
            command.ExecuteNonQuery();

            query = "SELECT last_insert_rowid();";
            command = new SQLiteCommand(query, db);
            int id = Int32.Parse(command.ExecuteScalar().ToString());
            db.Close();

            return id;
        }

        /* Returns id of modpack if exists, -1 otherwise */
        public Modpack getModpack(string name)
        {
            string query = "SELECT * FROM modpack WHERE name = '" + name + "'";
            Modpack modpack = null;

            db.Open();
            SQLiteCommand command = new SQLiteCommand(query, db);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
                modpack = new Modpack { Id = reader.GetInt32(0), Name = reader.GetString(1), Version = reader.GetInt32(2), ImagePath = reader.GetString(3) };
            reader.Close();
            db.Close();

            return modpack;
        }

        /* Retrieves every modpack from database */
        public ObservableCollection<Modpack> retrieveModpacks(int version)
        {
            ObservableCollection<Modpack> modpacks = new ObservableCollection<Modpack>();
            string query = "SELECT * FROM modpack WHERE version = " + version;

            db.Open();
            SQLiteCommand command = new SQLiteCommand(query, db);
            SQLiteDataReader reader = command.ExecuteReader();
            while(reader.Read())
                modpacks.Add(new Modpack { Id = reader.GetInt32(0), Name = reader.GetString(1), Version = reader.GetInt32(2), ImagePath = reader.GetString(3) });
            db.Close();

            return modpacks;
        }

        /* Updates modpack property with given id */
        public void updateModpack(int id, string column, string value)
        {
            string query = "UPDATE modpack SET " + column + " = '" + value + "' WHERE id = " + id;

            db.Open();
            SQLiteCommand command = new SQLiteCommand(query, db);
            command.ExecuteNonQuery();
            db.Close();
        }

        /* Deletes modpack with given id */
        public void deleteModpack(int id)
        {
            string query = "DELETE FROM modpack WHERE id = " + id;

            db.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, db);
            cmd.ExecuteNonQuery();

            query = "DELETE FROM mod WHERE modpack_id = " + id;
            cmd = new SQLiteCommand(query, db);
            cmd.ExecuteNonQuery();
            db.Close();
        }

        /* Updates modpack list of mods
         * Returns number of mods in modpack if a modpack is active, -1 otherwise */
        public void addModsToModpack(Modpack modpack, ObservableCollection<string> checkedMods, List<string> remainingMods)
        {
            string query;
            bool exists = false;

            db.Open();
            foreach(string mod in checkedMods)
            {
                /* Selects current mod from database to check if exists */
                query = "SELECT * FROM mod WHERE filename = '" + mod + "' AND modpack_id = " + modpack.Id;
                SQLiteCommand select = new SQLiteCommand(query, db);
                SQLiteDataReader reader = select.ExecuteReader();
                exists = reader.Read();

                /* If CPK is checked and doesn't exist in modpack, adds it */
                if (!exists)
                {
                    query = "INSERT INTO mod (filename, modpack_id) VALUES ('" + mod + "', " + modpack.Id + ")";
                    using (var cmd = new SQLiteCommand(query, db))
                        cmd.ExecuteNonQuery();
                }
            }

            if(remainingMods != null)
            {
                foreach (string mod in remainingMods)
                {
                    /* Selects current mod from database to check if exists */
                    query = "SELECT * FROM mod WHERE filename = '" + mod + "' AND modpack_id = " + modpack.Id;
                    SQLiteCommand select = new SQLiteCommand(query, db);
                    SQLiteDataReader reader = select.ExecuteReader();
                    exists = reader.Read();

                    /* If CPK is not checked but exist in modpack, removes it */
                    if (exists)
                    {
                        query = "DELETE FROM mod WHERE filename = '" + mod + "' AND modpack_id = " + modpack.Id;
                        using (var cmd = new SQLiteCommand(query, db))
                            cmd.ExecuteNonQuery();
                    }
                }
            }
            db.Close();
        }

        public List<string> getMods(Modpack modpack)
        {
            if (modpack == null)
                return null;
            
            string query = "SELECT * FROM mod WHERE modpack_id = " + modpack.Id;
            List<string> mods = new List<string>();

            db.Open();
            SQLiteCommand command = new SQLiteCommand(query, db);
            SQLiteDataReader reader = command.ExecuteReader();
            while(reader.Read())
                mods.Add(reader.GetString(1));
            db.Close();
            
            // Sorts mods to respect order of DLCs
            mods = DpFileListUtil.arrangeDLC(mods);
            return mods;
        }

        public bool modpackHasMod(Modpack modpack, string modFilename)
        {
            if (modpack == null)
                return false;

            string query = "SELECT * FROM mod WHERE filename = '" + modFilename + "' AND modpack_id = " + modpack.Id;

            db.Open();
            SQLiteCommand command = new SQLiteCommand(query, db);
            SQLiteDataReader reader = command.ExecuteReader();
            bool hasMod = reader.Read();
            reader.Close();
            db.Close();
            return hasMod;
        }

        public void newSetting(string property, string value)
        {
            string query = "SELECT * FROM settings WHERE property = '" + property + "'";

            db.Open();
            SQLiteCommand select = new SQLiteCommand(query, db);
            SQLiteDataReader reader = select.ExecuteReader();
            bool exists = reader.Read();
            reader.Close();

            if (exists) // setting already exists, overwrite it
            {
                query = "UPDATE settings SET value = '" + value + "' WHERE property = '" + property + "'";
                using (var cmd = new SQLiteCommand(query, db))
                    cmd.ExecuteNonQuery();
            }
            else // does not exist, create it
            {
                query = "INSERT INTO settings (property, value) VALUES ('" + property + "', '" + value + "')";
                using (var cmd = new SQLiteCommand(query, db))
                    cmd.ExecuteNonQuery();
            }
            db.Close();
        }

        public string getSetting(string property)
        {
            string query = "SELECT * FROM settings WHERE property = '" + property + "'";
            string value = "";
            db.Open();
            SQLiteCommand select = new SQLiteCommand(query, db);
            using (var reader = select.ExecuteReader())
            {
                if (reader.Read())
                    value = reader.GetString(1);
            }
            db.Close();
            return value;
        }

        public void removeSetting(string property)
        {
            string query = "DELETE FROM settings WHERE property = '" + property + "'";

            db.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, db);
            cmd.ExecuteNonQuery();
            db.Close();
        }
    }
}

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
        public void addModpack(string name, string thumbnail)
        {
            string query = "INSERT INTO modpack (name, thumbnail, is_active) VALUES ('" + name + "', '" + thumbnail + "', 0)";

            db.Open();
            SQLiteCommand command = new SQLiteCommand(query, db);
            command.ExecuteNonQuery();
            db.Close();
        }

        /* Retrieves every modpack from database */
        public ObservableCollection<Modpack> retrieveModpacks()
        {
            ObservableCollection<Modpack> modpacks = new ObservableCollection<Modpack>();
            string query = "SELECT * FROM modpack";

            db.Open();
            SQLiteCommand command = new SQLiteCommand(query, db);
            SQLiteDataReader reader = command.ExecuteReader();
            while(reader.Read())
                modpacks.Add(new Modpack { Id = reader.GetInt32(0), Name = reader.GetString(1), ImagePath = reader.GetString(2) });
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
            SQLiteCommand command = new SQLiteCommand(query, db);
            command.ExecuteNonQuery();
            db.Close();
        }

        /* Updates modpack list of mods
         * Returns true if a modpack is active, false otherwise */
        public bool addModsToModpack(Modpack modpack, ObservableCollection<Mod> mods)
        {
            if (modpack == null)
                return false;

            string query;
            bool exists = false;

            db.Open();
            foreach(Mod mod in mods)
            {
                /* Selects current mod from database to check if exists */
                query = "SELECT * FROM mod WHERE filename = '" + mod.Filename + "' AND modpack_id = " + modpack.Id;
                SQLiteCommand select = new SQLiteCommand(query, db);
                SQLiteDataReader reader = select.ExecuteReader();
                exists = reader.Read();

                /* If CPK is checked and doesn't exist in modpack, adds it
                 * otherwise, if CPK is not checked but exist in modpack, removes it */
                if(mod.Selected)
                {
                    if(!exists)
                    {
                        query = "INSERT INTO mod (filename, modpack_id) VALUES ('" + mod.Filename + "', " + modpack.Id + ")";
                        using (var cmd = new SQLiteCommand(query, db))
                            cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    if(exists)
                    {
                        query = "DELETE FROM mod WHERE filename = '" + mod.Filename + "' AND modpack_id = " + modpack.Id;
                        using (var cmd = new SQLiteCommand(query, db))
                            cmd.ExecuteNonQuery();
                    }
                }
            }
            db.Close();
            return true;
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
            db.Close();
            return hasMod;
        }
    }
}

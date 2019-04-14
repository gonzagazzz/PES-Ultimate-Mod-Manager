using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                db = new SQLiteConnection("Data Source=pumm.sqlite;Version=3;");
                migrate();
            } else
            {
                db = new SQLiteConnection("Data Source=pumm.sqlite;Version=3;");
            }
            
        }

        /* Creates required tables */
        public void migrate()
        {
            db.Open();
            string query = "CREATE TABLE modpack (" +
                "'id' INTEGER NOT NULL," +
                "'name' TEXT," +
                "'thumbnail' TEXT," +
                "'is_active' INTEGER NOT NULL," +
                "PRIMARY KEY('id')" +
                ")";
            SQLiteCommand command = new SQLiteCommand(query, db);
            command.ExecuteNonQuery();
            query = "CREATE TABLE mod (" +
                "'id' INTEGER NOT NULL," +
                "'filepath' TEXT NOT NULL," +
                "PRIMARY KEY('id')" +
                ")";
            command = new SQLiteCommand(query, db);
            command.ExecuteNonQuery();
            query = "CREATE TABLE mod_modpack (" +
                "'id_mod' INTEGER NOT NULL," +
                "'id_modpack' INTEGER NOT NULL" +
                ")";
            command = new SQLiteCommand(query, db);
            command.ExecuteNonQuery();
            db.Close();
        }
    }
}

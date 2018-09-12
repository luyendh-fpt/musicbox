using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using MusicBox.Entity;
using SQLite.Net;
using Windows.Storage;

namespace MusicBox.Model
{
    class DataAccess
    {
        public static void InitializeDatabase()
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=songs_manager.db"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS songs (id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "title NVARCHAR(50) NOT NULL, " +
                    "description NVARCHAR(255), " +
                    "author NVARCHAR(50), " +
                    "kind NVARCHAR(50), " +
                    "singer NVARCHAR(50), " +
                    "link NVARCHAR(255) NOT NULL, " +
                    "thumbnail NVARCHAR(255)" +
                    ") " ;

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static string DB_PATH = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "SongsManager.sqlite"));
        public static void CreateDatabase()
        {
           
                using (var db = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), DB_PATH))
                {
                    db.CreateTable<Song>();
                
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

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
    }
}

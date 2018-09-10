using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBox.Model
{
    public class SongModel
    {
        private static ObservableCollection<Entity.Song> listSong;

        private static void InitSongs() {
            listSong.Add(new Entity.Song
            {
                Id = 1,
                Title = "Giấc mơ chỉ là giấc mơ",
                Thumbnail = "http://lyric.tkaraoke.com/24714/giac_mo_chi_la_giac_mo.gif",
                Author = "Đức Trí",
                Singer = "Hà Anh Tuấn",
                Link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui945/GiacMoChiLaGiacMoSeeSingShare2-HaAnhTuan-5082049.mp3",
                Kind = "Nhạc Trẻ"
            });
            listSong.Add(new Entity.Song
            {
                Id = 2,
                Title = "Người tình mùa đông",
                Thumbnail = "http://khuyennhac.net/wp-content/uploads/2016/06/Nguoi-tinh-mua-dong.png",
                Author = "Anh Bằng",
                Singer = "Hà Anh Tuấn",
                Link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui963/NguoiTinhMuaDongSEESINGSHARE2-HaAnhTuan-5104816.mp3",
                Kind = "Nhạc Trẻ"
            });
            listSong.Add(new Entity.Song
            {
                Id = 3,
                Title = "Nơi ấy bình yên",
                Thumbnail = "https://3.bp.blogspot.com/-9Qn0gEJqseA/V0ZOOqZFDgI/AAAAAAAAMog/AdZm-YzBHFEBS_yNCp1kTievslN4--n6gCLcB/s1600/NOI%2BAY%2BBINH%2BYEN-Bao%2BChan%2BAm.jpg",
                Author = "Bảo Chấn",
                Singer = "Hà Anh Tuấn",
                Link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui946/NoiAyBinhYenSeeSingShare2-HaAnhTuan-5085337.mp3",
                Kind = "Nhạc Trẻ"
            });
        }

        public static ObservableCollection<Entity.Song> GetSongs() {
            DataAccess.InitializeDatabase();

            if (listSong == null) {
                listSong = new ObservableCollection<Entity.Song>();
                
            }
            using (SqliteConnection db = new SqliteConnection("Filename=songs_manager.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM songs";
                SqliteDataReader sqliteData = selectCommand.ExecuteReader();
                Entity.Song song;
                while (sqliteData.Read())
                {
                    song = new Entity.Song {
                        Id = Convert.ToInt16(sqliteData["id"]),
                        Title = Convert.ToString(sqliteData["title"]),
                        Description = Convert.ToString(sqliteData["description"]),
                        Author = Convert.ToString(sqliteData["author"]),
                        Kind = Convert.ToString(sqliteData["kind"]),
                        Link = Convert.ToString(sqliteData["link"]),
                        Singer = Convert.ToString(sqliteData["singer"]),
                        Thumbnail = Convert.ToString(sqliteData["thumbnail"]),
                    };
                    listSong.Add(song);
                }              
                db.Close();
            }
            if (listSong == null)
            {
                listSong = new ObservableCollection<Entity.Song>();
            }
            return listSong;
        }

        public static void SetSongs(ObservableCollection<Entity.Song> songs)
        {
            listSong = songs;
        }

        public static void AddSong(Entity.Song song)
        {
            DataAccess.InitializeDatabase();
            using (SqliteConnection db =new SqliteConnection("Filename=songs_manager.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO songs (title, description, author, kind, singer, link, thumbnail) VALUES (@title, @description, @author, @kind, @singer, @link, @thumbnail);";
                insertCommand.Parameters.AddWithValue("@title", song.Title);
                insertCommand.Parameters.AddWithValue("@description", song.Description);
                insertCommand.Parameters.AddWithValue("@author", song.Author);
                insertCommand.Parameters.AddWithValue("@kind", song.Kind);
                insertCommand.Parameters.AddWithValue("@singer", song.Singer);
                insertCommand.Parameters.AddWithValue("@link", song.Link);
                insertCommand.Parameters.AddWithValue("@thumbnail", song.Thumbnail);

                insertCommand.ExecuteReader();

                        db.Close();
                    }
            if (listSong == null)
            {
                listSong = new ObservableCollection<Entity.Song>();
            }
            listSong.Add(song);
        }
    }
}

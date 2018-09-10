using MusicBox.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBox.Service
{
    class SongService
    {
        public static ObservableCollection<Song> Songs = null;
        public static MetaData MetaData { get; set; }

        public static void LoadStaticSong() {
            if (Songs == null) {
                Songs = new ObservableCollection<Song>();
            }
            if (Songs.Count == 0) {
                Songs.Add(new Song
                {
                    Id = 1,
                    Title = "1234",
                    Thumbnail = "http://localhost/music/chi%20dan.jpg",
                    Author = "Singer Chí Dân",
                    Singer = "Chí Dân",
                    Link = "http://localhost/music/1234%20-%20Chi%20Dan.mp3",
                    Kind = "Nhạc Trẻ"
                });
                Songs.Add(new Song
                {
                    Id = 2,
                    Title = "1234",
                    Thumbnail = "http://localhost/music/chi%20dan.jpg",
                    Author = "Singer Chí Dân",
                    Singer = "Chí Dân",
                    Link = "http://localhost/music/1234%20-%20Chi%20Dan.mp3",
                    Kind = "Nhạc Trẻ"
                });
                Songs.Add(new Song
                {
                    Id = 3,
                    Title = "1234",
                    Thumbnail = "http://localhost/music/chi%20dan.jpg",
                    Author = "Singer Chí Dân",
                    Singer = "Chí Dân",
                    Link = "http://localhost/music/1234%20-%20Chi%20Dan.mp3",
                    Kind = "Nhạc Trẻ"
                });             
            }
        }

        public static void LoadSongFromApi() {

        }

        public static ObservableCollection<Song> GetSongs(int page, int limit)
        {
            LoadStaticSong();
            if (MetaData == null) {
                MetaData = new MetaData();
            }
            // tao moi meta data tu api hoac fix tai local.
            MetaData.Page = page;
            MetaData.Limit = limit;
            MetaData.TotalPage = 1;
            MetaData.From = 1;
            MetaData.To = 6;
            MetaData.Total = 6;            
            return Songs;
        }
        
    }
}

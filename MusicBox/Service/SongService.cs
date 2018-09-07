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
                    Title = "Me Muoi",
                    Author = "KV",
                    Singer = "Tram",
                    Description = "https://c1-ex-swe.nixcdn.com/NhacCuaTui967/MeMuoi-BuiLanHuong-5574358.mp3",
                    Kind = "nhac tre"
                });
                Songs.Add(new Song
                {
                    Id = 2,
                    Title = "Mot Hai Ba Bon",
                    Author = "KV",
                    Singer = "Tram",
                    Description = "http://focusteam-api.tk/music/1234%20-%20Chi%20Dan.mp3",
                    Kind = "nhac tre"
                });
                Songs.Add(new Song
                {
                    Id = 3,
                    Title = "Em Gai Mua 3",
                    Author = "KV",
                    Singer = "Tram",
                    Description = "Hay",
                    Kind = "nhac tre"
                });
                Songs.Add(new Song
                {
                    Id = 4,
                    Title = "Em Gai Mua 4",
                    Author = "KV",
                    Singer = "Tram",
                    Description = "Hay",
                    Kind = "nhac tre"
                });
                Songs.Add(new Song
                {
                    Id = 5,
                    Title = "Em Gai Mua 5",
                    Author = "KV",
                    Singer = "Tram",
                    Description = "Hay",
                    Kind = "nhac tre"
                });
                Songs.Add(new Song
                {
                    Id = 6,
                    Title = "Em Gai Mua 6",
                    Author = "KV",
                    Singer = "Tram",
                    Description = "Hay",
                    Kind = "nhac tre"
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

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
                Title = "1234",
                Thumbnail = "http://localhost/music/chi%20dan.jpg",
                Author = "Singer Chí Dân",
                Singer = "Chí Dân",
                Link = "http://localhost/music/1234%20-%20Chi%20Dan.mp3",
                Kind = "Nhạc Trẻ"
            });
            listSong.Add(new Entity.Song
            {
                Id = 2,
                Title = "Anh nang cua anh",
                Thumbnail = "http://localhost/music/chi%20dan.jpg",
                Author = "Singer Đức Phúc",
                Singer = "Đức Phúc",
                Link = "http://localhost/music/Anh%20nang%20cua%20anh.mp3",
                Kind = "Nhạc Trẻ"
            });
            listSong.Add(new Entity.Song
            {
                Id = 3,
                Title = "Because you live",
                Thumbnail = "http://localhost/music/chi%20dan.jpg",
                Author = "Singer  Jesse McCartney",
                Singer = " Jesse McCartney",
                Link = "http://localhost/music/Because%20you%20live.mp3",
                Kind = "Nhạc Trẻ"
            });
            listSong.Add(new Entity.Song
            {
                Id = 4,
                Title = "Hoc tieng meo keu",
                Thumbnail = "http://localhost/music/chi%20dan.jpg",
                Author = "Singer  Tieu Phong Phong",
                Singer = " Tieu Phong Phong",
                Link = "http://localhost/music/Hoc%20tieng%20meo%20keu.mp3",
                Kind = "Nhạc Trẻ"
            });
            listSong.Add(new Entity.Song
            {
                Id = 5,
                Title = "Yeu em rat nhieu",
                Thumbnail = "http://localhost/music/chi%20dan.jpg",
                Author = "Singer  Hoang Ton",
                Singer = " Hoang Ton",
                Link = "http://localhost/music/YeuEmRatNhieu-HoangTon-5166491.mp3",
                Kind = "Nhạc Trẻ"
            });
        }

        public static ObservableCollection<Entity.Song> GetSongs() {
            if (listSong == null) {
                listSong = new ObservableCollection<Entity.Song>();
                InitSongs();
            }
            return listSong;
        }

        public static void SetSongs(ObservableCollection<Entity.Song> songs)
        {
            listSong = songs;
        }

        public static void AddSong(Entity.Song song)
        {
            if (listSong == null)
            {
                listSong = new ObservableCollection<Entity.Song>();
            }
            listSong.Add(song);
        }
    }
}

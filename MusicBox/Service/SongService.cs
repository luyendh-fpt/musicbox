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
                    Title = "Giấc mơ chỉ là giấc mơ",
                    Thumbnail = "http://lyric.tkaraoke.com/24714/giac_mo_chi_la_giac_mo.gif",
                    Author = "Đức Trí",
                    Singer = "Hà Anh Tuấn",
                    Link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui945/GiacMoChiLaGiacMoSeeSingShare2-HaAnhTuan-5082049.mp3",
                    Kind = "Nhạc Trẻ"
                });
                Songs.Add(new Song
                {
                    Id = 2,
                    Title = "Người tình mùa đông",
                    Thumbnail = "http://khuyennhac.net/wp-content/uploads/2016/06/Nguoi-tinh-mua-dong.png",
                    Author = "Anh Bằng",
                    Singer = "Hà Anh Tuấn",
                    Link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui963/NguoiTinhMuaDongSEESINGSHARE2-HaAnhTuan-5104816.mp3",
                    Kind = "Nhạc Trẻ"
                });
                Songs.Add(new Song
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

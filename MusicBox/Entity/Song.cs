using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBox.Entity
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Kind { get; set; }
        public string Singer { get; set; }

        public string Link { get => link; set => link = value; }
        public string Thumbnail { get => thumbnail; set => thumbnail = value; }

        private string link;
        private string thumbnail;
    }
}

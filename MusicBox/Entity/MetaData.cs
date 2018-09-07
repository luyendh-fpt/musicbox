using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBox.Entity
{
    public class MetaData
    {
        public int Total { get; set; }
        public int TotalPage { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int From { get; set; }
        public int To { get; set; }

        public ObservableCollection<int> ListPage { get; set; }

        public MetaData()
        {
            ListPage = new ObservableCollection<int>();
            ListPage.Add(1);
            ListPage.Add(2);            
        }
    }
}

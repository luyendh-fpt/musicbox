using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MusicBox.Entity;
using MusicBox.Service;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicBox.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListSong : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Song> _songs;
        private Song _selectedSong = null;
        private MetaData _meta;
        private bool isPlaying = false;

        public MetaData MetaItems { get; set; }
        public ObservableCollection<Song> Songs
        {
            get => _songs;
            set
            {
                if (value != _songs)
                {
                    _songs = value;
                    OnPropertyChanged();
                }
            }
        }

        public Song SelectedSong
        {
            get => _selectedSong;
            set
            {
                if (value != this._selectedSong)
                {
                    this._selectedSong = value;
                    OnPropertyChanged();
                }
            }
        }

        public MetaData Meta
        {
            get => _meta;
            set
            {
                if (value != this._meta)
                {
                    this._meta = value;
                    OnPropertyChanged();
                }
            }
        }

        public ListSong()
        {
            if (SongService.Songs == null)
            {
                SongService.GetSongs(1, 6);                
            }
            Songs = SongService.Songs;

            MetaItems = SongService.MetaData;
            this.InitializeComponent();
        }

        private void Kind_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // Songs = SongService.GetSongs(((MetaData)e.AddedItems[0]).Page, 6);
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            SelectedSong = (Song)((StackPanel)sender).Tag;
            if (SelectedSong != null)
            {
                Uri newuri = new Uri(SelectedSong.Link);
                this.MyPlayer.Source = newuri;
                Rectangle1.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(SelectedSong.Thumbnail));


                this.SongDetail.Visibility = Visibility.Visible;

            }

        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void DownLoadBtn_OnClick(object sender, RoutedEventArgs e)
        {
            string requestUri =
                "https://upload.wikimedia.org/wikipedia/en/thumb/6/63/IMG_%28business%29.svg/1200px-IMG_%28business%29.svg.png";
            string filename = "Nono.jpg";
            if (requestUri == null)
                throw new ArgumentNullException("requestUri");

            await DownloadAsync(new Uri(requestUri), filename);
        }

        public async Task DownloadAsync(Uri requestUri, string filename)
        {
            if (filename == null)
                throw new ArgumentNullException(filename);

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
                {
                    using (Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(),
                        stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 10000, true))
                    {
                        await contentStream.CopyToAsync(stream);
                    }
                }
            }
        }

        private void ShowSongForm(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof (View.SongForm));
        }

        private void playSong()
        {
            MyPlayer.Play();
            isPlaying = true;
        }

        private void pauseSong()
        {
            MyPlayer.Pause();
            isPlaying = false;
        }

        private void resumeSong()
        {
            if (!isPlaying)
            {
                playSong();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using MusicBox.Entity;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MusicBox
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Entity.Song> songs;
        public ObservableCollection<Song> Songs { get => Model.SongModel.GetSongs(); set => Model.SongModel.SetSongs(value); }

        private bool isPlaying = false;

        int onPlay = 0;


        public MainPage()
        {
            this.InitializeComponent();
            volumeSlider.Value = 100;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, object e)
        {
            if (MediaPlayer.Source != null && MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                Progress.Minimum = 0;
                Progress.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                Progress.Value = MediaPlayer.Position.TotalSeconds;

            }
        }





        private void SaveSong(object sender, RoutedEventArgs e)
        {
            Entity.Song song = new Entity.Song();
            song.Title = this.Title.Text;
            song.Description = this.Description.Text;
            song.Author = this.Author.Text;
            song.Singer = this.Singer.Text;
            song.Kind = this.Kind.Text;
            song.Link = this.Link.Text;
            song.Thumbnail = this.Thumbnail.Text;
            Model.SongModel.AddSong(song);
        }

        private void Song_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Entity.Song selectedSong = (Entity.Song)((StackPanel)sender).Tag;
            MediaPlayer.Source = new Uri(selectedSong.Link);
            onPlay = selectedSong.Id - 1;
            this.nowPlaying.Text = selectedSong.Title + " - " + selectedSong.Singer;

            playSong();
        }

        private void loadSong(Entity.Song currentSong)
        {
            this.nowPlaying.Text = currentSong.Title + " - " + currentSong.Singer;
            MediaPlayer.Source = new Uri(currentSong.Link);
        }

        private void playSong()
        {
            MediaPlayer.Play();
            PlayButton.Icon = new SymbolIcon(Symbol.Pause);
            isPlaying = true;
        }

        private void pauseSong()
        {
            MediaPlayer.Pause();
            PlayButton.Icon = new SymbolIcon(Symbol.Play);
            isPlaying = false;

        }

        private void resumeSong()
        {
            if (!isPlaying)
            {
                playSong();
            }
        }

        private void playBack(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Stop();
            if (onPlay > 0)
            {
                onPlay = onPlay - 1;

            }
            else
            {
                onPlay = Songs.Count - 1;
            }
            loadSong(Songs[onPlay]);
            playSong();
            MenuList.SelectedIndex = onPlay;
        }

        private void playNext(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Stop();
            if (onPlay < Songs.Count - 1)
            {
                onPlay = onPlay + 1;
            }
            else
            {
                onPlay = 0;
            }
            loadSong(Songs[onPlay]);
            playSong();
            MenuList.SelectedIndex = onPlay;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                pauseSong();

            }
            else
            {
                playSong();
            }
        }

        private void volumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider vol = sender as Slider;

            if (vol != null)
            {
                MediaPlayer.Volume = vol.Value / 100;

                this.volume.Text = vol.Value.ToString();
            }
        }
    }
}

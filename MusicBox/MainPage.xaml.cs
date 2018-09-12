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


        public MainPage()
        {
            this.InitializeComponent();
            User.Navigate(typeof(View.UserForm));
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
            MediaPlayer.Play();
        }
    }
}

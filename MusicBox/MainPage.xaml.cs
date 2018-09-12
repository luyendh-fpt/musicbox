using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using MusicBox.Entity;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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





        private async void SaveSong(object sender, RoutedEventArgs e)
        {
            //        Entity.Song song = new Entity.Song();
            //        song.Title = this.Title.Text;
            //        song.Description = this.Description.Text;
            //        song.Author = this.Author.Text;
            //        song.Singer = this.Singer.Text;
            //        song.Kind = this.Kind.Text;
            //        song.Link = this.Link.Text;
            //        song.Thumbnail = this.Thumbnail.Text;
            //        //Model.SongModel.AddSong(song);
            //        CameraCaptureUI captureUI = new CameraCaptureUI();
            //        captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            //        captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            //        StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            //        if (photo == null)
            //        {
            //            // User cancelled photo capture
            //            return;
            //        }
            //        StorageFolder destinationFolder =
            //await ApplicationData.Current.LocalFolder.CreateFolderAsync("ProfilePhotoFolder",
            //    CreationCollisionOption.OpenIfExists);

            //        await photo.CopyAsync(destinationFolder, "ProfilePhoto.jpg", NameCollisionOption.ReplaceExisting);
            //        await photo.DeleteAsync();
            HttpUploadFile(UploadUrl, "myFile", "image/png");

        }

        public static async void Upload(byte[] image)
        {
            using (var client = new HttpClient())
            {
                using (var content =
                    new MultipartFormDataContent("Upload----" + DateTime.Now.ToString()))
                {
                    content.Add(new StreamContent(new MemoryStream(image)), "myFile");

                    using (
                       var message =
                           await client.PostAsync(UploadUrl, content))
                    {
                        var input = await message.Content.ReadAsStringAsync();
                        Debug.WriteLine(message);
                        Debug.WriteLine("@@");
                        Debug.WriteLine(input);
                    }
                }
            }
        }

        private static string UploadUrl;
        private static StorageFile file;

        private static async void GetUploadUrl()
        {
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            Uri requestUri = new Uri("https://1-dot-backup-server-002.appspot.com/get-upload-token");
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";
            try
            {
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
            Debug.WriteLine(httpResponseBody);
            UploadUrl = httpResponseBody;
        }

        public static async void HttpUploadFile(string url, string paramName, string contentType)
        {          
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
           
            Stream rs = await wr.GetRequestStreamAsync();
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, "path_file", contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            // write file.
            Stream fileStream = await file.OpenStreamForReadAsync();
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
           
            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);

            WebResponse wresp = null;
            try
            {
                wresp = await wr.GetResponseAsync();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                Debug.WriteLine(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error uploading file", ex.InnerException);
                if (wresp != null)
                {                    
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }

        private async void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            GetUploadUrl();
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".jpeg");            
            file = await openPicker.PickSingleFileAsync();            
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
            //MediaPlayer.Source = new Uri(currentSong.Link);
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

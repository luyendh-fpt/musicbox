using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using MusicBox.Annotations;
using MusicBox.Entity;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicBox.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserForm : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserForm()
        {
            FormUser = new User();
            this.InitializeComponent();
        }

        public ObservableCollection<Gender> Genders = new ObservableCollection<Gender>
        {
            new Gender {Name = "Male", Value = 1},
            new Gender {Name = "FeMale", Value = 0}
        };

        private User _formUser;
        public User FormUser {get => _formUser; set { if (_formUser != value) { _formUser = value; OnPropertyChanged(); } } }

        private void Birthday_OnDateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            FormUser.Birthday = String.Format("{0:yyyy-MM-dd}", sender.Date);
        }

        private async void OnSelectImg(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
        }

        private async void OnSubmit(object sender, RoutedEventArgs e)
        {
            UWPConsole.Console.WriteLine(FormUser.FirstName);
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(HttpMediaTypeWithQualityHeaderValue.Parse("application/json"));
            Uri requestUri = new Uri("https://1-dot-backup-server-002.appspot.com/member/register");
            
            IHttpContent content = new HttpStringContent(JsonConvert.SerializeObject(FormUser));
            try
            {
                Windows.Web.Http.HttpResponseMessage httpResponse = await httpClient.PostAsync(requestUri, content);
                httpResponse.EnsureSuccessStatusCode();
                string httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                UWPConsole.Console.WriteLine(httpResponseBody);
            }
            catch (Exception ex)
            {
                UWPConsole.Console.WriteLine(ex.Message);
            }
        }

        private void OnSelectGender(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbx = (ComboBox) sender;
            FormUser.Gender = (int) cbx.SelectedValue;
        }


    }

    public class Gender
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
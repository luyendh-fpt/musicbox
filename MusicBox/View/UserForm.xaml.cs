using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using MusicBox.Annotations;
using MusicBox.Entity;
using MusicBox.Utility;
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

        private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings();

        /// <summary>
        /// The User data to send throw API. Working with two way binding.
        /// </summary>
        private User _formUser = new User();
        public User FormUser { get => _formUser; set { if (_formUser != value) { _formUser = value; OnPropertyChanged(); } } }

        public UserForm()
        {
            this._jsonSerializerSettings.ContractResolver = new LowercaseContractResolver();
            this.InitializeComponent();
        }

        /// <summary>
        /// Item Resource for select gender
        /// </summary>
        public ObservableCollection<Gender> Genders = new ObservableCollection<Gender>
        {
            new Gender {Name = "Male", Value = 1},
            new Gender {Name = "FeMale", Value = 0}
        };

        /// <summary>
        /// Trigger khi chọn ngày sinh nhật, set giá trị birhtday cho FormUser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Birthday_OnDateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            this.FormUser.Birthday = String.Format("{0:yyyy-MM-dd}", sender.Date);
        }

        /// <summary>
        /// Hàm mở chọn file, sau đó thực hiện up ảnh. Hàm này là async nên có thể thực hiện thao tác khác trong khi upload.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnSelectImg(object sender, RoutedEventArgs e)
        {
            try
            {
                var uploadUrl = this.GetUploadUrl();
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".png");
                StorageFile file = await openPicker.PickSingleFileAsync();

                // Ẩn nội dung trong nút và hiển thị progress ring
                this.AvatarBtnContent.Visibility = Visibility.Collapsed;
                this.AvatarPreview.Visibility = Visibility.Collapsed;
                this.UploadImgProgress.Visibility = Visibility.Visible;

                this.FormUser.Avatar = await this.HttpUploadFile(await uploadUrl, "myFile", "image/png", file);
                BitmapImage img = new BitmapImage(new Uri(this.FormUser.Avatar));

                // Hiển thị ảnh demo ở trong nút, ẩn progress ring.
                this.AvatarPreview.Source = img;
                this.AvatarPreview.Visibility = Visibility.Visible;
                this.UploadImgProgress.Visibility = Visibility.Collapsed;

                UWPConsole.BackgroundConsole.WriteLine("Upload Img success!");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                UWPConsole.BackgroundConsole.WriteLine("Upload Img failed!");
                Debug.WriteLine(exception.InnerException);
            }
            
        }

        /// <summary>
        /// Submit form, Tất cả dữ liệu đều là twoway binding nên không cần get dữ liệu nữa.
        /// Chỉ cần parse ra json string và gửi đi là xong.
        /// Hàm chưa validate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnSubmit(object sender, RoutedEventArgs e)
        {
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(HttpMediaTypeWithQualityHeaderValue.Parse("application/json"));
            Uri requestUri = new Uri("https://1-dot-backup-server-002.appspot.com/member/register");

            string jsonToSend = JsonConvert.SerializeObject(FormUser, Formatting.Indented, this._jsonSerializerSettings);
            UWPConsole.BackgroundConsole.WriteLine(jsonToSend);

            IHttpContent content = new HttpStringContent(jsonToSend);
            try
            {
                Windows.Web.Http.HttpResponseMessage httpResponse = await httpClient.PostAsync(requestUri, content);
                httpResponse.EnsureSuccessStatusCode();
                string httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                UWPConsole.BackgroundConsole.WriteLine(httpResponseBody);
            }
            catch (Exception ex)
            {
                UWPConsole.BackgroundConsole.WriteLine("Error when submit form");
                Debug.WriteLine(ex.InnerException);
            }
        }

        /// <summary>
        /// Khi chọn giới tính, set giá trị gender cho FormUser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectGender(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbx = (ComboBox) sender;
            FormUser.Gender = (int) cbx.SelectedValue;
        }

        /// <summary>
        /// - Lấy url up ảnh từ api.
        /// - Trả về Task<string> khi nào cần đến string này mới await.
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetUploadUrl()
        {
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            Uri requestUri = new Uri("https://1-dot-backup-server-002.appspot.com/get-upload-token");
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();

            httpResponse = await httpClient.GetAsync(requestUri);
            httpResponse.EnsureSuccessStatusCode();

            return await httpResponse.Content.ReadAsStringAsync();

        }

        /// <summary>
        /// - Hàm upload file ảnh lên api.
        /// - Trả lại Task<string> chính là "Thread" có url, khi nào cần sử dụng thì mới await nó.
        /// - Không cần try catch, sẽ try catch khi gọi đến hàm này.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <param name="contentType"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> HttpUploadFile(string url, string paramName, string contentType, StorageFile file)
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

            wresp = await wr.GetResponseAsync();
            Stream stream2 = wresp.GetResponseStream();
            StreamReader reader2 = new StreamReader(stream2);

            return reader2.ReadToEnd();
        }
    }

    public class Gender
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
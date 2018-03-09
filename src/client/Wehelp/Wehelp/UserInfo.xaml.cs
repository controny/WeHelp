using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Wehelp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserInfo : Page
    {
        public UserInfo()
        {
            this.InitializeComponent();
        }
        Models.Users currentUser { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            //dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.DataRequested);
            if (e.Parameter.GetType() == typeof(Models.Users))
            {
                currentUser = (Models.Users)(e.Parameter);
            }

            /*if (rootFrame.CanGoBack)
             {
                 SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                     AppViewBackButtonVisibility.Visible;
             }
             else
             {
                 SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                     AppViewBackButtonVisibility.Collapsed;
             }*/

            username.Text = currentUser.userName;
            phone.Text = currentUser.phone;
            user.Text = username.Text;
            score.Text = currentUser.score.ToString();

            if (currentUser.localsource != null) 
                {
                    touxiang.ImageSource = currentUser.localsource;
                    headSculpture.ImageSource = currentUser.localsource;
                }
        }


        //点击登出，回到signin页面
        private void signoutClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Signin));
        }
        //跳转至新建Item页面
        private void createTask(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddItem), currentUser);
        }
        //刷新本页面
        private void refreshClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserInfo), currentUser);
        }
        //跳转至好友页面
        private void friendsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Friends), currentUser);
        }
        //跳转至已完成页面
        private void finishedClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FinishedItem), currentUser);
        }

        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        byte[] temp;
        private async void selectPicture(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var s = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                DataReader read = new DataReader(s.GetInputStreamAt(0));
                await read.LoadAsync((uint)s.Size);

                temp = new byte[s.Size];
                read.ReadBytes(temp);   //图片的二进制流

                string picname = Guid.NewGuid().ToString() + ".jpg";
                StorageFile picfile = await localFolder.CreateFileAsync(picname);
                await FileIO.WriteBytesAsync(picfile, temp);  //将图片的二进制流放在本地文件
                uri = "ms-appdata:///local/" + picname;
                ImageSource b = new BitmapImage(new Uri(uri, UriKind.Absolute));
                headSculpture.ImageSource = b;
            }
        }
        string uri;
        private void submitButtonClick(object sender, RoutedEventArgs e)
        {
            ChangeUserInfo();
            Frame.Navigate(typeof(MainPage), currentUser);
        }
        public static string getString(byte[] bytes)

        {

            StringBuilder strBuilder = new StringBuilder();

            foreach (byte bt in bytes)

            {

                strBuilder.AppendFormat("{0:X2}", bt);

            }

            return strBuilder.ToString();

        }
        private byte[] getBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private async void ChangeUserInfo()
        {
            string t = "";
            if (password.Text == "")
            {
                t = currentUser.password;
            }
            else
            {
                t = password.Text;
            }
            using (HttpClient client = new HttpClient())
            {
                var kvp = new List<KeyValuePair<string, string>>
                {
                     new KeyValuePair<string,string>("userId",currentUser.userId),
                     new KeyValuePair<string, string>("password", t),
                     new KeyValuePair<string, string>("headSculpture", uri)
                };
                HttpResponseMessage response = await client.PostAsync("http://paradox5.cn/userInfo" + "?userId=" + currentUser.userId, new FormUrlEncodedContent(kvp));
                string responseBody = await response.Content.ReadAsStringAsync();
                JObject jo = (JObject)JsonConvert.DeserializeObject(responseBody);
                if (jo["statusCode"].ToString() == "ok")
                {
                    currentUser.password = t;
                    currentUser.localsource = headSculpture.ImageSource;
                    await new MessageDialog("Successful operation.").ShowAsync();
                }
                else
                {
                    await new MessageDialog(jo["errorMessage"].ToString()).ShowAsync();
                }
            }
        }

        private void cancelButtonClick(object sender, RoutedEventArgs e)
        {
            
           

            password.Text = "";
        }

        private void homeClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), currentUser);
        }
    }
}

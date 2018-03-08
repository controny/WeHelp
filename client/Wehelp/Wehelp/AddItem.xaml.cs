using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public sealed partial class AddItem : Page
    {
        public AddItem()
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
            publisher.Text = currentUser.userName;
            username.Text = currentUser.userName;
            score.Text = currentUser.score.ToString();
            if (currentUser.localsource  != null)
            touxiang.ImageSource = currentUser.localsource;
        }
        //点击setting，跳转至userInfo页面
        private void userSettingClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserInfo), currentUser);
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
            Frame.Navigate(typeof(AddItem), currentUser);
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
                byte[] temp = new byte[s.Size];
                read.ReadBytes(temp);

                string picname = Guid.NewGuid().ToString() + ".jpg";
                StorageFile picfile = await localFolder.CreateFileAsync(picname);

                await FileIO.WriteBytesAsync(picfile, temp);

                ImageSource b = new BitmapImage(new Uri("ms-appdata:///local/" + picname, UriKind.Absolute));
                task_image.Source = b;
            }
        }

        private async void submitButtonClick(object sender, RoutedEventArgs e)
        {
            
            string errorMessage = "";
            if (!Regex.IsMatch(commission.Text, "^\\d*\\.?\\d*$") || commission.Text == "")
            {
                await new MessageDialog("Commission should be number.").ShowAsync();
                return;
            }
            if (details.Text == "")
            {
                await new MessageDialog("Details shouldn't be empty.").ShowAsync();
                return;
            }

                    using (HttpClient client = new HttpClient())
            {
                try
                {
                    var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("itemId", Guid.NewGuid().ToString()),
                    new KeyValuePair<string,string>("publisherId", publisher.Text),
                    new KeyValuePair<string,string>("type", type.Text),
                    new KeyValuePair<string,string>("location", location.Text),
                    new KeyValuePair<string,string>("details", details.Text),
                    new KeyValuePair<string,string>("commission", commission.Text),
                    new KeyValuePair<string,string>("dateTime", DateTime.Now.ToString()),
                    new KeyValuePair<string,string>("isCompleted", "false")

                };
                    HttpResponseMessage response = await client.PostAsync("http://paradox5.cn/addItem?userId=" + currentUser.userId,
                        new FormUrlEncodedContent(kvp));
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var responceJson = JsonConvert.DeserializeObject<JObject>(responseBody);
                    errorMessage = responceJson["statusCode"].ToString();
                    if (responceJson["statusCode"].ToString() == "ok")
                    {
                        currentUser.itemId += "&" + currentUser.currentTarget;
                        Frame.Navigate(typeof(MainPage), currentUser);
                    } else
                    {
                        await new MessageDialog(errorMessage).ShowAsync();
                    }
                }
                catch (HttpRequestException ex)
                {
                    await new MessageDialog(errorMessage).ShowAsync();
                }
            }
        }

        private void cancelButtonClick(object sender, RoutedEventArgs e)
        {
            type.Text = "";
            details.Text = "";
            location.Text = "";
            commission.Text = "";
        }

        private void Testbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;
            if (!Regex.IsMatch(textbox.Text, "^\\d*\\.?\\d*$") && textbox.Text != "")
            {
                int pos = textbox.SelectionStart - 1;
                textbox.Text = textbox.Text.Remove(pos, 1);
                textbox.SelectionStart = pos;
            }
        }

        private void homeClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), currentUser);
        }

    }
}

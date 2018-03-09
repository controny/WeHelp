using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
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

/*
 * 向服务器发送一个请求，获取该用户被他人完成的任务
 * 每个页面都可以通过汉堡菜单跳转到各个页面和背景的改变
 * 
 * By 许慕欣
 */


namespace Wehelp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FinishedItem : Page
    {
        public FinishedItem()
        {
            this.InitializeComponent();
            finishedItemViewModel = new ViewModels.HelpItemViewModel();
        }
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        ViewModels.HelpItemViewModel finishedItemViewModel { get; set; }
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
            else
            {
                return;
            }

            ImageBrush im = new ImageBrush();
            im.ImageSource = currentUser.background;
            bg.Background = im;

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
            clientName.Text = currentUser.userName;
            marks.Text = (currentUser.score).ToString();
            if (currentUser.localsource != null)
                touxiang.ImageSource = currentUser.localsource;
            askForFinishedInfo();
        }
        private string getString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        private byte[] getBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private async void askForFinishedInfo()
        {
            using (HttpClient client = new HttpClient())
            {
                var kvp = new List<KeyValuePair<string, string>>
                {
                     new KeyValuePair<string,string>("userId",currentUser.userId),
                };

                HttpResponseMessage response = await client.PostAsync("http://paradox5.cn/finishedItem" + "?userId=" + currentUser.userId, new FormUrlEncodedContent(kvp));
                string responseBody = await response.Content.ReadAsStringAsync();
                JObject jo = (JObject)JsonConvert.DeserializeObject(responseBody);
                if (jo["statusCode"].ToString() == "ok")
                {
                    foreach (JToken o in jo["list"].Children())
                    {
                        String publisherName = o["publisherName"].ToString();
                        String consumatorName = o["consummatorName"].ToString();
                        String location = o["location"].ToString();
                        String commission = o["commission"].ToString();
                        String type = o["type"].ToString();
                        String itemid = o["itemId"].ToString();

                        finishedItemViewModel.AddFinishedItem(publisherName, consumatorName, location, type, int.Parse(commission),itemid);
                    }
                }
                else
                {
                    await new MessageDialog(jo["statusCode"].ToString()).ShowAsync();
                }
            }
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
            Frame.Navigate(typeof(FinishedItem), currentUser);
        }
        //跳转至好友页面
        private void friendsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Friends), currentUser);
        }

        private void finishedClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FinishedItem), currentUser);
        }

        //点击跳转至该Item的ItemInfo页面
        private void helpListViewItemClick(object sender, ItemClickEventArgs e)
        {
            currentUser.currentTarget = ((Models.HelpItem)(e.ClickedItem)).itemId;
            Frame.Navigate(typeof(ItemInfo), currentUser);
        }

        //跳转至主页面
        private void homeClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), currentUser);
        }

        //打开汉堡菜单
        private void hanburgButtonClick(object sender, RoutedEventArgs e)
        {
            hanburgView.IsPaneOpen = !hanburgView.IsPaneOpen;
        }
        ObservableCollection<Models.NavItem> menuItems = new ObservableCollection<Models.NavItem>()
        {
            new Models.NavItem() { Icon=Symbol.Home,Text="Home"},
             new Models.NavItem() { Icon=Symbol.Bullets,Text="Finished Item"},
              new Models.NavItem() { Icon=Symbol.ContactInfo,Text="Friends"},
               new Models.NavItem() { Icon=Symbol.Add,Text="Create Task"},
               new Models.NavItem() {Icon=Symbol.Refresh, Text="Refresh" },
               new Models.NavItem() {Icon=Symbol.Pictures, Text="Change Background" }
        };

        private void hanburgListViewItemClick(object sender, ItemClickEventArgs e)
        {
            switch ((e.ClickedItem as Models.NavItem).Text)
            {
                case "Home":
                    this.homeClick(null, null);
                    break;
                case "Finished Item":
                    this.finishedClick(null, null);
                    break;
                case "Friends":
                    friendsClick(null, null);
                    break;
                case "Create Task":
                    createTask(null, null);
                    break;
                case "Refresh":
                    refreshClick(null, null);
                    break;
                case "Change Background":
                    changeBackground();
                    break;
            }
        }

        private async void changeBackground()
        {

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            var uri = "";
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var s = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                DataReader read = new DataReader(s.GetInputStreamAt(0));
                await read.LoadAsync((uint)s.Size);

                var temp = new byte[s.Size];
                read.ReadBytes(temp);   //图片的二进制流

                string picname = Guid.NewGuid().ToString() + ".jpg";
                StorageFile picfile = await ApplicationData.Current.LocalFolder.CreateFileAsync(picname);
                await FileIO.WriteBytesAsync(picfile, temp);  //将图片的二进制流放在本地文件
                uri = "ms-appdata:///local/" + picname;
                ImageSource b = new BitmapImage(new Uri(uri, UriKind.Absolute));

                ImageBrush im = new ImageBrush();
                im.ImageSource = b;
                currentUser.background = b;
                bg.Background = im;

            }
            else
            {
                uri = "ms-appx:///Assets/SignupBackground.jpg";
            }


            using (HttpClient client = new HttpClient())
            {
                var kvp = new List<KeyValuePair<string, string>>
                {
                     new KeyValuePair<string, string>("background", uri)
                };
                HttpResponseMessage response = await client.PostAsync("http://paradox5.cn/userInfo" + "?userId=" + currentUser.userId, new FormUrlEncodedContent(kvp));
            }
        }

    }
}

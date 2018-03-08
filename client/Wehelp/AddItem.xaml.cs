using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

/*
 * 实现对任务的发布，向服务器post一个数据过去，发布任务前会对任务信息进行检查，判断所有项是否为空，location可以为空
 * commission必须大于等于0，在选择type后，任务图片自动随着type类型的改变
 * 每个页面都可以通过汉堡菜单跳转到各个页面和背景的改变
 * 
 * By 许慕欣
 */


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
            
            if (e.Parameter.GetType() == typeof(Models.Users))
            {
                currentUser = (Models.Users)(e.Parameter);
            } else
            {
                return;
            }



            ImageBrush im = new ImageBrush();
            im.ImageSource = currentUser.background;
            bg.Background = im;

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
                task_image.ImageSource = b;
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
                    // 默认type为第一个
                    if (type.SelectedIndex == -1)
                        type.SelectedIndex = 0;
                    var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("itemId", Guid.NewGuid().ToString()),
                    new KeyValuePair<string,string>("publisherId", publisher.Text),
                    new KeyValuePair<string,string>("type", (type.Items[type.SelectedIndex] as ComboBoxItem).Content.ToString() ),
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
            // 变回默认type
            type.PlaceholderText = "";
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

        // 选择type后更新预览图片
        private void typeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var content = (type.Items[type.SelectedIndex] as ComboBoxItem).Content.ToString() ;
            task_image.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/ItemType/" 
                + content + ".png"));

            return;
        }

    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class Friends : Page
    {
        public Friends()
        {
            this.InitializeComponent();
            this.FriendItemViewModel = new ViewModels.UserItemViewModel();
        }

        ViewModels.UserItemViewModel FriendItemViewModel { get; set; }
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
            if (currentUser.localsource != null)
                touxiang.ImageSource = currentUser.localsource;
            username.Text = currentUser.userName;
            score.Text = currentUser.score.ToString();

            askForFriendsInfo();
        }


        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        private async void askForFriendsInfo()
        {
            using (HttpClient client = new HttpClient())
            {
                var kvp = new List<KeyValuePair<string, string>>
                {
                     new KeyValuePair<string,string>("userId",currentUser.userId),
                };
                HttpResponseMessage response = await client.GetAsync("http://paradox5.cn/friends" + "?userId=" + currentUser.userId);
                string responseBody = await response.Content.ReadAsStringAsync();
                JObject jo = (JObject)JsonConvert.DeserializeObject(responseBody);
                if (jo["statusCode"].ToString() == "ok")
                {

                    if (jo["list"] != null)
                    {
                        foreach (JToken o in jo["list"].Children())
                        {
                            String username = o["userName"].ToString();
                            String score = o["score"].ToString();
                            string image = o["headSculpture"].ToString();

                            byte[] temp = getBytes(image);

                            string picname = Guid.NewGuid().ToString() + ".jpg";

                            StorageFile picfile = await localFolder.CreateFileAsync(picname);

                            await FileIO.WriteBytesAsync(picfile, temp);

                            ImageSource b = new BitmapImage(new Uri("ms-appdata:///local/" + picname, UriKind.Absolute));

                            FriendItemViewModel.AddFriend(username, int.Parse(score), image, b);
                        }
                    }


                }
                else
                {
                   
                }
            }
        }

        private byte[] getBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private string getString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        private async void addFriendsClick(object sender, RoutedEventArgs e)
        {
            string friendName = friendname.Text;
            using (HttpClient client = new HttpClient())
            {
                var kvp = new List<KeyValuePair<string, string>>
                {
                     new KeyValuePair<string,string>("userId",currentUser.userId),
                     new KeyValuePair<string,string>("userName",friendName)
                };
                HttpResponseMessage response = await client.PostAsync("http://paradox5.cn/friends?userId=" + currentUser.userId, new FormUrlEncodedContent(kvp));
                string responseBody = await response.Content.ReadAsStringAsync();
                JObject jo = (JObject)JsonConvert.DeserializeObject(responseBody);
                if (jo["statusCode"].ToString() == "ok")
                {
                    // 同步User?
                    if (currentUser.friendsId == "")
                    {
                        currentUser.friendsId += friendName;
                    }
                    else
                    {
                        currentUser.friendsId += "&" + friendName;
                    }

                    // 刷新好友页面
                    Frame.Navigate(typeof(Friends), currentUser);
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
            Frame.Navigate(typeof(Friends), currentUser);
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

        private void homeClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), currentUser);
        }
    }
}

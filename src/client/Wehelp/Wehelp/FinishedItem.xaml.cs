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
        protected override async void OnNavigatedTo(NavigationEventArgs e)
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
            clientName.Text = currentUser.userName;
            marks.Text = (currentUser.score).ToString();
            if (currentUser.localsource  != null)
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

    }
}

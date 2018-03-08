﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
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
 * 用于显示任务的详情，可以接受任务，若用户不为接受者
 * 可以删除任务，若该用户为发布者
 * 每个页面都可以通过汉堡菜单跳转到各个页面和背景的改变
 * 
 * By 许琦
 */


namespace Wehelp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ItemInfo : Page
    {
        public ItemInfo()
        {
            this.InitializeComponent();
        }

        Models.Users currentUser { set; get; }
        string publisherId;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (e.Parameter.GetType() == typeof(Models.Users))
            {
                var presentUser = (Models.Users)(e.Parameter);
                currentUser = presentUser;
                string errorMessage = "";

                ImageBrush im = new ImageBrush();
                im.ImageSource = currentUser.background;
                bg.Background = im;

                if (currentUser.userId != publisherId)
                {
                    accpetButton.IsEnabled = false;
                }

                if (currentUser.userId == publisherId)
                {
                    accpetButton.IsEnabled = false;
                }

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync("http://paradox5.cn/initItemInfo?itemId=" + presentUser.currentTarget);
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseJson = JsonConvert.DeserializeObject<JObject>(responseBody);
                        var currentItem = responseJson["list"];
                        errorMessage = responseJson["statusCode"].ToString();
                        if (responseJson["statusCode"].ToString() == "ok")
                        {
                            task_image.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/ItemType/" + currentItem["type"].ToString() + ".png"));
                            publisherId = currentItem["publisherId"].ToString();
                            publisher.Text = currentItem["publisherName"].ToString();
                            type.Text = currentItem["type"].ToString();
                            location.Text = currentItem["location"].ToString();
                            time.Text = currentItem["dateTime"].ToString();
                            commission.Text = currentItem["commission"].ToString();
                            details.Text = currentItem["details"].ToString();
                            
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
                username.Text = currentUser.userName;
                score.Text = currentUser.score.ToString();
                if (currentUser.localsource != null)
                    touxiang.ImageSource = currentUser.localsource;
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
            Frame.Navigate(typeof(ItemInfo), currentUser);

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
        //只有自己发布的才能删除
        private async void deleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentUser.userId != publisherId)
            {
                accpetButton.IsEnabled = false;
            }
            else
            {
                string errorMessage = "";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("itemId", currentUser.currentTarget)
                };
                        HttpResponseMessage response = await client.PostAsync("http://paradox5.cn/deleteItem?userId=" + currentUser.userId,
                            new FormUrlEncodedContent(kvp));
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseJson = JsonConvert.DeserializeObject<JObject>(responseBody);
                        errorMessage = responseJson["statusCode"].ToString();

                        if (responseJson["statusCode"].ToString() == "ok")
                        {


                            currentUser.itemId = currentUser.itemId.Replace(currentUser.currentTarget + "&", "");
                            currentUser.itemId = currentUser.itemId.Replace(currentUser.currentTarget, "");
                            currentUser.currentTarget = null;
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
                // delete item
            }
        }
        //同理，只有别人发布的才能接受
        private async void accpetButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentUser.userId == publisherId)
            {
                accpetButton.IsEnabled = false;
            }
            else
            {
                string errorMessage = "";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("itemId", currentUser.currentTarget)
                };
                        HttpResponseMessage response = await client.PostAsync("http://paradox5.cn/main?userId=" + currentUser.userId,
                            new FormUrlEncodedContent(kvp));
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responceJson = JsonConvert.DeserializeObject<JObject>(responseBody);
                        errorMessage = responceJson["statusCode"].ToString();

                        if (responceJson["statusCode"].ToString() == "ok")
                        {
                            currentUser.currentTarget = null;
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
                // accept item
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
    }
}

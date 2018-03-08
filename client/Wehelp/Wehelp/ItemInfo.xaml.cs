using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

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
    }
}

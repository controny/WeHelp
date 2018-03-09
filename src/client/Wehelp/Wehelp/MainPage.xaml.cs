
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
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Wehelp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            helpItemViewModel = new ViewModels.HelpItemViewModel();
        }
        private ViewModels.HelpItemViewModel helpItemViewModel { get; set; }
        Models.Users currentUers { get; set; }
        bool IsActive = false;
        string address = "http://paradox5.cn/";

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.DataRequested);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(DataRequested);
            if (e.Parameter.GetType() == typeof(Models.Users))
            {
                var presentUser = (Models.Users)(e.Parameter);
                currentUers = presentUser;
                username.Text = presentUser.userName;
                score.Text = presentUser.score.ToString();
                string errorMessage = "";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync("http://paradox5.cn/main?userId=" + presentUser.userId);
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseJson = JsonConvert.DeserializeObject<JObject>(responseBody);
                        errorMessage = responseJson["statusCode"].ToString();
                        JArray items = JsonConvert.DeserializeObject<JArray>(responseJson["list"].ToString());
                        if (responseJson["statusCode"].ToString() == "ok")
                        {
                            for (int i = 0; i < items.Count; ++i)
                            {
                                JObject presentItem = (JObject)items[i];
                                string publisherid = presentItem["publisherId"].ToString();
                                string ty = presentItem["type"].ToString();
                                string detail = presentItem["details"].ToString();
                                string location = presentItem["location"].ToString();
                                string date = presentItem["dateTime"].ToString();
                                string itemid = presentItem["itemId"].ToString();
                                string image = "";
                                int c;
                                try
                                {
                                    c = int.Parse(presentItem["commission"].ToString());
                                }
                                catch(Exception ex)
                                {
                                    c = 0;
                                }
                                helpItemViewModel.AddHelpItem(itemid, publisherid, "", ty, detail, location, date, image, false, c);

                            }

                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        await new MessageDialog(errorMessage).ShowAsync();
                    }
                    username.Text = currentUers.userName;
                    score.Text = (currentUers.score).ToString();
                    if (currentUers.localsource != null)
                        touxiang.ImageSource = currentUers.localsource;
                }
                UpdatePrimaryTile();

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

        string Shared_content;


        private void share_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
            MenuFlyoutItem click = sender as MenuFlyoutItem;
            if (click != null)
            {
                Models.HelpItem now = click.DataContext as Models.HelpItem;
                if (now != null)
                {
                    Shared_content = "";
                    Shared_content += now.publisherId + " have a " + now.type + "task" + " whose details are" + now.details + ". And it is worth " + now.commission + " scores.";

                }
            }
        }

        private void DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            var db = request.Data;
            var deferral = request.GetDeferral();

            //var photo = await StorageFile.GetFileFromApplicationUriAsync(new Uri(Shared_img.UriSource.ToString()));
            //db.SetStorageItems(new List<StorageFile> { photo });
            request.Data.Properties.Title = "Share Text Example";
            if (Shared_content != null)
            request.Data.SetText(Shared_content);
            deferral.Complete();
        }

        //点击setting，跳转至userInfo页面
        private void userSettingClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserInfo), currentUers);
        }
        //点击登出，回到signin页面
        private void signoutClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Signin));
        }
        //跳转至新建Item页面
        private void createTask(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddItem), currentUers);
        }
        //刷新本页面
        private void refreshClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), currentUers);
        }
        //跳转至好友页面
        private void friendsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Friends), currentUers);
        }
        //跳转至已完成页面
        private void finishedClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FinishedItem), currentUers);
        }



        //点击跳转至该Item的ItemInfo页面
        private void helpListViewItemClick(object sender, ItemClickEventArgs e)
        {
            helpItemViewModel.SelectedItem = (Models.HelpItem)(e.ClickedItem);
            currentUers.currentTarget = helpItemViewModel.SelectedItem.itemId;
            if (InlineHelpItemViewGrid.Visibility == Visibility.Visible)
            {
                Models.HelpItem now = helpItemViewModel.SelectedItem;
                publisher.Text = now.publisherId;
                title.Text = now.type;
                location.Text = now.location;
                time.Text = now.dateTime;
                commission.Text = now.commission.ToString();
                details.Text = now.details;
            }
            else
            {   
                Frame.Navigate(typeof(ItemInfo), currentUers);
            }

        }

        //只有别人发的才能接受，自己的不行


        private async void inlineAccpetButtonClick(object sender, RoutedEventArgs e)
        {   if (helpItemViewModel.SelectedItem == null) return;
            if (helpItemViewModel.SelectedItem.publisherId == currentUers.userId)
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
                    new KeyValuePair<string,string>("itemId", helpItemViewModel.SelectedItem.itemId)
                };
                        HttpResponseMessage response = await client.PostAsync("http://paradox5.cn/main?userId=" + currentUers.userId,
                            new FormUrlEncodedContent(kvp));
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responceJson = JsonConvert.DeserializeObject<JObject>(responseBody);
                        errorMessage = responceJson["statusCode"].ToString();

                        if (responceJson["statusCode"].ToString() == "ok")
                        {
                            currentUers.score += helpItemViewModel.SelectedItem.commission;
                            helpItemViewModel.RemoveHelpItem(helpItemViewModel.SelectedItem.itemId);
                            helpItemViewModel.SelectedItem = null;
                            currentUers.currentTarget = null;
                            Frame.Navigate(typeof(MainPage), currentUers);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        await new MessageDialog(errorMessage).ShowAsync();
                    }
                }

            }
        }

        //只有自己发布的才能删除
        private async void inlineDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (helpItemViewModel.SelectedItem == null) return;
            if (helpItemViewModel.SelectedItem.publisherId != currentUers.userId)
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
                    new KeyValuePair<string,string>("itemId", helpItemViewModel.SelectedItem.itemId)
                };
                        HttpResponseMessage response = await client.PostAsync("http://paradox5.cn/deleteItem?userId=" + currentUers.userId,
                            new FormUrlEncodedContent(kvp));
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responceJson = JsonConvert.DeserializeObject<JObject>(responseBody);
                        errorMessage = responceJson["statusCode"].ToString();

                        if (responceJson["statusCode"].ToString() == "ok")
                        {

                            helpItemViewModel.RemoveHelpItem(helpItemViewModel.SelectedItem.itemId);
                            currentUers.itemId = currentUers.itemId.Replace(currentUers.currentTarget + "&", "");
                            currentUers.itemId = currentUers.itemId.Replace(currentUers.currentTarget, "");

                            helpItemViewModel.SelectedItem = null;
                            currentUers.currentTarget = null;
                            Frame.Navigate(typeof(MainPage), currentUers);
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
        private void UpdatePrimaryTile()
        {
            System.Xml.Linq.XDocument xdoc = System.Xml.Linq.XDocument.Load("tiles.xml");

            var updator = TileUpdateManager.CreateTileUpdaterForApplication();
            updator.EnableNotificationQueueForSquare150x150(true);
            updator.EnableNotificationQueueForSquare310x310(true);
            updator.EnableNotificationQueueForWide310x150(true);
            updator.EnableNotificationQueue(true);
            updator.Clear();
            var n = helpItemViewModel.HelpItemSize();
            var index = 0;
            if (n >= 5) index = n - 5;
            else index = 0;
            for (var i = n - 1; i >= index; i--)
            {
                XmlDocument tilexml = new XmlDocument();
                tilexml.LoadXml(xdoc.ToString());
                XmlNodeList titles = tilexml.GetElementsByTagName("text");
                titles[0].InnerText = helpItemViewModel.AllItems[i].type;
                titles[1].InnerText = helpItemViewModel.AllItems[i].type;
                titles[3].InnerText = helpItemViewModel.AllItems[i].type;
                titles[6].InnerText = helpItemViewModel.AllItems[i].type;
                titles[2].InnerText = helpItemViewModel.AllItems[i].details;
                titles[4].InnerText = helpItemViewModel.AllItems[i].details;
                titles[7].InnerText = helpItemViewModel.AllItems[i].details;
                titles[5].InnerText = helpItemViewModel.AllItems[i].dateTime.ToString();
                titles[8].InnerText = helpItemViewModel.AllItems[i].dateTime.ToString();

                /*BitmapImage b = (BitmapImage)touxiang.ImageSource;
                var images = tilexml.GetElementsByTagName("image");
                if (images != null)
                {
                    var image = images[0] as XmlElement; if (b != null) image.SetAttribute("src", b.UriSource.ToString());
                    image = images[1] as XmlElement; if (b != null) image.SetAttribute("src", b.UriSource.ToString());
                    image = images[2] as XmlElement; if (b != null) image.SetAttribute("src", b.UriSource.ToString());
                    image = images[3] as XmlElement; if (b != null) image.SetAttribute("src", b.UriSource.ToString());
                }*/
                TileNotification notification = new TileNotification(tilexml);
                updator.Update(notification);
            }
        }
        //同理，只有别人发布的才能接受
        private async void accpetClick(object sender, RoutedEventArgs e)
        {
            
            MenuFlyoutItem click = sender as MenuFlyoutItem;
            if (click != null)
            {
                helpItemViewModel.SelectedItem = click.DataContext as Models.HelpItem;
                if (helpItemViewModel.SelectedItem.publisherId == currentUers.userId)
                {
                    var i = new MessageDialog("You can't accept the task!").ShowAsync();
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
                    new KeyValuePair<string,string>("itemId", helpItemViewModel.SelectedItem.itemId)
                };
                            HttpResponseMessage response = await client.PostAsync("http://paradox5.cn/main?userId=" + currentUers.userId,
                                new FormUrlEncodedContent(kvp));
                            string responseBody = await response.Content.ReadAsStringAsync();
                            var responceJson = JsonConvert.DeserializeObject<JObject>(responseBody);
                            errorMessage = responceJson["statusCode"].ToString();

                            if (responceJson["statusCode"].ToString() == "ok")
                            {
                                currentUers.score += helpItemViewModel.SelectedItem.commission;
                                helpItemViewModel.RemoveHelpItem(helpItemViewModel.SelectedItem.itemId);
                                helpItemViewModel.SelectedItem = null;
                                currentUers.currentTarget = null;
                                Frame.Navigate(typeof(MainPage), currentUers);
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            await new MessageDialog(errorMessage).ShowAsync();
                        }
                    }
                    // add item
                }
            }

        }


    }
}


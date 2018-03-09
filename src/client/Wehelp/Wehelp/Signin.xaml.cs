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
    public sealed partial class Signin : Page
    {
        public Signin()
        {
            this.InitializeComponent();
        }

        private async void signinClick(object sender, RoutedEventArgs e)
        {
            string errorMessage = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("userName", username.Text),
                    new KeyValuePair<string,string>("password", password.Password)
                };
                    HttpResponseMessage response = await client.PostAsync("http://paradox5.cn/signin", new FormUrlEncodedContent(kvp));
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var responceJson1 = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var responceJson = responceJson1["list"];
                    errorMessage = responceJson1["statusCode"].ToString();
                    if (responceJson1["statusCode"].ToString() == "ok")
                    {
                        var head = responceJson["headSculpture"];
                        var itId = responceJson["itemId"];
                        var fId = responceJson["friendsId"];
                        var headString = head == null ? "" : head.ToString();
                        var itIdString = itId == null ? "" : itId.ToString();
                        var fIdString = fId == null ? "" : fId.ToString();
                        var phone = responceJson["phone"].ToString();
                        Models.Users regster = new Models.Users(responceJson["userId"].ToString(), responceJson["userName"].ToString(),
                        responceJson["password"].ToString(), headString,
                        itIdString, fIdString, int.Parse(responceJson["score"].ToString()),
                        phone);

                        if (headString !="")
                            regster.localsource = new BitmapImage(new Uri(headString));
                        else regster.localsource = null;
                        Frame.Navigate(typeof(MainPage), regster);

                    }
                    else
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

        private void signupClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Signup));
        }
    }
}

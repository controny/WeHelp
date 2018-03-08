using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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

/*
 * 注册页面，想服务器发送请求，检查该用户是否已被注册
 * 若未被注册，则给服务器发送该用户的默认信息
 * 若被注册，则返还错误信息
 * 发送信息前，检查电话号码是否符合要求 1开头，全为数字，总共11位
 * 
 * 
 * By 许琦 许慕欣
 */



namespace Wehelp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Signup : Page
    {
        public Signup()
        {
            this.InitializeComponent();
        }

        ViewModels.UserItemViewModel all;

        private void cancelClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Signup));
        }
        private async void submitClick(object sender, RoutedEventArgs e)
        {
            string errorMessage = "";
            string userId = Guid.NewGuid().ToString();
            if (password.Password != comfirmPassword.Password)
            {
                await new MessageDialog("Please input the same password at the second time").ShowAsync();
                return;
            }
            if (!Regex.IsMatch(phone.Text, "^1\\d{10}$"))
            {
                await new MessageDialog("Invalid phone!").ShowAsync();
                return;
            }
                using (HttpClient client = new HttpClient())
            {
                try
                {
                    var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("userName", username.Text),
                    new KeyValuePair<string,string>("password", password.Password),
                    new KeyValuePair<string,string>("userId", username.Text),
                    new KeyValuePair<string,string>("itemId", ""),
                    new KeyValuePair<string,string>("friendsId", ""),
                    new KeyValuePair<string,string>("score", "0"),
                    new KeyValuePair<string,string>("headSculpture", "ms-appx:///Assets/boy/1.png"),
                    new KeyValuePair<string, string>("phone",phone.Text),

                    new KeyValuePair<string, string>("background", "ms-appx:///Assets/SignupBackground.jpg")



                };
                    HttpResponseMessage response = await client.PostAsync("http://paradox5.cn/signup", new FormUrlEncodedContent(kvp));
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var responceJson = JsonConvert.DeserializeObject<JObject>(responseBody);
                    errorMessage = responceJson["statusCode"].ToString();
                    if (responceJson["statusCode"].ToString() == "ok")
                    {
                        await new MessageDialog("Regist suceessfully!").ShowAsync();
                        Frame.Navigate(typeof(Signin));

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

        private void signinClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Signin));
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
    }
}

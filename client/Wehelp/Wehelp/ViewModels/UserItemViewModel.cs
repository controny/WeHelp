﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Wehelp.ViewModels
{
    class UserItemViewModel
    {
        private ObservableCollection<Models.Users> allItems = new ObservableCollection<Models.Users>();
        public ObservableCollection<Models.Users> AllItems { get { return this.allItems; } }

        private Models.Users _selectedItem = default(Models.Users);
        public Models.Users SelectedItem { get { return _selectedItem; } set { this._selectedItem = value; } }

        public UserItemViewModel()
        {
            this._selectedItem = null;
        }

        public void RegisteUser(string userName, string password, string phone)
        {
            this.allItems.Add(new Models.Users(userName, password, phone));
        }

        public void AddFriend(string friendName, int score, string headSculpture, ImageSource image)
        {
            this.allItems.Add(new Models.Users(friendName, score, headSculpture, image));
        }

        public void LoginUser(string userId, string userName, string password, string headSculpture, string itemId, string friendsId, int score, string phone)
        {
            this.allItems.Add(new Models.Users(userId, userName, password, headSculpture, itemId, friendsId, score,phone));
        }

        public void RemoveAFriend(string id)
        {
            for (int i = 0; i < this.allItems.Count; i++)
            {
                if (allItems[i].userId == id)
                {
                    allItems.RemoveAt(i);
                    break;
                }
            }
            this._selectedItem = null;
        }

        
    }
}

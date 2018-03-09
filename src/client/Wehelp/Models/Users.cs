using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;


/*
 * Users表示一个用户
 * 每个用户有一个独立的ID和username
 * itemId表示该用户发布的任务ID，headSculpture表示该用户的头像
 * friendsId表示该用户的朋友，score为该用户的当前积分
 * currenTarget代表当前用户用鼠标选中的Item
 * background用于存放该用户使用的背景图片
 * 
 * By 许琦
 */ 
namespace Wehelp.Models
{
    class Users : INotifyPropertyChanged
    {
        private string _userId;
        private string _userName;
        private string _password;
        private string _headSculpture;
        private string _itemId;
        private string _friendsId;
        private int _score;
        private string _currentTarget;
        private ImageSource _localsource;
        private string _phone;
        private ImageSource _background;



        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string phone
        {
            get { return _phone; }
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    RaisePropertyChanged("phone");
                }
            }
        }

        public ImageSource background
        {
            get { return _background; }
            set
            {
                if (_background != value)
                {
                    _background = value;
                    RaisePropertyChanged("background");
                }
            }
        }

        public ImageSource localsource
        {
            get { return _localsource; }
            set
            {
                if (_localsource != value)
                {
                    _localsource = value;
                    RaisePropertyChanged("localsource");
                }
            }
        }

        public string userId
        {
            get { return _userId; }
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    RaisePropertyChanged("userId");
                }
            }
        }

        public string userName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    RaisePropertyChanged("userName");
                }
            }
        }

        public string password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged("password");
                }
            }
        }

        public string headSculpture
        {
            get { return _headSculpture; }
            set
            {
                if (_headSculpture != value)
                {
                    _headSculpture = value;
                    RaisePropertyChanged("headSculpture");
                }
            }
        }

        public string itemId
        {
            get { return _itemId; }
            set
            {
                if (_itemId != value)
                {
                    _itemId = value;
                    RaisePropertyChanged("itemId");
                }
            }
        }

        public string friendsId
        {
            get { return _friendsId; }
            set
            {
                if (_friendsId != value)
                {
                    _friendsId = value;
                    RaisePropertyChanged("friendsId");
                }
            }
        }

        public int score
        {
            get { return _score; }
            set
            {
                if (_score != value)
                {
                    _score = value;
                    RaisePropertyChanged("score");
                }
            }
        }

        public string currentTarget
        {
            get { return _currentTarget; }
            set
            {
                if (_currentTarget != value)
                {
                    _currentTarget = value;
                    RaisePropertyChanged("currentTarget");
                }
            }
        }

        public Users(string userName, string password, string phone)
        {
            this._userId = Guid.NewGuid().ToString();
            this._password = password;
            this._headSculpture = null;
            this._itemId = null;
            this._friendsId = null;
            this._score = 0;
            this._phone = phone;
        }  // 在前端注册用户时

        public Users(string userId, string userName, string password, string headSculpture, string itemId, string friendsId, int score,string phone)
        {
            this._userName = userName;
            this._userId = userId;
            this._password = password;
            this._headSculpture = headSculpture;
            this._itemId = itemId;
            this._friendsId = friendsId;
            this._score = score;
            this._phone = phone;
        }  // 从服务器根据登陆用户的id获取参数信息

        public Users(string friendName, int score, string headSculpture, ImageSource localsource)
        {
            this._userName = friendName;
            this._score = score;
            this._headSculpture = headSculpture;
            this._localsource = localsource;
        }   // 登陆用户的朋友
    }
}

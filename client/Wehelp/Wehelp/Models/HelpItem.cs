using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Wehelp.Models
{
    class HelpItem : INotifyPropertyChanged
    {
        private string _itemId;
        private string _publisherId;
        private string _consummatorId;
        private string _type;
        private string _details;
        private string _location;
        private string _dateTime;
        private string _image;
        private bool _isCompleted;
        private int _commission;
        private string _sn;
        private string _cn;
        private ImageSource _localsource;


        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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

        public string publisherId
        {
            get { return _publisherId; }
            set
            {
                if (_publisherId != value)
                {
                    _publisherId = value;
                    RaisePropertyChanged("publisherId");
                }
            }
        }

        public string consummatorId
        {
            get { return _consummatorId; }
            set
            {
                if (_consummatorId != value)
                {
                    _consummatorId = value;
                    RaisePropertyChanged("consummatorId");
                }
            }
        }

        public string image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    RaisePropertyChanged("image");
                }
            }
        }

        public string type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    RaisePropertyChanged("type");
                }
            }
        }

        public string details
        {
            get { return _details; }
            set
            {
                if (_details != value)
                {
                    _details = value;
                    RaisePropertyChanged("details");
                }
            }
        }

        public string location
        {
            get { return _location; }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    RaisePropertyChanged("location");
                }
            }
        }

        public string dateTime
        {
            get { return _dateTime; }
            set
            {
                if (_dateTime != value)
                {
                    _dateTime = value;
                    RaisePropertyChanged("dateTime");
                }
            }
        }

        public int commission
        {
            get { return _commission; }
            set
            {
                if (_commission != value)
                {
                    _commission = value;
                    RaisePropertyChanged("commission");
                }
            }
        }

        public bool isCompleted
        {
            get { return _isCompleted; }
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    RaisePropertyChanged("isCompleted");
                }
            }
        }

        public string sn
        {
            get { return _sn; }
            set
            {
                if (_sn != value)
                {
                    _sn = value;
                    RaisePropertyChanged("sn");
                }
            }
        }

        public string cn
        {
            get { return _cn; }
            set
            {
                if (_cn != value)
                {
                    _cn = value;
                    RaisePropertyChanged("cn");
                }
            }
        }
        public HelpItem(string publisherId, string type, string details, string location, string dateTime
            , string image, int commission, string itemid)
        {
            this._itemId = Guid.NewGuid().ToString();
            this._publisherId = publisherId;
            this._type = type;
            this._details = details;
            this._location = location;
            this._dateTime = dateTime;
            this._image = image;
            this._isCompleted = false;
            this._commission = commission;
            this._itemId = itemId;
        }

        public HelpItem(string itemId, string publisherId, string consummatorId, string type, string details, string location, string dateTime
            , string image, bool isCompleted, int commission)
        {
            this._itemId = itemId;
            this._publisherId = publisherId;
            this._consummatorId = consummatorId;
            this._type = type;
            this._details = details;
            this._location = location;
            this._dateTime = dateTime;
            this._image = image;
            this._isCompleted = isCompleted;
            this._commission = commission;
        }

        public HelpItem(string sn, string cn, string location, string type, int commission,string itemId)
        {
            this._itemId = itemId;
            this._publisherId = publisherId;
            this._consummatorId = consummatorId;
            this._type = type;
            this._details = details;
            this._location = location;
            this._dateTime = dateTime;
            this._image = image;
            this._isCompleted = isCompleted;
            this._commission = commission;
            this._sn = sn;
            this._cn = cn;
        }

    }
}

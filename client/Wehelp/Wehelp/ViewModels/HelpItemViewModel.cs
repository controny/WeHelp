using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Wehelp.ViewModels
{
    class HelpItemViewModel
    {
        private ObservableCollection<Models.HelpItem> allItems = new ObservableCollection<Models.HelpItem>();
        public ObservableCollection<Models.HelpItem> AllItems { get { return this.allItems; } }

        private Models.HelpItem _selectedItem = default(Models.HelpItem);
        public Models.HelpItem SelectedItem { get { return _selectedItem; } set { this._selectedItem = value; } }

        private int _itemCount;

        public int HelpItemSize()
        {
            return _itemCount;
        }
        public HelpItemViewModel()
        {
            this._selectedItem = null;
            this._itemCount = 0;
        }

        public void AddHelpItem(string itemId, string publisherId, string consummatorId, string type, string details, string location, string dateTime
            , string image, bool isCompleted, int commission)
        {
            this.allItems.Add(new Models.HelpItem(itemId, publisherId, consummatorId, type, details, location, dateTime, image, isCompleted, commission));
            _itemCount++;
        } 

        public void AddHelpItem(string publisherId, string type, string details, string location, string dateTime
            , string image, int commission, string itemid)
        {
            Models.HelpItem newOne = new Models.HelpItem(publisherId, type, details, location, dateTime, image, commission, itemid);
            this.allItems.Add(newOne);
            _itemCount++;
        }  // mainpagehelp

        public void AddFinishedItem(string sn, string cn, string location, string type, int commission,string itemid)
        {
            Models.HelpItem newOne = new Models.HelpItem(sn, cn, location, type, commission,itemid);
            this.allItems.Add(newOne);
        }

        public void RemoveHelpItem(string id)
        {
            for (int i = 0; i < this.allItems.Count; i++)
            {
                if (allItems[i].itemId == id)
                {
                    allItems.RemoveAt(i);
                    break;
                }
            }
            _itemCount--;
            // set selectedItem to null after remove
            this._selectedItem = null;
        }
    }
}

﻿#pragma checksum "D:\UWP_Cocos\MidtermProject\源代码\客户端\Wehelp\ItemInfo.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "59750666091DFD2EE452C8B5DD286A9D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wehelp
{
    partial class ItemInfo : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        internal class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(global::Windows.UI.Xaml.Controls.ItemsControl obj, global::System.Object value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Object) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Object), targetNullValue);
                }
                obj.ItemsSource = value;
            }
        };

        private class ItemInfo_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IItemInfo_Bindings
        {
            private global::Wehelp.ItemInfo dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.ListView obj5;

            public ItemInfo_obj1_Bindings()
            {
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 5:
                        this.obj5 = (global::Windows.UI.Xaml.Controls.ListView)target;
                        break;
                    default:
                        break;
                }
            }

            // IItemInfo_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
            }

            // ItemInfo_obj1_Bindings

            public void SetDataRoot(global::Wehelp.ItemInfo newDataRoot)
            {
                this.dataRoot = newDataRoot;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::Wehelp.ItemInfo obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_menuItems(obj.menuItems, phase);
                    }
                }
            }
            private void Update_menuItems(global::System.Collections.ObjectModel.ObservableCollection<global::Wehelp.Models.NavItem> obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(this.obj5, obj, null);
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2:
                {
                    this.bg = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3:
                {
                    this.hanburgView = (global::Windows.UI.Xaml.Controls.SplitView)(target);
                }
                break;
            case 4:
                {
                    this.splitViewToggle = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 82 "..\..\..\ItemInfo.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.splitViewToggle).Click += this.hanburgButtonClick;
                    #line default
                }
                break;
            case 5:
                {
                    this.hanburgListView = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    #line 28 "..\..\..\ItemInfo.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListView)this.hanburgListView).ItemClick += this.hanburgListViewItemClick;
                    #line default
                }
                break;
            case 6:
                {
                    this.publisher = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 7:
                {
                    this.type = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 8:
                {
                    this.location = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 9:
                {
                    this.time = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 10:
                {
                    this.commission = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 11:
                {
                    this.details = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 12:
                {
                    this.deleteButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 74 "..\..\..\ItemInfo.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.deleteButton).Click += this.deleteButtonClick;
                    #line default
                }
                break;
            case 13:
                {
                    this.accpetButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 75 "..\..\..\ItemInfo.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.accpetButton).Click += this.accpetButtonClick;
                    #line default
                }
                break;
            case 14:
                {
                    this.task_image = (global::Windows.UI.Xaml.Media.ImageBrush)(target);
                }
                break;
            case 15:
                {
                    this.username = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 16:
                {
                    this.score = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 17:
                {
                    this.userSetting = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 56 "..\..\..\ItemInfo.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.userSetting).Click += this.userSettingClick;
                    #line default
                }
                break;
            case 18:
                {
                    global::Windows.UI.Xaml.Controls.Button element18 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 57 "..\..\..\ItemInfo.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element18).Click += this.signoutClick;
                    #line default
                }
                break;
            case 19:
                {
                    this.touxiang = (global::Windows.UI.Xaml.Media.ImageBrush)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)target;
                    ItemInfo_obj1_Bindings bindings = new ItemInfo_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                }
                break;
            }
            return returnValue;
        }
    }
}


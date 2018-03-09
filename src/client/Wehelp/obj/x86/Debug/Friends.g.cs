﻿#pragma checksum "D:\UWP_Cocos\MidtermProject\源代码\客户端\Wehelp\Friends.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7121F2B855CBE3620781FEFB933A4C6E"
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
    partial class Friends : 
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
            public static void Set_Windows_UI_Xaml_Controls_TextBlock_Text(global::Windows.UI.Xaml.Controls.TextBlock obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.Text = value ?? global::System.String.Empty;
            }
            public static void Set_Windows_UI_Xaml_Media_ImageBrush_ImageSource(global::Windows.UI.Xaml.Media.ImageBrush obj, global::Windows.UI.Xaml.Media.ImageSource value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::Windows.UI.Xaml.Media.ImageSource) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::Windows.UI.Xaml.Media.ImageSource), targetNullValue);
                }
                obj.ImageSource = value;
            }
        };

        private class Friends_obj7_Bindings :
            global::Windows.UI.Xaml.IDataTemplateExtension,
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IFriends_Bindings
        {
            private global::Wehelp.Models.Users dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);
            private global::Windows.UI.Xaml.ResourceDictionary localResources;
            private global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement> converterLookupRoot;
            private bool removedDataContextHandler = false;

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.TextBlock obj8;
            private global::Windows.UI.Xaml.Controls.TextBlock obj9;
            private global::Windows.UI.Xaml.Media.ImageBrush obj10;

            private Friends_obj7_BindingsTracking bindingsTracking;

            public Friends_obj7_Bindings()
            {
                this.bindingsTracking = new Friends_obj7_BindingsTracking(this);
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 8:
                        this.obj8 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 9:
                        this.obj9 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 10:
                        this.obj10 = (global::Windows.UI.Xaml.Media.ImageBrush)target;
                        break;
                    default:
                        break;
                }
            }

            public void DataContextChangedHandler(global::Windows.UI.Xaml.FrameworkElement sender, global::Windows.UI.Xaml.DataContextChangedEventArgs args)
            {
                 global::Wehelp.Models.Users data = args.NewValue as global::Wehelp.Models.Users;
                 if (args.NewValue != null && data == null)
                 {
                    throw new global::System.ArgumentException("Incorrect type passed into template. Based on the x:DataType global::Wehelp.Models.Users was expected.");
                 }
                 this.SetDataRoot(data);
                 this.Update();
            }

            // IDataTemplateExtension

            public bool ProcessBinding(uint phase)
            {
                throw new global::System.NotImplementedException();
            }

            public int ProcessBindings(global::Windows.UI.Xaml.Controls.ContainerContentChangingEventArgs args)
            {
                int nextPhase = -1;
                switch(args.Phase)
                {
                    case 0:
                        nextPhase = -1;
                        this.SetDataRoot(args.Item as global::Wehelp.Models.Users);
                        if (!removedDataContextHandler)
                        {
                            removedDataContextHandler = true;
                            ((global::Windows.UI.Xaml.Controls.UserControl)args.ItemContainer.ContentTemplateRoot).DataContextChanged -= this.DataContextChangedHandler;
                        }
                        this.initialized = true;
                        break;
                }
                this.Update_((global::Wehelp.Models.Users) args.Item, 1 << (int)args.Phase);
                return nextPhase;
            }

            public void ResetTemplate()
            {
                this.bindingsTracking.ReleaseAllListeners();
            }

            // IFriends_Bindings

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
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            // Friends_obj7_Bindings

            public void SetDataRoot(global::Wehelp.Models.Users newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.dataRoot = newDataRoot;
            }
            public void SetConverterLookupRoot(global::Windows.UI.Xaml.FrameworkElement rootElement)
            {
                this.converterLookupRoot = new global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement>(rootElement);
            }

            public global::Windows.UI.Xaml.Data.IValueConverter LookupConverter(string key)
            {
                if (this.localResources == null)
                {
                    global::Windows.UI.Xaml.FrameworkElement rootElement;
                    this.converterLookupRoot.TryGetTarget(out rootElement);
                    this.localResources = rootElement.Resources;
                    this.converterLookupRoot = null;
                }
                return (global::Windows.UI.Xaml.Data.IValueConverter) (this.localResources.ContainsKey(key) ? this.localResources[key] : global::Windows.UI.Xaml.Application.Current.Resources[key]);
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::Wehelp.Models.Users obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_userName(obj.userName, phase);
                        this.Update_score(obj.score, phase);
                    }
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_headSculpture(obj.headSculpture, phase);
                    }
                }
            }
            private void Update_userName(global::System.String obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj8, obj, null);
                }
            }
            private void Update_score(global::System.Int32 obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj9, obj.ToString(), null);
                }
            }
            private void Update_headSculpture(global::System.String obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Media_ImageBrush_ImageSource(this.obj10, (global::Windows.UI.Xaml.Media.ImageSource)this.LookupConverter("fChange").Convert(obj, typeof(global::Windows.UI.Xaml.Media.ImageSource), null, null), null);
                }
            }

            private class Friends_obj7_BindingsTracking
            {
                global::System.WeakReference<Friends_obj7_Bindings> WeakRefToBindingObj; 

                public Friends_obj7_BindingsTracking(Friends_obj7_Bindings obj)
                {
                    WeakRefToBindingObj = new global::System.WeakReference<Friends_obj7_Bindings>(obj);
                }

                public void ReleaseAllListeners()
                {
                    UpdateChildListeners_(null);
                }

                public void PropertyChanged_(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    Friends_obj7_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        string propName = e.PropertyName;
                        global::Wehelp.Models.Users obj = sender as global::Wehelp.Models.Users;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                    bindings.Update_headSculpture(obj.headSculpture, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "headSculpture":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_headSculpture(obj.headSculpture, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                }
                public void UpdateChildListeners_(global::Wehelp.Models.Users obj)
                {
                    Friends_obj7_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        if (bindings.dataRoot != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)bindings.dataRoot).PropertyChanged -= PropertyChanged_;
                        }
                        if (obj != null)
                        {
                            bindings.dataRoot = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_;
                        }
                    }
                }
            }
        }

        private class Friends_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IFriends_Bindings
        {
            private global::Wehelp.Friends dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.ListView obj5;
            private global::Windows.UI.Xaml.Controls.ListView obj6;

            public Friends_obj1_Bindings()
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
                    case 6:
                        this.obj6 = (global::Windows.UI.Xaml.Controls.ListView)target;
                        break;
                    default:
                        break;
                }
            }

            // IFriends_Bindings

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

            // Friends_obj1_Bindings

            public void SetDataRoot(global::Wehelp.Friends newDataRoot)
            {
                this.dataRoot = newDataRoot;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::Wehelp.Friends obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_menuItems(obj.menuItems, phase);
                        this.Update_FriendItemViewModel(obj.FriendItemViewModel, phase);
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
            private void Update_FriendItemViewModel(global::Wehelp.ViewModels.UserItemViewModel obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_FriendItemViewModel_AllItems(obj.AllItems, phase);
                    }
                }
            }
            private void Update_FriendItemViewModel_AllItems(global::System.Collections.ObjectModel.ObservableCollection<global::Wehelp.Models.Users> obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(this.obj6, obj, null);
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
                    #line 101 "..\..\..\Friends.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.splitViewToggle).Click += this.hanburgButtonClick;
                    #line default
                }
                break;
            case 5:
                {
                    this.hanburgListView = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    #line 30 "..\..\..\Friends.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListView)this.hanburgListView).ItemClick += this.hanburgListViewItemClick;
                    #line default
                }
                break;
            case 6:
                {
                    this.FriendsListView = (global::Windows.UI.Xaml.Controls.ListView)(target);
                }
                break;
            case 11:
                {
                    this.friendname = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 12:
                {
                    this.addFriends = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 72 "..\..\..\Friends.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.addFriends).Click += this.addFriendsClick;
                    #line default
                }
                break;
            case 13:
                {
                    this.username = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 14:
                {
                    this.score = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 15:
                {
                    this.userSetting = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 59 "..\..\..\Friends.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.userSetting).Click += this.userSettingClick;
                    #line default
                }
                break;
            case 16:
                {
                    global::Windows.UI.Xaml.Controls.Button element16 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 60 "..\..\..\Friends.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element16).Click += this.signoutClick;
                    #line default
                }
                break;
            case 17:
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
                    Friends_obj1_Bindings bindings = new Friends_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                }
                break;
            case 7:
                {
                    global::Windows.UI.Xaml.Controls.UserControl element7 = (global::Windows.UI.Xaml.Controls.UserControl)target;
                    Friends_obj7_Bindings bindings = new Friends_obj7_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot((global::Wehelp.Models.Users) element7.DataContext);
                    bindings.SetConverterLookupRoot(this);
                    element7.DataContextChanged += bindings.DataContextChangedHandler;
                    global::Windows.UI.Xaml.DataTemplate.SetExtensionInstance(element7, bindings);
                }
                break;
            }
            return returnValue;
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using MVVM_TreeView.Core;

namespace Core
{
    public class ToolBarItemViewModel : ViewModelBase
    {
        #region Fields

        public string DisplayName
        {
            get
            {
                return base.DisplayName;
            }

            set
            {
                base.DisplayName = value;
                base.OnPropertyChanged("DisplayName");
            }
        }

        private ICommand _executeCommand;
        public ICommand ExecuteCommand
        {
            get
            {
                if (_executeCommand == null)
                    _executeCommand = new RelayCommand(OnCommandExecuted);

                return _executeCommand;
            }
        }

        protected bool _isEnabled = true;
        public virtual bool IsEnabled
        {
            get
            {
                //is disabled if the user does not have full access
                //if (!LoginService.CurrentUser.FullAccess)
                //    return false;

                return _isEnabled;
            }

            set
            {
                _isEnabled = value;
                base.OnPropertyChanged("IsEnabled");
            }
        }

        private string _toolTip;
        public string ToolTip
        {
            get
            {
                return _toolTip;
            }

            set
            {
                _toolTip = value;
                base.OnPropertyChanged("ToolTip");
            }
        }

        private string _imageSource;
        /// <summary>
        /// Name of the Icon
        /// Icons located in /Resources/Images/Icons/
        /// set just the name of the icon, without .png
        /// </summary>
        public string ImageSource
        {
            get
            {
                return _imageSource;
            }

            set
            {
#if DEBUG
                if (value != null && Regex.IsMatch(value, @"\/|\."))
                    Debug.Fail(
                        "Icon not found! Icons located in /Resources/Images/Icons/ set just the name of the icon, without .png");
#endif
                _imageSource = String.Format("/Resources/Images/Icons/{0}.png", value);
                base.OnPropertyChanged("Image");
            }
        }

        /// <summary>
        /// Returns a BitmapImage
        /// ImageSource must be set!
        /// </summary>
        public BitmapImage Image
        {
            get
            {
                if (this.ImageSource != null)
                    return new BitmapImage(new Uri(this.ImageSource, UriKind.Relative));

                return null;
            }
        }

        #endregion

        #region Constructor

        public ToolBarItemViewModel(string header, string imageSource)
            : this(header, imageSource, header)
        { }

        public ToolBarItemViewModel(string header, string imageSource, string toolTip)
        {
            base.DisplayName = header;
            this.ImageSource = imageSource;
            this.ToolTip = toolTip;
        }

        #endregion

        #region private Helpers

        protected virtual void OnCommandExecuted()
        {
            this.CommandExecuted(this, EventArgs.Empty);
        }

        #endregion

        #region Events

        public virtual event EventHandler CommandExecuted;

        #endregion

        #region Cleanup

        protected override void OnDispose()
        {
            CommandExecuted = null;
            base.OnDispose();
        }

        #endregion
    }

    /// <summary>
    /// This ToolBarItem is enabled for all users. It will not be disabled if the user has insufficient rights
    /// </summary>
    public class PublicAccessToolBarItemViewModel : ToolBarItemViewModel
    {
        public PublicAccessToolBarItemViewModel(string header, string imageSource)
            : base(header, imageSource, header)
        { }

        public PublicAccessToolBarItemViewModel(string header, string imageSource, string toolTip)
            : base(header, imageSource, toolTip)
        { }

        public override bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                _isEnabled = value;
                base.OnPropertyChanged("IsEnabled");
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return base._isReadOnly;
            }
            set
            {
                base.IsReadOnly = value;
            }
        }
    }

    /// <summary>
    /// Sepparator for Toolbaritems
    /// </summary>
    public class ToolBarItemSeparatorViewModel : ToolBarItemViewModel
    {
        private ToolBarItemSeparatorViewModel()
            : base(null, null)
        { }

        private static readonly ToolBarItemSeparatorViewModel _instance = new ToolBarItemSeparatorViewModel();
        public static ToolBarItemSeparatorViewModel Instance
        {
            get
            {
                return _instance;
            }
        }
    }

    /// <summary>
    /// Submenu Entry which can have other ToolBarItems as Itemssource
    /// </summary>
    public class SubMenuToolBarItemViewModel : ToolBarItemViewModel
    {
        #region Fields

        private ObservableCollection<ToolBarItemViewModel> _toolBarItems;
        public ObservableCollection<ToolBarItemViewModel> ToolBarItems
        {
            get
            {
                if (_toolBarItems == null)
                    _toolBarItems = new ObservableCollection<ToolBarItemViewModel>();

                return _toolBarItems;
            }

            set
            {
                _toolBarItems = value;
                base.OnPropertyChanged("ToolBarItems");
            }
        }

        #endregion

        #region Constructor

        public SubMenuToolBarItemViewModel(string header, string imageSource)
            : base(header, imageSource)
        { }

        #endregion
    }

    /// <summary>
    /// Public Access Submenu Entry which can have other ToolBarItems as Itemssource
    /// </summary>
    public class PublicAccessSubMenuToolBarItemViewModel : SubMenuToolBarItemViewModel
    {
        public PublicAccessSubMenuToolBarItemViewModel(string header, string imageSource)
            : base(header, imageSource)
        { }

        public override bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                _isEnabled = value;
                base.OnPropertyChanged("IsEnabled");
            }
        }
    }

    public class SearchTextBoxToolBarItemViewModel : PublicAccessToolBarItemViewModel
    {
        #region Fields

        public string SearchText
        { get; set; }

        private ICommand _executeKeyCommand;
        public ICommand ExecuteKeyCommand
        {
            get
            {
                if (_executeKeyCommand == null)
                    _executeKeyCommand = new RelayCommand<KeyEventArgs>(OnCommandExecuted);

                return _executeKeyCommand;
            }
        }

        #endregion

        #region Private Helpers

        private void OnCommandExecuted(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                base.OnCommandExecuted();
        }

        #endregion

        #region Constructor

        public SearchTextBoxToolBarItemViewModel(string header)
            : base(header, null)
        {
        }

        public SearchTextBoxToolBarItemViewModel(string header, string toolTip)
            : base(header, null, toolTip)
        {
        }

        #endregion
    }
}

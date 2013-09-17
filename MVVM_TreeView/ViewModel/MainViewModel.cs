using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using MVVM_TreeView.Core;

namespace MVVM_TreeView.ViewModel
{
    /// <summary>
    /// ViewModel for the MainWindow of the Application
    /// </summary>
    public class MainWindowViewModel : CloseableViewModel
    {
        #region Fields

        public override string DisplayName
        {
            get
            {
                return String.Format("The Name");
            }
        }

        private bool _isEnabled = true;
        public bool IsEnabled
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

        #endregion

        #region MenuItems

        private ObservableCollection<ToolBarItemViewModel> _configurationMenuItems;
        public ObservableCollection<ToolBarItemViewModel> ConfigurationMenuItems
        {
            get
            {
                return _configurationMenuItems;
            }
        }

        #endregion

        #region IsAllowedToClose

        public override bool IsAllowedToClose
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Handle Closing Event of Window
        /// </summary>
        public ICommand WindowClosing
        {
            get
            {
                return new RelayCommand<CancelEventArgs>(
                    (args) =>
                    {
                        if (!IsAllowedToClose)
                        {
                            args.Cancel = true;
                        }
                    });
            }
        }

        #endregion // Commands

        #region Constructor

        public MainWindowViewModel()
        {
        }

        #endregion

        #region Private Helpers

        private void RefreshToolBarItems()
        {
            // Clear Items
            base.ToolBarItems.Clear();


            base.ToolBarItems.Add(ToolBarItemSeparatorViewModel.Instance);
        }
        #endregion

        #region EventHandlers

        private void OnWorkspaceToolBarItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshToolBarItems();
        }

        #endregion

        #region Cleanup

        protected override void OnDispose()
        {
            base.OnDispose();
        }

        #endregion

    }
}
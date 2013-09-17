using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace MVVM_TreeView.Core
{
    public class CloseableViewModel : ViewModelBase
    {
        #region Fields

        RelayCommand _closeCommand;

        bool _isCloseable = true;
        public bool IsCloseable
        {
            get
            {
                return _isCloseable;
            }
            set
            {
                _isCloseable = value;
                base.OnPropertyChanged("IsCloseable");
            }
        }

        public virtual bool IsAllowedToClose
        {
            get
            {
                return true;
            }
        }

        #endregion // Fields

        #region Constructor

        protected CloseableViewModel()
        {
        }

        #endregion // Constructor

        #region CloseCommand

        /// <summary>
        /// Returns the Closecommand
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(OnRequestClose);

                return _closeCommand;
            }
        }

        #endregion // CloseCommand

        #region RequestClose [event]

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        void OnRequestClose()
        {
            if (this.IsAllowedToClose)
            {
                EventHandler handler = this.RequestClose;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
        }

        #endregion // RequestClose [event]

        #region ToolBarItems

        private ObservableCollection<ToolBarItemViewModel> _toolBarItems = new ObservableCollection<ToolBarItemViewModel>();
        public ObservableCollection<ToolBarItemViewModel> ToolBarItems
        {
            get
            {
                return _toolBarItems;
            }
        }

        #endregion
    }
}

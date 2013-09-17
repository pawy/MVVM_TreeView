using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Splasher;

namespace MVVM_TreeView.Core
{
    /// <summary>
    ///   Contains helper methods for UI, so far just one for showing a waitcursor
    /// </summary>
    public static class UiService
    {
        #region Dispatcher MousePointerHandler
        /// <summary>
        ///   A value indicating whether the UI is currently busy
        /// </summary>
        private static bool IsBusy;

        /// <summary>
        /// Sets the busystate as busy.
        /// </summary>
        public static void SetBusyState()
        {
            SetBusyState(true);
        }

        /// <summary>
        /// Sets the busystate to busy or not busy.
        /// </summary>
        /// <param name="busy">if set to <c>true</c> the application is now busy.</param>
        private static void SetBusyState(bool busy)
        {
            if (busy != IsBusy && Application.Current != null)
            {
                IsBusy = busy;

                if (IsBusy)
                {
                    new DispatcherTimer(TimeSpan.FromSeconds(0), DispatcherPriority.ApplicationIdle, dispatcherTimer_Tick, Application.Current.Dispatcher);
                }
            }
            Mouse.OverrideCursor = busy ? Cursors.Wait : null;
        }

        /// <summary>
        /// Handles the Tick event of the dispatcherTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var dispatcherTimer = sender as DispatcherTimer;
            if (dispatcherTimer != null)
            {
                SetBusyState(false);
                dispatcherTimer.Stop();
                LoadingFinished();
            }
        }
        #endregion

        #region Loading

#if DEBUG
        private static DateTime startTime;
#endif

        private static string _finishedMessage;

        /// <summary>
        /// Show Wait Cursor while loading
        /// Display a Message while loading and display another message when finished
        /// </summary>
        /// <param name="loadingMessage">The Message displayed while loading</param>
        /// <param name="finishedMessage">The Message displayed after loading. 
        /// If String.Empty, the message will not be overwritten when finished</param>
        public static void Loading(string loadingMessage, string finishedMessage)
        {
            Loading(loadingMessage);
            _finishedMessage = finishedMessage;
        }

        /// <summary>
        /// Show wait cursor while loading
        /// Display a Message while loading and erase the message when finished
        /// </summary>
        /// <param name="loadingMessage">The Message displayed while loading</param>
        public static void Loading(string loadingMessage)
        {
#if DEBUG
            startTime = DateTime.Now;
            Debug.WriteLine(String.Format("Loading: {0}", loadingMessage));
#endif
            SetBusyState();
            MessageListener.Instance.ReceiveMessage(loadingMessage);
        }

        private static void LoadingFinished()
        {
            LoadingFinished(null);
        }

        /// <summary>
        /// Reset the Message
        /// </summary>
        public static void ResetLoadingMessage()
        {
            MessageListener.Instance.ReceiveMessage("");
        }

        /// <summary>
        /// Set the Message to display status progress
        /// </summary>
        /// <param name="message"></param>
        public static void SetLoadingMessage(string message)
        {
            MessageListener.Instance.ReceiveMessage(message);
        }

        /// <summary>
        /// Reset message
        /// </summary>
        /// <param name="finishedMessage">the message to set</param>
        public static void LoadingFinished(string finishedMessage)
        {
#if DEBUG
            Debug.WriteLine(String.Format("Loading Finished: {0} {1}", finishedMessage ?? _finishedMessage, (DateTime.Now - startTime).Duration()));
#endif

            //Set message
            if (finishedMessage != null)
            {
                MessageListener.Instance.ReceiveMessage(finishedMessage);
            }
            //Do not overwrite the Message
            else if (_finishedMessage == String.Empty)
            {
            }
            //Do overwrite the message
            else if (_finishedMessage != null)
            {
                MessageListener.Instance.ReceiveMessage(_finishedMessage);
            }
            //Reset the message
            else
            {
                MessageListener.Instance.ReceiveMessage("");
            }
        }

        #endregion

    }
}

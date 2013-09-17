using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;

namespace Splasher
{
    /// <summary>
    /// Message listener, singlton pattern.
    /// Inherit from DependencyObject to implement DataBinding.
    /// </summary>
    public class MessageListener : DependencyObject
    {
        private static List<string> _messages = new List<string>();
        public static List<string> funnyLoadingMessages
        {
            get
            {

                if(_messages.Count > 0)
                    return _messages;

                //_messages = File.ReadAllLines(ConfigurationManager.AppSettings["LoadingMessagesFile"], Encoding.UTF8).ToList();
                /*_messages = new List<string>()
                    {
                        "Spinning up the hamster...", "Searching for Answer to Life, the Universe, and Everything...", 
                        "Now presented in double-vision, if drunk!", "Generating rumors...", "We're testing your patience...","Would you like fries with that?", 
                        "Warming up Large Hadron Collider...", "Hang on a sec, I know your data is here somewhere...","HELP!, I'm being held hostage, and forced to write the stupid lines!",
                    };*/

                if(_messages.Count == 0)
                    _messages.Add("");
                return _messages;
            }
        }

        private bool _displayAdditionalMessage = true;
        /// <summary>
        /// Display the LoadingMessages
        /// </summary>
        public bool DisplayAdditionalMessage
        {
            get
            {
                return _displayAdditionalMessage;
            }

            set
            {
                Message = "";
                _displayAdditionalMessage = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static MessageListener mInstance;

        /// <summary>
        /// 
        /// </summary>
        private MessageListener ( )
        {

        }

        /// <summary>
        /// Get MessageListener instance
        /// </summary>
        public static MessageListener Instance
        {
            get
            {
                if ( mInstance == null )
                    mInstance = new MessageListener ( );
                return mInstance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private int currentMessageNumber = getRandomNumber();
        public void ReceiveMessage ( string message )
        {
            Message = message;
            if(DisplayAdditionalMessage)
                Message += "\n\n" + funnyLoadingMessages[currentMessageNumber];
            Debug.WriteLine ( Message );
            DispatcherHelper.DoEvents ( );
        }

        private static int getRandomNumber()
        {
            var r = new Random();
            int i = r.Next(funnyLoadingMessages.Count - 1);
            return i;
        }

        public void RandomFunnyMessage()
        {
            Message = funnyLoadingMessages[getRandomNumber()];
            Debug.WriteLine(Message);
            DispatcherHelper.DoEvents();
        }
       
        /// <summary>
        /// Get or set received message
        /// </summary>
        public string Message
        {
            get { return ( string ) GetValue ( MessageProperty ); }
            set { SetValue ( MessageProperty, value ); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register ( "Message", typeof ( string ), typeof ( MessageListener ), new UIPropertyMetadata ( null ) );

    }
}

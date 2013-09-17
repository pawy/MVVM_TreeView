using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Splasher;

namespace MVVM_TreeView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App application;

        /// <summary>
        /// Startup Application and show Splashscreen
        /// </summary>
        [System.STAThread()]
        static void Main()
        {
            //Initialize SplashWindow
            var splashWindow = new View.SplashWindow
            {
                DataContext = new ViewModel.SplashScreenViewModel()
            };

            //Create Splasher with MessageListener
            Splasher.Splasher.Splash = splashWindow;
            Splasher.Splasher.ShowSplash();
            if (application == null)
            {
                Core.UiService.Loading("Initializing WPF ...");
                application = new App();
                application.InitializeComponent();
                application.Run();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Core.UiService.Loading("Lade Startbildschirm ...");

            var window = new View.MainWindow();

            // Create the ViewModel to which 
            // the main window binds.
            var viewModel = new ViewModel.MainWindowViewModel();

            // When the ViewModel asks to be closed, 
            // close the window.
            EventHandler handler = null;
            handler = delegate
            {
                viewModel.RequestClose -= handler;
                window.Close();
            };
            viewModel.RequestClose += handler;

            // Allow all controls in the window to 
            // bind to the ViewModel by setting the 
            // DataContext, which propagates down 
            // the element tree.
            window.DataContext = viewModel;

            //Initialize Assemblies in Background
            var loadAssemblies = new BackgroundWorker();
            loadAssemblies.DoWork += delegate
            {
                //Assembly assembly = Assembly.LoadFrom("WPFToolkit.dll");
                //Initialize whatever you like here for faster startup
            };
            loadAssemblies.RunWorkerAsync();

            window.Show();
            handler = delegate
            {
                window.ContentRendered -= handler;
                MessageListener.Instance.DisplayAdditionalMessage = false;
                Splasher.Splasher.CloseSplash();
                Core.UiService.LoadingFinished("Bereit!");

                //Check for an Update & Clear Message
                var Timer = new DispatcherTimer();
                Timer.Tick += delegate
                {
                    //Clear Status Message
                    Core.UiService.ResetLoadingMessage();
#if !DEBUG
                    //Check for Update for One-Click-Deployment
                    /*try
                    {
                        if (System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CheckForUpdate())
                        {
                            Core.UiService.LoadingFinished("Update Available");

                            if (!_updateMessageShowed)
                                MessageScreen.Show("New Update available, please reopen!");
                            _updateMessageShowed = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Core.UiService.LoadingFinished("Started from Visual Studio but not in Debug-Mode! Please start in Debug-Mode!");
                    }*/
#endif
                };
                Timer.Interval = TimeSpan.FromSeconds(10);
                Timer.Start();
            };
            window.ContentRendered += handler;
        }
    }
}

using MVVM_TreeView.Core;

namespace MVVM_TreeView.ViewModel
{
    public class SplashScreenViewModel : ViewModelBase
    {
        #region Fields

        public string Version
        {
            get
            {
                return "DEV";
                //This is for One-Click-Deployment
                /*try
                {
                    return "V " + System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                }
                catch (Exception ex)
                {
                    return "DEV";
                }*/
            }

        }

        #endregion
    }
}

using EasySave.src.Utils;

namespace EasySave.src.ViewModels
{
    /// <summary>
    /// HomeViewModel class
    /// </summary>
    public class HomeViewModel
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public HomeViewModel()
        {
        }

        /// <summary>
        /// check for updates
        /// </summary>
        /// <returns>bool if up to date</returns>
        public static bool IsUpToDate()
        {
            return !VersionUtils.CompareVersions();
        }

    }

}
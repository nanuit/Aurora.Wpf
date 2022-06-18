using System.Windows;

namespace Aurora.Wpf
{
    public class MessageEx
    {
        #region Static Properties
        public static string Title { get; set; } = "Program";
        #endregion
        public static MessageBoxResult ShowErrorMessage(string message)
        {
            return (ShowMessage(message, Title, MessageBoxButton.OK, MessageBoxImage.Error));
        }
        public static MessageBoxResult ShowErrorMessage(string message, string title)
        {
            return (ShowMessage(message, title, MessageBoxButton.OK, MessageBoxImage.Error));
        }
        public static MessageBoxResult ShowMessage(string message)
        {
            return (ShowMessage(message, Title, MessageBoxButton.OK, MessageBoxImage.Information));
        }

        /// <summary>
        ///     Display a Messagebox.
        ///     If a progressWindow is shown it is hidden during Message display
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title for the MessageBox</param>
        /// <param name="buttons">MessageButtons to be displayed</param>
        /// <param name="icon">Message Icon to be displayed</param>
        /// <returns>MessageBoxResult of the MessageBox</returns>
        public static MessageBoxResult ShowMessage(string message, string title, MessageBoxButton buttons, MessageBoxImage icon)
        {
            return (MessageBox.Show(message, title, buttons, icon));
        }
    }
}

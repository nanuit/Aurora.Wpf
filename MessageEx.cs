using System.Windows;

namespace Aurora.Wpf
{
    /// <summary>
    /// display default message box
    /// </summary>
    public class MessageEx
    {
        #region Static Properties
        public static string Title { get; set; } = "Program";
        #endregion
        /// <summary>
        /// Display a Messagebox.
        ///  with ok Button and Error Icon and the <see cref="Title"/> as message title
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        /// <returns>MessageBoxResult of the MessageBox</returns>
        public static MessageBoxResult ShowErrorMessage(string message)
        {
            return (ShowMessage(message, Title, MessageBoxButton.OK, MessageBoxImage.Error));
        }

        /// <summary>
        /// Display a Messagebox.
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title for the MessageBox</param>
        /// <returns>MessageBoxResult of the MessageBox</returns>
        public static MessageBoxResult ShowErrorMessage(string message, string title)
        {
            return (ShowMessage(message, title, MessageBoxButton.OK, MessageBoxImage.Error));
        }
        /// <summary>
        /// Display a Messagebox.
        ///  with ok Button and Information Icon and the <see cref="Title"/> as message title
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        /// <returns>MessageBoxResult of the MessageBox</returns>
        public static MessageBoxResult ShowMessage(string message)
        {
            return (ShowMessage(message, Title, MessageBoxButton.OK, MessageBoxImage.Information));
        }

        /// <summary>
        /// Display a Messagebox.
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

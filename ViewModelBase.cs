using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Aurora.Wpf
{
    /// <summary>
    /// Base class for view models implementing the INotifyPropertyChanged Interface
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChange
        public event PropertyChangedEventHandler? PropertyChanged;
        /// <summary>
        /// Eventmethod to invoke a property change
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        /// <summary>
        /// Verify that the property name matches a real, public, instance property on this object. 
        /// </summary>
        /// <param name="propertyName"></param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;
                //if (this.ThrowOnInvalidPropertyName)
                //    throw new Exception(msg);
                //else
                    Debug.Fail(msg);
            }
        }
    }
}

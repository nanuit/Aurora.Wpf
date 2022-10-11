using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Aurora.Wpf
{
    /// <summary>
    /// Class to implement the command execution for MVVM
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields
        private readonly Action<object>? m_Execute;
        private readonly Predicate<object>? m_CanExecute;
        #endregion // Fields 
        #region Constructors

        /// <summary>
        /// create an instance for command execution
        /// </summary>
        /// <param name="execute">Action to be executed</param>
        /// <param name="canExecute">Predicate to determine if the command can be executed</param>
        /// <exception cref="ArgumentNullException">execute parameter cannot be null</exception>
        public RelayCommand(Action<object> execute, Predicate<object>? canExecute = null)
        {
            m_Execute = execute ?? throw new ArgumentNullException(nameof(execute)); 
            m_CanExecute = canExecute;
        }
        #endregion 
        #region ICommand Members 
        /// <summary>
        /// invoke the Predicate to determine if the command can be executed
        /// </summary>
        /// <param name="parameter">parameter to pass to predicate</param>
        /// <returns></returns>
        //[DebuggerStepThrough]
        public bool CanExecute(object? parameter)
        {
            bool retVal = m_CanExecute?.Invoke(parameter!) ?? true;
            return retVal;
        }
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        /// <summary>
        /// Invoke the execution command
        /// </summary>
        /// <param name="parameter">parameter to pass to execution action</param>
        public void Execute(object? parameter)
        {
            m_Execute?.Invoke(parameter);
        }
        #endregion 
    }
}

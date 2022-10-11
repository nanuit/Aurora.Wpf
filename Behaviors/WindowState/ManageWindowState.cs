using System;
using System.Windows;

using Aurora.Configs;
#if NET48
using System.Windows.Interactivity;
#else
using Microsoft.Xaml.Behaviors;
#endif


namespace Aurora.Wpf.Behaviors.WindowState
{
    /// <summary>
    /// Class to store and recover the window state of a wpf window to and from a jsonfile in the userprofile
    /// </summary>
    public class ManagePositions : Behavior<Window>
    {
        #region Private Members
        private StoredWindowState? m_State;
        private ConfigJson<StoredWindowState>? m_Config;
        #endregion
        /// <summary>
        /// XAML Property to set the activation state of the behavior
        /// </summary>
        public static readonly DependencyProperty ActivatedProperty =
            DependencyProperty.Register(
                nameof(Activated),
                typeof(bool),
                typeof(ManagePositions),
                new PropertyMetadata(OnActivatedChanged)
            );
        /// <summary>
        /// XAML Property to set the context. it determines the json file name
        /// </summary>
        public static readonly DependencyProperty ContextProperty =
            DependencyProperty.Register(
                nameof(Context),
                typeof(string),
                typeof(ManagePositions),
                new PropertyMetadata(OnContextChanged)
            );
        #region Properties
        /// <summary>
        /// Indicator if the behavior is active
        /// </summary>
        public bool Activated
        {
            get => (bool)GetValue(ActivatedProperty);
            set => SetValue(ActivatedProperty, value);
            
        }
        /// <summary>
        /// Cóntext name 
        /// </summary>
        public string Context
        {
            get => GetValue(ContextProperty).ToString();
            set => SetValue(ActivatedProperty, value);
        }

        #endregion
        #region Protected Methods

        /// <summary>
        /// event method for Activated Property change
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="e"></param>
        protected static void OnActivatedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            
        }
        /// <summary>
        /// event method for Context Property change
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="e"></param>
        protected static void OnContextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {   
        }
        /// <summary>
        /// event method when the behavior is attached to a window
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.Closing += OnClosing;
            InitConfig();
            SetState();
        }
        /// <summary>
        /// event method when the behavior is detached from a window
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.Closing -= OnClosing;
        }

        private void OnClosing(object sender, EventArgs eventArgs)
        {
            if (!Activated)
                return;
            InitConfig();
            if (m_State == null)
                return;
            m_State.Height = Math.Round(AssociatedObject.Height);
            m_State.Width = Math.Round(AssociatedObject.Width);
            m_State.Top = Math.Round(AssociatedObject.Top);
            m_State.Left = Math.Round(AssociatedObject.Left);
            m_State.State = AssociatedObject.WindowState;
            m_State.Stored = true;
            m_Config?.Save();
        }

        #endregion
        private void InitConfig()
        {
            if (!Activated)
                return;
            m_Config ??= new ConfigJson<StoredWindowState>(ConfigType.LocalProfile, string.IsNullOrEmpty(Context) ? "WindowState" : Context, AssociatedObject.Title);
            m_State ??= m_Config.Load();
        }

        private void SetState()
        {
            if (!Activated || !m_State.Stored)
                return;
            if (m_State.Height > System.Windows.SystemParameters.VirtualScreenHeight)
                m_State.Height = System.Windows.SystemParameters.VirtualScreenHeight;

            if (m_State.Width > System.Windows.SystemParameters.VirtualScreenWidth)
                m_State.Width = System.Windows.SystemParameters.VirtualScreenWidth;

            if (m_State.Top + m_State.Height / 2 > System.Windows.SystemParameters.VirtualScreenHeight)
                m_State.Top = System.Windows.SystemParameters.VirtualScreenHeight - m_State.Height;

            if (m_State.Left + m_State.Width / 2 > System.Windows.SystemParameters.VirtualScreenWidth)
                m_State.Left = System.Windows.SystemParameters.VirtualScreenWidth - m_State.Width;
            
            if (m_State.Top < 0)
                m_State.Top = 0;

            if (m_State.Left < 0)
                m_State.Left = 0;

            AssociatedObject.Left = m_State.Left;
            AssociatedObject.Width = m_State.Width;
            AssociatedObject.Top = m_State.Top;
            AssociatedObject.Height = m_State.Height;
            AssociatedObject.WindowState = m_State.State;
        }
    }
}

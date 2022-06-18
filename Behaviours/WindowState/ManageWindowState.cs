using System;
using System.Windows;
using System.Windows.Interactivity;
using Aurora.Configs;


namespace Aurora.Wpf.Behaviours.WindowState
{
    public class ManagePositions : Behavior<Window>
    {
        #region Private Members
        private StoredWindowState m_State;
        private ConfigJson<StoredWindowState> m_Config;
        #endregion
        public static readonly DependencyProperty ActivatedProperty =
            DependencyProperty.Register(
                "Activated",
                typeof(bool),
                typeof(ManagePositions),
                new PropertyMetadata(OnActivatedChanged)
            );

        public static readonly DependencyProperty ContextProperty =
            DependencyProperty.Register(
                "Context",
                typeof(string),
                typeof(ManagePositions),
                new PropertyMetadata(OnContextChanged)
            );
        #region Properties
        public bool Activated
        {
            get => (bool)GetValue(ActivatedProperty);
            set => SetValue(ActivatedProperty, value);
            
        }
        public string Context
        {
            get => GetValue(ContextProperty).ToString();
            set => SetValue(ActivatedProperty, value);

        }

        #endregion
        #region Protected Methods
        static void OnActivatedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            
        }
        static void OnContextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {   
        }
        protected override void OnAttached()
        {
            AssociatedObject.Closing += OnClosing;
            InitConfig();
            SetState();
        }
       
        protected override void OnDetaching()
        {
            AssociatedObject.Closing -= OnClosing;
        }

        void OnClosing(object sender, EventArgs eventArgs)
        {
            if (!Activated)
                return;
            InitConfig();

            m_State.Height = Math.Round(AssociatedObject.Height);
            m_State.Width = Math.Round(AssociatedObject.Width);
            m_State.Top = Math.Round(AssociatedObject.Top);
            m_State.Left = Math.Round(AssociatedObject.Left);
            m_State.State = AssociatedObject.WindowState;
            m_State.Stored = true;
            m_Config.Save();
        }

        #endregion
        private void InitConfig()
        {
            if (!Activated)
                return;
            if (m_Config == null)
                m_Config = new ConfigJson<StoredWindowState>(ConfigType.LocalProfile, string.IsNullOrEmpty(Context) ? "WindowState" : Context, AssociatedObject.Title);
            if (m_State == null)
                m_State = m_Config.Load();
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

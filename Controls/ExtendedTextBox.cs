using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aurora.Wpf.Controls
{
    /// <summary>
    /// Extended TextBox Control to add mouse wheel zoom functionality
    /// </summary>
    public class ExtendedTextBox :TextBox
    {
        #region To Life and Die in Starlight
        /// <summary>
        /// register mouse wheel event
        /// </summary>
        public ExtendedTextBox()
        {
            PreviewMouseWheel += OnMouseWheel;
        }
        #endregion

        #region MouseWheelZoom
        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            // We'll need a DoubleAnimation object to drive
            // the ScaleX and ScaleY properties.
            if (MouseWheelFontZoom)
            {
                if (e.Delta > 0)
                    FontSize++;
                else
                    FontSize = Math.Max(FontSize - 1, 1);
            }

        }
        /// <summary>
        /// Dependency property to enable XAML element
        /// </summary>
        public static readonly DependencyProperty MouseWheelFontZoomProperty =
            DependencyProperty.Register(nameof(MouseWheelFontZoom), typeof(bool), typeof(ExtendedTextBox), new UIPropertyMetadata(false));
        /// <summary>
        /// Property controlling the active state of the mouse wheel zoom
        /// </summary>
        public bool MouseWheelFontZoom
        {
            get => (bool)GetValue(MouseWheelFontZoomProperty);
            set => SetValue(MouseWheelFontZoomProperty, value);
        }
        #endregion
    }
}

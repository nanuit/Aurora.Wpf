using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aurora.Wpf.Controls
{
    public class ExtendedTextBox :TextBox
    {
        #region To Life and Die in Starlight

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
        public static readonly DependencyProperty MouseWheelFontZoomProperty =
            DependencyProperty.Register("MouseWheelFontZoom", typeof(bool), typeof(ExtendedTextBox), new UIPropertyMetadata(false));
        public bool MouseWheelFontZoom
        {
            get => (bool)GetValue(MouseWheelFontZoomProperty);
            set => SetValue(MouseWheelFontZoomProperty, value);
        }
        #endregion
    }
}

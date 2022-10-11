using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Aurora.Wpf.Controls
{
    /// <summary>
    /// Class to enable mouse wheel zoom on a UserControl
    /// </summary>
    public class ZoomUserControl : UserControl
    {
        /// <summary>
        /// register for mouse wheel event
        /// </summary>
        protected ZoomUserControl()
        {
            PreviewMouseWheel += OnMouseWheel;
        }
        private float m_ScaleFactor = 1.0f;
        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            if (!(this.LayoutTransform is ScaleTransform scaler))
            {
                scaler = new ScaleTransform(1.0, 1.0);
                this.LayoutTransform = scaler;
            }

            // We'll need a DoubleAnimation object to drive
            // the ScaleX and ScaleY properties.
            if (MouseWheelFontZoom)
            {
                DoubleAnimation animator = new DoubleAnimation()
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(600)),
                };
                if (e.Delta > 0)
                    m_ScaleFactor += 0.1f;
                else
                    m_ScaleFactor -= 0.1f;

                animator.To = m_ScaleFactor;
                scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
                scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);
            }
            else
            {
                if (e.Delta > 0)
                    FontSize ++;
                else
                    FontSize = Math.Max(FontSize - 1, 1);
            }
        }
        public static readonly DependencyProperty MouseWheelFontZoomProperty =
            DependencyProperty.Register(nameof(MouseWheelFontZoom), typeof(bool), typeof(ZoomUserControl), new UIPropertyMetadata(false));
        /// <summary>
        /// Property controlling the active state of the mouse wheel zoom
        /// </summary>
        public bool MouseWheelFontZoom
        {
            get => (bool)GetValue(MouseWheelFontZoomProperty);
            set => SetValue(MouseWheelFontZoomProperty, value);
        }
    }
}

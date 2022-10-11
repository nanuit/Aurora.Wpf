namespace Aurora.Wpf.Behaviors.WindowState
{
    /// <summary>
    /// class holding the windows state of a wpf window
    /// </summary>
    public class StoredWindowState
    {
        /// <summary>
        /// height of the window
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Width of the window
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Top position of the window
        /// </summary>
        public double Top { get; set; }
        /// <summary>
        /// Left position of the window
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// State of the window
        /// </summary>
        public System.Windows.WindowState State { get; set; }

        /// <summary>
        /// indicator if the state has been stored
        /// </summary>
        public bool Stored { get; set; } = false;
    }
}

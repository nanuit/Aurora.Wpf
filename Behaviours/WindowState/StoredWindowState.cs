namespace Aurora.Wpf.Behaviours.WindowState
{
    public class StoredWindowState
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }

        public System.Windows.WindowState State { get; set; }

        public bool Stored { get; set; } = false;
    }
}

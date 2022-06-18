using System;
using System.Windows;
using System.Windows.Interop;

namespace Aurora.Wpf.Device
{
    public class DeviceMonitor : IDisposable
    {
        #region Event

        public delegate void DeviceEventHandler(DeviceData device);
        public event DeviceEventHandler DeviceArrival;
        public event DeviceEventHandler DeviceRemoval;

        public void OnDeviceArrival(DeviceData device)
        {
            DeviceArrival?.Invoke(device);
        }
        public void OnDeviceRemoval(DeviceData device)
        {
            DeviceRemoval?.Invoke(device);
        }
        #endregion
        #region Private members

        private DeviceNotification.DbtDeviceType m_DeviceType;
        private bool m_FilterByType = false;
        #endregion
        #region To Life and Die in starlight

        public DeviceMonitor(Window window, DeviceNotification.DbtDeviceType deviceType)
        {
            m_DeviceType = deviceType;
            m_FilterByType = true;
            Init(window);
        }
        public DeviceMonitor(Window window)
        {
            Init(window);
        }
        #endregion

        #region Private Methods
        private void Init(Window window)
        {
            // Adds the windows message processing hook and registers USB device add/removal notification.
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(window).Handle);
            if (source != null)
            {
                var windowHandle = source.Handle;
                source.AddHook(HwndHandler);
                DeviceNotification.RegisterDeviceNotification(windowHandle);
            }
        }

        private void Close()
        {
            DeviceNotification.UnregisterDeviceNotification();
        }
        /// <summary>
        /// Method that receives window messages.
        /// </summary>
        private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == DeviceNotification.c_WmDevicechange)
            {
                DeviceNotification.DbtEventType eventType = (DeviceNotification.DbtEventType)wparam;
                DeviceData device = DeviceNotification.GetDevice(lparam);
                if (!m_FilterByType || device.DeviceType == m_DeviceType)
                {
                    if (eventType == DeviceNotification.DbtEventType.Arrival)
                        OnDeviceArrival(device);
                    else if (eventType == DeviceNotification.DbtEventType.RemoveComplete)
                        OnDeviceRemoval(device);
                }
            }
            handled = false;
            return IntPtr.Zero;
        }

        #endregion
        #region Public Methods
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Close();
        }
        #endregion
    }
}

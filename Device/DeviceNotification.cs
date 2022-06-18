using System;
using System.Runtime.InteropServices;

namespace Aurora.Wpf.Device
{
    public static class DeviceNotification
    {
        public enum DbtDeviceType
        {
            DeviceInterface = 5,
            Handle = 6,
            Oem = 0,
            Port = 3,
            Volume = 2
        }

        public enum DbtEventType
        {
            Arrival = 0x8000,
            RemoveComplete = 0x8004,
            NodeChanged = 7,
            TypeSpecific = 0x8005
        }

        public enum Dbtf
        {
            Media = 1,
            Net = 2
        }

        //https://msdn.microsoft.com/en-us/library/aa363480(v=vs.85).aspx
        
        
        public const int c_WmDevicechange = 0x0219; // device change event      
        
        
        //https://msdn.microsoft.com/en-us/library/aa363431(v=vs.85).aspx
        private const int c_Device_Notify_All_Interface_Classes = 4;
        private static readonly Guid GuidDevinterfaceUsbDevice = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"); // USB devices
        private static IntPtr m_NotificationHandle;

        /// <summary>
        /// Registers a window to receive notifications when devices are plugged or unplugged.
        /// </summary>
        /// <param name="windowHandle">Handle to the window receiving notifications.</param>
        /// <param name="usbOnly">true to filter to USB devices only, false to be notified for all devices.</param>
        public static void RegisterDeviceNotification(IntPtr windowHandle, bool usbOnly = false)
        {
            var dbi = new DevBroadcastDeviceinterfaceFixed()
            {
                Devicetype = (int)DbtDeviceType.DeviceInterface,
                Reserved = 0,
                Classguid = GuidDevinterfaceUsbDevice,
                Name = (char)0
            };
            
            dbi.Size = Marshal.SizeOf(dbi);
            IntPtr buffer = Marshal.AllocHGlobal(dbi.Size);
            Marshal.StructureToPtr(dbi, buffer, true);

            m_NotificationHandle = RegisterDeviceNotification(windowHandle, buffer, usbOnly ? 0 : c_Device_Notify_All_Interface_Classes);
        }

        /// <summary>
        /// Unregisters the window for device notifications
        /// </summary>
        public static void UnregisterDeviceNotification()
        {
            UnregisterDeviceNotification(m_NotificationHandle);
        }

        public static DeviceData GetDevice(IntPtr lParam)
        {
            DeviceData itemData = new DeviceData();
            if ((int)lParam == 0)
                return itemData;
            
            DevBroadcastHdr hdr = new DevBroadcastHdr();
            Marshal.PtrToStructure(lParam, hdr);
            itemData.DeviceType = ((DbtDeviceType)hdr.dbch_devicetype);
            switch ((DbtDeviceType)hdr.dbch_devicetype)
            {
                case DbtDeviceType.DeviceInterface:
                    DevBroadcastDeviceinterfaceVariable devIf = new DevBroadcastDeviceinterfaceVariable();

                    // Convert lparam to DEV_BROADCAST_DEVICEINTERFACE structure
                    Marshal.PtrToStructure(lParam, devIf);

                    // Get the device path from the broadcast message
                    itemData.Data = new string(devIf.dbcc_name);

                    // Remove null-terminated data from the string
                    int pos = itemData.Data.IndexOf((char)0);
                    if (pos != -1)
                    {
                        itemData.Data = itemData.Data.Substring(0, pos);
                    }
                    break;
                case DbtDeviceType.Port:
                    DevBroadcastPortVariable devPort = new DevBroadcastPortVariable();

                    // Convert lparam to DEV_BROADCAST_DEVICEINTERFACE structure
                    Marshal.PtrToStructure(lParam, devPort);

                    // Get the device path from the broadcast message
                    itemData.Data = new string(devPort.dbcc_name);

                    // Remove null-terminated data from the string
                    pos = itemData.Data.IndexOf((char)0);
                    if (pos != -1)
                    {
                        itemData.Data = itemData.Data.Substring(0, pos);
                    }
                    break;
                case DbtDeviceType.Volume:
                    DevBroadcastVolume volume = new DevBroadcastVolume();
                    Marshal.PtrToStructure(lParam, volume);
                    itemData.Data = $"{((volume.dbcv_flags & (int) Dbtf.Media) == 1 ? "Media" : "Net")} {FirstDriveFromMask(volume.dbcv_unitmask)}";
                    break;

                    
            }
            return (itemData);
        }

        public static char FirstDriveFromMask(int unitmask)
        {
            
            int i;

            for (i = 0; i < 26; ++i)
            {
                if ((unitmask & 0x1) == 1)
                    break;
                unitmask = unitmask >> 1;
            }

            return ((char)(i + 65));
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

        [DllImport("user32.dll")]
        private static extern bool UnregisterDeviceNotification(IntPtr handle);

        [StructLayout(LayoutKind.Sequential)]
        public class DevBroadcastDeviceinterfaceFixed
        {
            public int Size;
            public int Devicetype;
            public int Reserved;
            public Guid Classguid;
            public char Name;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class DevBroadcastDeviceinterfaceVariable
        {
            public int dbcc_size;
            public int dbcc_devicetype;
            public int dbcc_reserved;
            public Guid dbcc_classguid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
            public char[] dbcc_name;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class DevBroadcastHdr
        {
            public int dbch_size;
            public int dbch_devicetype;
            public int dbch_reserved;
        }
        [StructLayout(LayoutKind.Sequential)]
        public class DevBroadcastPortFixed
        {
            public int dbcp_size;
            public int dbcp_devicetype;
            public int dbcp_reserved;
            public char dbcc_name;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class DevBroadcastPortVariable
        {
            public int dbcp_size;
            public int dbcp_devicetype;
            public int dbcp_reserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
            public char[] dbcc_name;
        }
        [StructLayout(LayoutKind.Sequential)]
        public class DevBroadcastVolume
        {
            public int dbcv_size;
            public int dbcv_devicetype;
            public int dbcv_reserved;
            public int dbcv_unitmask;
            public char dbcv_flags;
        }
    }
}

namespace Aurora.Wpf.Device
{
    public class DeviceData
    {
            public DeviceNotification.DbtDeviceType DeviceType { get; set; } = DeviceNotification.DbtDeviceType.Oem;
            public string Data { get; set; } = string.Empty;
            
            public DeviceData()
            {
                
            }
            public override string ToString()
            {
                return string.Format("{1}{0}{2}{0}", ";", DeviceType, Data);
            }
     
    }
}

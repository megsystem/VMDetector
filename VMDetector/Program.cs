using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace VMDetector
{
    internal class Program
    {
        static string text;
        static string result;
        static void Main(string[] args)
        {
            text = "This program is running on a [RESULT]\nProgram created by @giovanni_giannone_";
            if (Detector.checkVM() ||
                Detector.checkSandboxie() ||
                Detector.checkDebugger()) result = "VM"; else result = "Real Machine";

            // print output
            Console.WriteLine(text.Replace("[RESULT]", result));
            Console.ReadKey();
            return;
        }
    }

    class Detector
    {
        // Check if in a virtual machine (like vmware and virtualbox)
        public static bool checkVM()
        {
            using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
            {
                try
                {
                    using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
                    {
                        foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
                        {
                            if ((managementBaseObject["Manufacturer"].ToString().ToLower() == "microsoft corporation" 
                                && managementBaseObject["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) || 
                                managementBaseObject["Manufacturer"].ToString().ToLower().Contains("vmware") || 
                                managementBaseObject["Model"].ToString() == "VirtualBox")
                            {
                                return true; // VM
                            }
                        }
                    }
                }
                catch
                {
                    return true; // VM
                }
            }
            foreach (ManagementBaseObject managementBaseObject2 in 
                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController").Get())
            {
                if (managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VMware") && 
                    managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VBox"))
                {
                    return true; // VM
                }
            }
            return false; // Real Machine
        }

        // SandBoxie
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        public static bool checkSandboxie()
        {
            string[] array = new string[5]
            {
                "SbieDll.dll",
                "SxIn.dll",
                "Sf2.dll",
                "snxhk.dll",
                "cmdvrt32.dll"
            };
            for (int i = 0; i < array.Length; i++)
            {
                if (GetModuleHandle(array[i]).ToInt32() != 0)
                {
                    return true; // VM
                }
            }
            return false; // Real Machine
        }

        // Debugger
        public static bool checkDebugger()
        {
            try
            {
                long ticks = DateTime.Now.Ticks;
                System.Threading.Thread.Sleep(10);
                if (DateTime.Now.Ticks - ticks < 10L)
                {
                    return true; // VM
                }
            }
            catch { }
            return false; // Real Machine
        }
    }
}

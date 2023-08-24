using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace EMCL.Modules
{
    public static class ModSerial
    {
        public class CInfo
        {
            public string CPUID
            {
                get
                {
                    try
                    {
                        string cpuInfo = "";//cpu序列号
                        ManagementClass mc = new ManagementClass("Win32_Processor");
                        ManagementObjectCollection moc = mc.GetInstances();
                        foreach (ManagementObject mo in moc)
                        {
                            cpuInfo = mo.Properties["ProcessorId"].Value.ToString()!;
                        }
                        moc = null!;
                        mc = null!;
                        return cpuInfo;
                    }
                    catch
                    {
                        return "unknown";
                    }

                    finally { }
                }
            }

            public string MAC
            {
                get
                {
                    try
                    {
                        string mac = "";
                        ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                        ManagementObjectCollection moc = mc.GetInstances();
                        foreach (ManagementObject mo in moc)
                        {
                            if ((bool)mo["IPEnabled"] == true)
                            {
                                mac = mo["MacAddress"].ToString()!;
                                break;
                            }
                        }
                        moc = null!;
                        mc = null!;
                        return mac;
                    }
                    catch
                    {
                        return "unknown";
                    }
                    finally { }
                }
            }

            public string DISK
            {
                get
                {
                    try
                    {
                        String diskID = "";
                        ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                        ManagementObjectCollection moc = mc.GetInstances();
                        foreach (ManagementObject mo in moc)
                        {
                            diskID = (string)mo.Properties["Model"].Value;
                        }
                        moc = null!;
                        mc = null!;
                        return diskID;
                    }
                    catch
                    {
                        return "unknown";
                    }
                    finally { }
                }
            }

            public string IP
            {
                get
                {
                    try
                    {
                        string st = "";
                        ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                        ManagementObjectCollection moc = mc.GetInstances();
                        foreach (ManagementObject mo in moc)
                        {
                            if ((bool)mo["IPEnabled"] == true)
                            {
                                System.Array ar;
                                ar = (System.Array)(mo.Properties["IpAddress"].Value);
                                st = ar.GetValue(0)!.ToString()!;
                                break;
                            }
                        }
                        moc = null!;
                        mc = null!;
                        return st;
                    }
                    catch
                    {
                        return "unknown";
                    }
                    finally { }
                }
            }

            public string USER
            {
                get
                {
                    try
                    {
                        string un = "";
                        un = Environment.UserName;
                        return un;
                    }
                    catch
                    {
                        return "unknown";
                    }
                    finally { }
                }
            }

            public string COMPUTER
            {
                get
                {
                    try
                    {
                        return System.Environment.MachineName;
                    }
                    catch
                    {
                        return "unknown";
                    }
                    finally { }
                }
            }

            public string SYSTEM
            {
                get
                {
                    try
                    {
                        string st = "";
                        ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                        ManagementObjectCollection moc = mc.GetInstances();
                        foreach (ManagementObject mo in moc)
                        {
                            st = mo["SystemType"].ToString()!;
                        }
                        moc = null!;
                        mc = null!;
                        return st;
                    }
                    catch
                    {
                        return "unknown";
                    }
                    finally { }
                }
            }

            public string MEMORY
            {
                get
                {
                    try
                    {
                        string st = "";
                        ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                        ManagementObjectCollection moc = mc.GetInstances();
                        foreach (ManagementObject mo in moc)
                        {
                            st = mo["TotalPhysicalMemory"].ToString()!;
                        }
                        moc = null!;
                        mc = null!;
                        return st;
                    }
                    catch
                    {
                        return "unknown";
                    }
                    finally { }
                }
            }

            public override string ToString()
            {
                return $"    CPU: {this.CPUID}\n" +
                    $"    内存: {this.MEMORY}\n" +
                    $"    MAC地址: {this.MAC}\n" +
                    $"    硬盘ID: {this.DISK}\n" +
                    $"    IP地址: {this.IP}\n" +
                    $"    当前用户名: {this.USER}\n" +
                    $"    计算机名称: {this.COMPUTER}\n" +
                    $"    系统信息: {this.SYSTEM}";
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Collections;

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
                }
            }

            public string BIOS
            {
                get
                {
                    return Identifier("Win32_BIOS", "Manufacturer")
                    + Identifier("Win32_BIOS", "SMBIOSBIOSVersion")
                    + Identifier("Win32_BIOS", "IdentificationCode")
                    + Identifier("Win32_BIOS", "SerialNumber")
                    + Identifier("Win32_BIOS", "ReleaseDate")
                    + Identifier("Win32_BIOS", "Version");
                }
            }

            public string GRAPH
            {
                get
                {
                    return Identifier("Win32_VideoController", "DriverVersion")
                    + Identifier("Win32_VideoController", "Name");
                }
            }

            public string MOTHERBOARD
            {
                get
                {
                    return Identifier("Win32_BaseBoard", "Model")
                    + Identifier("Win32_BaseBoard", "Manufacturer")
                    + Identifier("Win32_BaseBoard", "Name")
                    + Identifier("Win32_BaseBoard", "SerialNumber");
                }
            }
            private static string Identifier(string wmiClass, string wmiProperty)
            {
                string result = "";
                ManagementClass mc = new ManagementClass(wmiClass);
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if (result == "")
                    {
                        try
                        {
                            result = mo[wmiProperty].ToString()!;
                            break;
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                return result!;
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
                    $"    系统类型: {this.SYSTEM}\n" +
                    $"    主板ID: {this.MOTHERBOARD}\n" +
                    $"    显卡ID: {this.GRAPH}\n" +
                    $"    BIOS: {this.BIOS}";
            }

            public string GetCInfo()
            {
                return $"CPU: {this.CPUID}\n" +
                    $"内存: {this.MEMORY}\n" +
                    $"计算机名称: {this.COMPUTER}\n" +
                    $"系统类型: {this.SYSTEM}\n" +
                    $"主板ID: {this.MOTHERBOARD}\n" +
                    $"显卡ID: {this.GRAPH}\n" +
                    $"BIOS: {this.BIOS}";
            }

            public static explicit operator string(CInfo arg)
            {
                if (arg != null)
                {
                    return arg.ToString();
                }
                return "unknown";
            }
        }

        /// <summary>
        /// 生成计算机的唯一识别码
        /// 例: 0000-0000-0000-0000-0002-8C00-1F1E-1145
        /// </summary>
        public class FingerPrint
        {
            private string fingerPrint;
            private CInfo computer;

            public FingerPrint()
            {
                fingerPrint = string.Empty;
                this.computer = new CInfo();
            }

            public string Serial
            {
                get
                {
                    if (string.IsNullOrEmpty(fingerPrint))
                    {
                        fingerPrint = GetHash(this.computer.GetCInfo());
                    }
                    return fingerPrint;
                }
            }

            private string GetHash(string s)
            {
                SHA256 sec = SHA256.Create();
                ASCIIEncoding enc = new ASCIIEncoding();
                byte[] bt = enc.GetBytes(s);
                return GetHexString(sec.ComputeHash(bt));
            }

            private string GetHexString(byte[] bt)
            {
                string s = string.Empty;
                for (int i = 0; i < bt.Length; i++)
                {
                    byte b = bt[i];
                    int n, n1, n2;
                    n = (int)b;
                    n1 = n & 15;
                    n2 = (n >> 4) & 15;
                    if (n2 > 9)
                        s += ((char)(n2 - 10 + (int)'A')).ToString();
                    else
                        s += n2.ToString();
                    if (n1 > 9)
                        s += ((char)(n1 - 10 + (int)'A')).ToString();
                    else
                        s += n1.ToString();
                    if ((i + 1) != bt.Length && (i + 1) % 2 == 0) s += "-";
                }
                return s;
            }

            public override string ToString()
            {
                return this.Serial;
            }

            public static implicit operator string(FingerPrint arg)
            {
                return arg.ToString();
            }
        }
    }
}
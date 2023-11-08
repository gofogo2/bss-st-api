using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Launcher_SE.Helpers
{
    public static class NetworkHelper
    {
        public static string GetMyIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new Exception("error");
        }

        public static bool IsLocalHost(IPEndPoint ep)
        {
            return ep.Address.ToString().Equals(GetMyIpAddress());
        }

        public static string GetMacAddress()
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
                    byte[] bytes = physicalAddress.GetAddressBytes();

                    string macAddress = BitConverter.ToString(bytes);
                    Console.WriteLine($"MAC 주소: {macAddress}");
                    return macAddress;
                }
            }
            return null;
        }

    }
}
